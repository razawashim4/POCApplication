using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using POC.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Infrastructure.BackgroungTask
{
    public class FileUploadBackgroundService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IAzureBlobService _blobService;
        public FileUploadBackgroundService(IConfiguration config, IAzureBlobService blobService)
        {
            _config = config;
            _blobService = blobService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UploadLogs("Logs//Requests", "request-.log");
                await UploadLogs("Logs//Exceptions", "exception-.log");

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

        }

        private async Task UploadLogs(string folderPath, string containerName)
        {
            //if (!Directory.Exists(folderPath)) return;

            var files = Directory.GetFiles(folderPath)
            .OrderByDescending(f => File.GetLastWriteTimeUtc(f))
            .Skip(1); // skip active file

            foreach (var file in files)
            {
                //await _blobService.UploadFileOnAzureAsync(file, containerName);

                //string dd = Path.Combine(folderPath, "uploaded", Path.GetFileName(file));
                var uploadedFolder = Path.Combine(folderPath, "uploaded");
                if (!Directory.Exists(uploadedFolder))
                {
                    Directory.CreateDirectory(uploadedFolder);
                }
                File.Move(file, Path.Combine(uploadedFolder, Path.GetFileName(file)), true);
            }
        }
    }
}
