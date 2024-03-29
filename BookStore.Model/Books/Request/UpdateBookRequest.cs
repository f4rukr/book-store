﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BookStore.Model.Books.Request
{
    public class UpdateBookRequest : CreateBookRequest
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Reason")]
        public string Reason { get; set; }
        public List<SelectListItem> SelectedAuthorIds { get; set; }
    }
}
