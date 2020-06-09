using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Models
{
    public class CertisReportModel
    {
        public CertisReportModel()
        {
            eventlocation = new double[2];
        }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string where { get; set; }
        public string when { get; set; }
        public string detail { get; set; }
        public double[] eventlocation { get; set; }
        public string timestamp { get; set; }
        public string image { get; set; }
        public string site { get; set; }
        public string reported_by { get; set; }
    }

}
