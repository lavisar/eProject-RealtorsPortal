using System.IO;

namespace Eproject_RealtorsPortal.Models
{
    public class UploadFileInfo
    {
        public string BaseImageUrl { get; set; }
        public string UploadFunction { get; set; }
        public string DirectoryPath { get; set; }
        public string File { get; set; }
        public string SaveAsName { get; set; }
        public string ImageFrame { get; set; }
        public double MBytes { get; set; }
    }
}
