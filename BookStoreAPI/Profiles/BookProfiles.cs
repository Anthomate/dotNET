using AutoMapper;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.BookDto;

namespace BookStoreAPI.Profiles
{
    public class BookProfiles : Profile
    {
        public BookProfiles() 
        { 
            CreateMap<Book, BookGetRequestDto>();
            CreateMap<BookCreateRequestDto, Book>();
        }
    }
}
