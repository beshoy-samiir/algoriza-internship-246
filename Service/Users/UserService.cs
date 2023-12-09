using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Consts;
using Core.Domain.Users;
using Core.Enums;
using Core.Repository;
using Core.Service.Users;
using Core.Shared;
using static Core.Shared.Error;

namespace Service.Users
{
    public class UserService : UserManager<User>, IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private IBaseRepository<UserRefreshToken> _refreshTokenRepo;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;

        public UserService(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            SignInManager<User> signInManager,
            IBaseRepository<UserRefreshToken> refreshTokenRepo,
            IConfiguration configuration,
            RoleManager<Role> roleManager)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _signInManager = signInManager;
            _refreshTokenRepo = refreshTokenRepo;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public new async Task<Result<bool>> UpdateUserAsync(User newUserData)
        {
            User? user = await FindByIdAsync(newUserData.Id.ToString());

            if (user == null)
            {
                return Result.Failure<bool>(Errors.Users.UserNotFound());
            }

            user.Update(newUserData);

            IdentityResult updateResult = await UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Result.Failure<bool>(MapIdentityErrorToAppError(updateResult));
            }

            return Result.Success(true);
        }

        public async Task<Result<bool>> DeleteUserAsync(int id)
        {
            User? user = await FindByIdAsync(id.ToString());

            if (user == null)
            {
                return Result.Failure<bool>(Errors.Users.UserNotFound());
            }

            IdentityResult deleteResult = await DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                return Result.Failure<bool>(MapIdentityErrorToAppError(deleteResult));
            }

            return Result.Success(true);
        }

        public async Task<Result<UserJwtToken>> LoginUserAsync(string email, string password)
        {

            if (string.IsNullOrEmpty(email)) return Result.Failure<UserJwtToken>(Errors.Users.EmailError());
            if (string.IsNullOrEmpty(password)) return Result.Failure<UserJwtToken>(Errors.Users.InvalidPassword());

            User? user = await FindByEmailAsync(email);
            if (user == null) return Result.Failure<UserJwtToken>(Errors.Users.UserNotFound(email));

            bool canSignIn = await _signInManager.CanSignInAsync(user);
            if (!canSignIn) return Result.Failure<UserJwtToken>(Errors.Users.UserCanNotSignIn());

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);
            if (signInResult.IsNotAllowed) return Result.Failure<UserJwtToken>(Errors.Users.UserSignInNotAllowed());
            if (signInResult.IsLockedOut) return Result.Failure<UserJwtToken>(Errors.Users.UserLockout());
            if (!signInResult.Succeeded) return Result.Failure<UserJwtToken>(Errors.Users.InvalidPassword());

            UserJwtToken userToken = await GenerateUserTokens(user);

            return Result.Success(userToken);
        }

        public async Task<Result<User>> RegisterUserAsync(User user, string password, UserDiscriminator discriminator)
        {
            user.Discriminator = discriminator;

            string hashedPassword = PasswordHasher.HashPassword(user, password);

            Result setPasswordResult = User.SetUserPassword(user, hashedPassword);

            if (setPasswordResult.IsFailure) return Result.Failure<User>(setPasswordResult.Error);

            IdentityResult insertUserResult = await CreateAsync(user);

            if (!insertUserResult.Succeeded)
            {
                return Result.Failure<User>(MapIdentityErrorToAppError(insertUserResult));
            }

            return Result.Success(user);
        }

        public async Task<Result<bool>> AddUserToRoleAsync(User user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role)) return Result.Failure<bool>(Errors.Users.RoleNotFound(role));
            if (await IsInRoleAsync(user, role)) return Result.Failure<bool>(Errors.Users.AlreadyInRole(role));

            IdentityResult AddToRoleResult = await AddToRoleAsync(user, role);

            if (!AddToRoleResult.Succeeded)
            {
                return Result.Failure<bool>(MapIdentityErrorToAppError(AddToRoleResult));
            }

            return Result.Success(true);
        }

        private async Task<UserJwtToken> GenerateUserTokens(User user)
        {
            return new UserJwtToken
            {
                AccessToken = await GenerateAccessToken(user),
                RefreshToken = await GenerateRefreshToken(user),
            };
        }

        private async Task<JWTToken> GenerateAccessToken(User user)
        {
            var userRoles = await GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim( ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            string key = _configuration["Authentication:JwtBearer:SecurityKey"];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(AppConsts.User.TokenExpiryDays);
            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:JwtBearer:ValidIssuer"],
                audience: _configuration["Authentication:JwtBearer:ValidAudience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JWTToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        private async Task<JWTToken> GenerateRefreshToken(User user)
        {
            var claims = new List<Claim> {
                new Claim( ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtBearer:RefreshSecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);
            var expiration = DateTime.UtcNow.AddDays(AppConsts.User.RefreshTokenExpiryDays);

            var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _configuration["Authentication:JwtBearer:Issuer"],
                audience: _configuration["Authentication:JwtBearer:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            ));

            UserRefreshToken refreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                Token = token,
                ExpirationDate = expiration,
                IsUsed = false
            };

            await _refreshTokenRepo.AddAsync(refreshToken);
            await _refreshTokenRepo.SaveChangesAsync();

            return new JWTToken
            {
                Token = token,
                Expiration = expiration
            };
        }

        public static Error MapIdentityErrorToAppError(IdentityResult result)
        {
            string errorCode = result.Errors.FirstOrDefault() is null ? "NULL" : result.Errors.FirstOrDefault().Code;
            string errorMsg = result.Errors.FirstOrDefault() is null ? "NULL" : result.Errors.FirstOrDefault().Description;

            return Errors.Users.RegisterUserError(errorCode, errorMsg);
        }
    }
}
