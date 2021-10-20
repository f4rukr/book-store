using AutoMapper;
using BookStore.Database.Entities;
using BookStore.Model.Books.Request;
using BookStore.Model.Books.Response;

namespace BookStore.Web.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookResponse>().ReverseMap();
            CreateMap<Book, CreateBookRequest>().ReverseMap();
            CreateMap<Book, UpdateBookRequest>().ReverseMap();
        }
    }
}
