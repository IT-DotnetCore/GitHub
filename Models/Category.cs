using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestLibrary.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
