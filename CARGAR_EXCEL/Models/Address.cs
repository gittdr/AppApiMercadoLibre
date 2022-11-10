using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARGAR_EXCEL.Models
{
    public class Address
    {
        public string address_line { get; set; }
        public string street_name { get; set; }
        public string street_number { get; set; }
        public string intersection { get; set; }
        public string zip_code { get; set; }
        public City city { get; set; }
        public Country country { get; set; }
        public State state { get; set; }
        public Neighborhood neighborhood { get; set; }
        public Municipality municipality { get; set; }

    }
}
