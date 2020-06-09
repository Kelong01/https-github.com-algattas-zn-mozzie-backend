using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Models
{
    [Table("LocationFile")]
    public class LocationFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
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
        /// <summary>
        /// 位置Id
        /// </summary>
        public long LocationId { get; set; } 
        public Location Location { get; set; }
    }
}
