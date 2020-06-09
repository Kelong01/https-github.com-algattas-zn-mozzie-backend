using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class LocalActivityListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The mozzie name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mobile Unique Id
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// longitude
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// The Location address, which is translated by Map provider
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ReportDateTime
        /// </summary>
        public DateTime ReportDateTime { get; set; }

        /// <summary>
        /// 上报者Id
        /// </summary>
        public int? ReportUserId { get; set; }
    }
}
