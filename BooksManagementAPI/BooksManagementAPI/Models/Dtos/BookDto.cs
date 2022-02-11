using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagementAPI.Models.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public enum Category { Novel, Fiction, Adventure, Drama, Horror }
    }
}
