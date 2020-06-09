using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Models
{
    public class DengueZikaModel
    {
        public string DengueCaseCount { get; set; }
        public string DengueCaseUpdateDate { get; set; }
        public string ZikaCaseCount { get; set; }
        public string ZikaCaseUpdateDate { get; set; }
        /// <summary>
        /// Current DateTime
        /// </summary>
        public DateTime CurrentDate { get; set; }
    }
}
