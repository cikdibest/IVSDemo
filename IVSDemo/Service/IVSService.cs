using Amazon.IVS;
using Amazon.IVS.Model;
using Amazon.S3;
using Amazon.S3.Model;

namespace IVSDemo.Service
{
    public class IVSService
    {
        private readonly IAmazonIVS _ivsClient;
        private readonly IAmazonS3 _s3Client;

        public IVSService(IConfiguration configuration)
        {
            var accessKey = configuration["AWS:AccessKey"];
            var secretKey = configuration["AWS:SecretKey"];
            var region = configuration["AWS:Region"];

            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            _ivsClient = new AmazonIVSClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
            _s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }

        public async Task<CreateChannelResponse> CreateChannel(string channelName,string recordingConfigurationArn)
        {
            var request = new CreateChannelRequest
            {
                Name = channelName,
                LatencyMode = ChannelLatencyMode.NORMAL,
                Type = ChannelType.BASIC,
                RecordingConfigurationArn = recordingConfigurationArn
            };

            return await _ivsClient.CreateChannelAsync(request);
        }

        public async Task<CreateRecordingConfigurationResponse> CreateRecordingConfiguration(string s3BucketArn,int recordingReconnectWindowSeconds)
        {
            var request = new CreateRecordingConfigurationRequest
            {
                Name = "MyRecordingConfiguration",
                DestinationConfiguration = new DestinationConfiguration
                {
                    S3 = new S3DestinationConfiguration
                    {
                        BucketName = s3BucketArn // Kayıtların saklanacağı S3 bucket ARN'si
                    }
                },
                RecordingReconnectWindowSeconds = recordingReconnectWindowSeconds,
                ThumbnailConfiguration = new ThumbnailConfiguration
                {
                    RecordingMode = RecordingMode.DISABLED // İsterseniz küçük resim yakalama modunu da ekleyebilirsiniz.
                }
            };

            return await _ivsClient.CreateRecordingConfigurationAsync(request);
        }

        // Tüm S3 bucket'larını listeleme
        public async Task<List<S3Bucket>> ListBuckets()
        {
            var response = await _s3Client.ListBucketsAsync();
            return response.Buckets;
        }

        // Belirli bir bucket'taki içerikleri listeleme
        public async Task<List<S3Object>> ListObjectsInBucket(string bucketName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects;
        }
    }
}
