using IVSDemo.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class S3Controller : Controller
{
    private readonly IVSService _s3Service;

    public S3Controller(IVSService s3Service)
    {
        _s3Service = s3Service;
    }

    // Tüm bucket'ları listeleyen action
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var buckets = await _s3Service.ListBuckets();
        return View(buckets);
    }

    // Belirli bir bucket'ın içindeki dosyaları listeleyen action
    [HttpGet]
    public async Task<IActionResult> ListObjects(string bucketName)
    {
        var objects = await _s3Service.ListObjectsInBucket(bucketName);
        return View(objects);
    }
}
