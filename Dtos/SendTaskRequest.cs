using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class SendTaskRequest
    {
        public SendTaskRequest()
        {
            ImageURLs = new List<string>();
        }

        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string DeviceType { get; set; }
        [Required]
        public string InferenceObjectType { get; set; }
        public List<string> ImageURLs { get; set; }
        /// <summary>
        /// longitude
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double? Lat { get; set; }
        public string LocationName { get; set; }
    }
}
