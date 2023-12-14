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
                CreateMap<BookCreateRequestDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new Author { Id = src.AuthorId }))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => new Publisher { Id = src.PublisherId }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublicationDate))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.BookGenre, opt => opt.MapFrom(src => new Genre { Id = src.GenreId }));


            CreateMap<Book, BookCreateRequestDto>();
            CreateMap<BookEditRequestDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new Author { Id = src.AuthorId }))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => new Publisher { Id = src.PublisherId }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublicationDate))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.BookGenre, opt => opt.MapFrom(src => new Genre { Id = src.GenreId }));
        }
    }
}