using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{

    public class AlagattasContentEntryResponse
    {
        public string message { get; set; }
        public string responseStatus { get; set; }
        public System system { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }
        public Item[] items { get; set; }
        public int totalCount { get; set; }
        public bool isNextPage { get; set; }
    }

    public class System
    {
        public string type { get; set; }
        public string apiVersion { get; set; }
        public string[] orgGroup { get; set; }
        public string channelType { get; set; }
        public int contentTypeId { get; set; }
    }

    public class Item
    {
        public object itemType { get; set; }
        public string contentTypeId { get; set; }
        public string contentTypeName { get; set; }
        public string content { get; set; }
        public string entryId { get; set; }
        public string image { get; set; }
        public string transmit { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string mozzietype { get; set; }
    }

}
