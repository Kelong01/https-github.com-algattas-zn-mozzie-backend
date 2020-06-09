using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class NeaDengueResponse
    {
        public Srchresult[] SrchResults { get; set; }
    }

    public class Srchresult
    {
        public Srchresult()
        {
            Points = new List<LatLngPoint>();
        }
        public int FeatCount { get; set; }
        //public string Theme_Name { get; set; }
        //public string Category { get; set; }
        //public string Owner { get; set; }
        public Datetime DateTime { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string HYPERLINK { get; set; }
        public string CASE_SIZE { get; set; }
        public string HOMES { get; set; }
        public string PUBLIC_PLACES { get; set; }
        public string CONSTRUCTION_SITES { get; set; }
        public string MAPTIP { get; set; }
        public string SYMBOLCOLOR { get; set; }
        public string LatLng { get; set; }
        public string ICON_NAME { get; set; }
        [NotMapped]
        public List<LatLngPoint> Points { get; set; }
    }

    public class Datetime
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class LatLngPoint
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }

}
