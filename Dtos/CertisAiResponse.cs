using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class CertisAiResponse
    {
        public Detectionresult[] detectionResults { get; set; }
    }

    public class Detectionresult
    {
        public Label[] labels { get; set; }
        public FrameLabel[] frameLabels { get; set; }
        public string[] imageURLs { get; set; }
    }

    public class Label
    {
        public string category { get; set; }
        public Attributes attributes { get; set; }
    }

    public class FrameLabel
    {
        public string category { get; set; }
        public FrameAttributes attributes { get; set; }
    }

    public class Attributes
    {
        public string gender { get; set; }
        public string genderScore { get; set; }
        public string species { get; set; }
        public string speciesScore { get; set; }
        public string cameraIndex { get; set; }
    }

    public class FrameAttributes
    {
        public bool imageQuality { get; set; }
        public int quantity { get; set; }
        public bool hasAedes { get; set; }
    }

}
