using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace WebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdoptableDogController : ControllerBase
    {
        private readonly ILogger<AdoptableDogController> _logger;
        private readonly IConfiguration _config;

        public AdoptableDogController(ILogger<AdoptableDogController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            string _DbFileName = _config.GetValue<string>("DbFileName");
            string _SheetName = _config.GetValue<string>("SheetName");
            ExcelHelper.Instance.Init(_DbFileName, _SheetName);
        }

        [HttpGet(Name = "GetAdoptableDogs")]
        public IEnumerable<AdoptableDog> Get()
        {
            _logger.LogInformation("GetAdoptableDogs called");

            DirectoryInfo dogsXlsDirInfo = VisualStudioProvider.TryGetSolutionDirectoryInfo();
            if (dogsXlsDirInfo == null)
                return Enumerable.Empty<AdoptableDog>();

            List<AdoptableDog> dogs = ExcelHelper.Instance.ReadAdoptionDogsSheetData();
            if (dogs == null)
                return Enumerable.Empty<AdoptableDog>();

            foreach (var d in dogs)
            {
                if (!string.IsNullOrEmpty(d.Location))
                {
                    var dogFilePath = Directory.GetCurrentDirectory() + d.Location;
                    string dogPicFile = dogFilePath.Replace("/", "//");
                    var imgData = System.IO.File.ReadAllBytes(dogPicFile);
                    string _b64 = Convert.ToBase64String(imgData);
                    string dogFileExt = Path.GetExtension(dogPicFile).ToLower();
                    string base64ImagePrefix = "jpeg"; //Default prefix
                    switch (dogFileExt)
                    {
                        case "jpg":
                            base64ImagePrefix = "jpeg";
                            break;
                        case "png":
                            base64ImagePrefix = "png";
                            break;
                        default:

                            break;
                    }
                    d.ImageData = String.Format("data:image/{1};base64,{0}", _b64, base64ImagePrefix);
                }
            }
            return dogs.ToArray();
        }
        [HttpPost(Name = "AddAdoptableDogs")]
        public bool Add(AdoptableDog dog)
        {
            List<AdoptableDog> dogs = new List<AdoptableDog>();
            return true;
        }
    }
}