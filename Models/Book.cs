using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestLibrary.Models
{
    public class Book
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(40)]
        public string Title { get; set; }
        [Required]
        [StringLength(40)]
        public string Author { get; set; }
        [Required]
        [StringLength(40)]
        public string Language { get; set; }
        public Category Category { get; set; }
        [ForeignKey("Category")]
        public int categoryID { get; set; }
    }
}
