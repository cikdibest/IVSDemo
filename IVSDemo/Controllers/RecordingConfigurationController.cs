using IVSDemo.Service;
using Microsoft.AspNetCore.Mvc;

namespace IVSDemo.Controllers
{
    public class RecordingConfigurationController : Controller
    {
        private readonly IVSService _ivsService;

        public RecordingConfigurationController(IVSService ivsService)
        {
            _ivsService = ivsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecordingConfiguration(string s3BucketArn,int recordingReconnectWindowSeconds)
        {
            var response = await _ivsService.CreateRecordingConfiguration(s3BucketArn, recordingReconnectWindowSeconds);

            var recordingArn = response.RecordingConfiguration.Arn; // Oluşan kayıt yapılandırmasının ARN'si

            // Kayıt yapılandırmasının ARN'sini bir sonraki adıma ya da view'a döndür.
            return View("RecordingCreated", recordingArn);
        }
    }
}
