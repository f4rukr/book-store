using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;
namespace BookStore.Model.Books.Request
{
    public class CreateBookRequest
    {

        [Required]
        [StringLength(50)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Publish Date")]
        [Range(typeof(DateTime), "01/01/2000", "01/01/2022")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Published { get; set; }

        [Required(ErrorMessage = "Please select an author")]
        public List<int> AuthorIds { get; set; }
        
        public List<SelectListItem> drpAuthors { get; set; }
    }
}
