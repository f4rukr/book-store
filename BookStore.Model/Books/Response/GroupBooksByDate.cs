using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.Books.Response
{
    public class GroupBooksByDate
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime PublishedDate { get; set; }
        public int BookCount { get; set; }
    }
}
