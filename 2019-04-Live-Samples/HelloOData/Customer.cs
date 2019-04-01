using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloOData
{
    public class Customer
    {
        public int ID { get; set; }

        [Required]
        public string CustomerName { get; set; }
        public string Country { get; set; }
    }
}
