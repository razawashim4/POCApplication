using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Application.IServices
{
    public interface IAzureBlobService
    {
        Task UploadFileOnAzureAsync(string filePath, string containerName);
    }
}
