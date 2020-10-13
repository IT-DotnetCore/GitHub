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
        [Required(ErrorMessage ="Please enter the title of your book")]
        [StringLength(40,MinimumLength =5)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter Author name")]
        [StringLength(40, MinimumLength=5)]
        public string Author { get; set; }
        [Required(ErrorMessage = "Please enter the Language of your book")]
        [StringLength(40,MinimumLength =2)]
        public string Language { get; set; }

        public Category Category { get; set; }
        [ForeignKey("Category")]
        public int categoryID { get; set; }
    }
}
