using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class GetNearbyReportsRequest
    {
        /// <summary>
        /// longitude
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Last days
        /// </summary>
        public int Days { get; set; } = 7;
    }
}
