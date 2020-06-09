using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Azure
{
    public interface IAzureBlobStorage
    {
        Task UploadAsync(string blobName, string filePath);
        Task<string> UploadSasAsync(string inferenceType, string blobName, string filePath);
        Task UploadAsync(string blobName, Stream stream);
        Task<MemoryStream> DownloadAsync(string blobName);
        Task DownloadAsync(string blobName, string path);
        Task<List<AzureBlobItem>> ListAsync();
        Task<List<string>> ListFoldersAsync();
    }
}
