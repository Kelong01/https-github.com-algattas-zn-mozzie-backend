using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class AttachmentModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtenstion { get; set; }
        /// <summary>
        /// 单位KB
        /// </summary>
        public float FileSize { get; set; }
        /// <summary>
        /// Azure blob address
        /// </summary>
        public string AzureBlobAddress { get; set; }
    }
}
