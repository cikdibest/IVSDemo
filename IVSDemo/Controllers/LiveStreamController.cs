using IVSDemo.Service;
using Microsoft.AspNetCore.Mvc;

namespace IVSDemo.Controllers
{
    public class LiveStreamController : Controller
    {
        private readonly IVSService _ivsService;

        public LiveStreamController(IVSService ivsService)
        {
            _ivsService = ivsService;
        }

        // Canlı yayın başlatma formunu gösterecek
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Canlı yayın başlatma isteğini işleyecek
        [HttpPost]
        public async Task<IActionResult> StartLiveStream(string channelName,string recordingConfigurationArn)
        {
            var response = await _ivsService.CreateChannel(channelName, recordingConfigurationArn);

            var streamUrl = response.Channel.PlaybackUrl;  // İzleyiciler için
            var streamKey = response.StreamKey.Value;      // Yayıncı için

            return View("LiveStream", new { StreamUrl = streamUrl, StreamKey = streamKey });
        }
    }
}
