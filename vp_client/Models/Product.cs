using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vp_client.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string NameProduct { get; set; } = "";
        public byte[]? Photo { get; set; }
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string? Nicotine { get; set; }
        public string? Strength { get; set; }
        public double Cost { get; set; }

    }
}
