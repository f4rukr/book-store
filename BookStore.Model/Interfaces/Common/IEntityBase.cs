using System;
namespace BookStore.Model.Interfaces.Common
{
    public interface IEntityBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
