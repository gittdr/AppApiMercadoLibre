using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARGAR_EXCEL.Models
{
    public class Items
    {
        public string category { get; set; }
        public string description { get; set; }
        public string unit_code { get; set; }
        public string quantity { get; set; }
        public Dimensions dimensions { get; set; }

    }
}
