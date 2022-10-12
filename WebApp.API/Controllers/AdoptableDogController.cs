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

        public AdoptableDogController(ILogger<AdoptableDogController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAdoptableDogs")]
        public IEnumerable<AdoptableDog> Get()
        {
            List<AdoptableDog> dogs = ExcelHelper.ReadAdoptionDogsSheetData("D:\\Projects\\BelWo\\IMS\\ims-take-home\\ims-take-home-code-challenge\\Dogs For Adoption Data.xlsx"); //TODO: RAVI

            foreach (var d in dogs)
            {
                var imgData = System.IO.File.ReadAllBytes(Environment.CurrentDirectory + d.Location.Replace("/", "//"));
                string _b64 = Convert.ToBase64String(imgData);
                d.ImageData = String.Format("data:image/jpeg;base64,{0}", _b64);
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