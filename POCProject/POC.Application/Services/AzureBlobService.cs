using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using POC.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Application.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(IConfiguration config)
        {
            _blobServiceClient = null;
            //_blobServiceClient = new BlobServiceClient(config["AzureBlob:ConnectionString"]);
        }

        public async Task UploadFileOnAzureAsync(string filePath, string containerName)
        {
            if (!File.Exists(filePath)) return;

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var fileName = Path.GetFileName(filePath);
            var blobClient = containerClient.GetBlobClient(fileName);

            using var stream = File.OpenRead(filePath);
            await blobClient.UploadAsync(stream, overwrite: true);
        }
    }
}
