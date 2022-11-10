using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARGAR_EXCEL.Models
{
    public class MLMCartaPorte
    {
        public string id { get; set; }
        public string cost { get; set; }
        public int status { get; set; }
        public Recipient recipient { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public List<Shipments> shipments { get; set; }
        public Package package { get; set; }
        
    }
}
