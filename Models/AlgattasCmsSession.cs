using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Models
{
    [Table("AlgattasCmsSession")]
    public class AlgattasCmsSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Session Id
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// UDID
        /// </summary>
        public string Udid { get; set; }
        /// <summary>
        /// Expires date
        /// </summary>
        public DateTime Expires { get; set; }
        /// <summary>
        /// Last update datetime
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
    }
}
