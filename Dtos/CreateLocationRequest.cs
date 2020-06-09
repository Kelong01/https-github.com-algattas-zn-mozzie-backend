using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class CreateLocationRequest
    {
        /// <summary>
        /// The mozzie name
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// Mobile Unique Id
        /// </summary>
        [MaxLength(200)]
        [Required]
        public string Uuid { get; set; }
        /// <summary>
        /// longitude
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// 蚊子类型
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 蚊子分类
        /// </summary>
        public string SubCategory { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Report Datetime
        /// </summary>
        public DateTime ReportDateTime { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Additional info
        /// </summary>
        [MaxLength(1000)]
        public string AdditionalInfo { get; set; }

        public List<LocationFileInput> Files { get; set; }

        /// <summary>
        /// Type:Adult,Larvae,Egg
        /// </summary>
        [MaxLength(50)]
        public string Type { get; set; }
    }

    public class LocationFileInput
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [StringLength(100)]
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public float FileSize { get; set; }
    }
}
