using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vp_client.Models
{
    public class ViewDTO
    {
        public int ProductId { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly Time { get; set; }
    }
}
