using BookStore.Database.Interfaces.Common;
using System;
namespace BookStore.Database.Entities
{
    public class BookAuthor : IEntityBase
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
