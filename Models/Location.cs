using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Models
{
    [Table("Location")]
    public class Location
    {
        public Location()
        {
            Files = new HashSet<LocationFile>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// The Mozzie Name
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Mobile Unique Id
        /// </summary>
        [MaxLength(200)]
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
        /// Report Datetime
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime ReportDateTime { get; set; }

        /// <summary>
        /// The Location address, which is translated by Map provider
        /// </summary>
        [MaxLength(1000)]
        public string Address { get; set; }

        /// <summary>
        /// Additional info
        /// </summary>
        [MaxLength(1000)]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// creation datetime
        /// </summary>
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 上传的图片
        /// </summary>
        public ICollection<LocationFile> Files { get; set; }
        /// <summary>
        /// 上报者Id
        /// </summary>
        public int? ReportUserId { get; set; }
        /// <summary>
        /// Type:Adult,Larvae,Egg
        /// </summary>
        [MaxLength(50)]
        public string Type { get; set; }
    }
}
