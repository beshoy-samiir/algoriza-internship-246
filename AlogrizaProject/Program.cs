using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Data;
using System.Text;
using Vezeeta.Core.Domain.Users;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Service.Bookings;
using Vezeeta.Core.Service.Settings;
using Vezeeta.Core.Service.Users;
using Vezeeta.Repository;
using Vezeeta.Repository.Repositories;
using Vezeeta.Service.Bookings;
using Vezeeta.Service.Settings;
using Vezeeta.Service.Users;
using Vezeeta.Web.Helpers;

namespace Vezeeta.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Host.UseSerilog
                ((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "Vezeeta Public API v1.0", Version = "v1" });
                opt.AddSecurityDefinition("VezeetaApiAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Input Valid Access token to access this API"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "VezeetaApiAuth"
                            }
                        }, new List<string>()
                    }
                });
            });

            builder.Services.AddDbContextPool<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString: configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            }, 2048 /* pool size */ );


            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
                .AddUserStore<UserStore<User, Role, ApplicationDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>>()
                .AddRoleStore<RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>>()
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddPasswordValidator<PasswordValidator<User>>()
                .AddSignInManager<SignInManager<User>>();

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["Authentication:JwtBearer:ValidAudience"],
                    ValidIssuer = configuration["Authentication:JwtBearer:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]))
                };
            });

            builder.Services.AddAutoMapper(typeof(MapperConfig));

            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient(typeof(IDoctorRepository), typeof(DoctorRepository));
            builder.Services.AddTransient(typeof(IPatientRepository), typeof(PatientRepository));
            builder.Services.AddTransient(typeof(IBookingRepository), typeof(BookingRepository));

            builder.Services.AddTransient(typeof(IUserService), typeof(UserService));
            builder.Services.AddTransient(typeof(IDoctorService), typeof(DoctorService));
            builder.Services.AddTransient(typeof(IPatientService), typeof(PatientService));
            builder.Services.AddTransient(typeof(IBookingService), typeof(BookingService));
            builder.Services.AddTransient(typeof(ICouponService), typeof(CouponService));

            builder.Services.AddTransient(typeof(IImageHelper), typeof(ImageHelper));


            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vezeeta Public API V1");
                });
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

    }
}