using Core.Controller;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vezeeta.Web.Helpers
{
    public interface IImageHelper
    {
        Result<string> UploadImage(IFormFile? image);
    }

    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageHelper(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public Result<string> UploadImage(IFormFile? image)
        {
            if (image != null)
            {
                if (!HasImageExtension(image)) return Result.Failure<string>(Error.Errors.General.InvalidImage());

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                string imageName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string imagePath = Path.Combine(uploadsFolder, imageName);
                image.CopyTo(new FileStream(imagePath, FileMode.Create));

                return Result.Success(imageName);
            }

            return Result.Success(string.Empty);
        }

        public static bool HasImageExtension(IFormFile image)
        {
            return (image.FileName.EndsWith(".png") || image.FileName.EndsWith(".jpg"));
        }
    }
}