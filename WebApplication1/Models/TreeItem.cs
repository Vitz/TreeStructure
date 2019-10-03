using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class TreeItem 
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Value { get; set; }
       
        public int? Parent { get; set; }
    }
}
