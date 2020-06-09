using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class AbpResponse
    {
        public object Result { get; set; }
        public bool Success { get; set; }
        public object Error { get; set; }
    }
}
