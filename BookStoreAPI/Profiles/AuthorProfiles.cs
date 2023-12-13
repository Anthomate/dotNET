using AutoMapper;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.AuthorDto;

namespace BookStoreAPI.Profiles
{
    public class AuthorProfiles : Profile
    {
        public AuthorProfiles()
        {
            CreateMap<Author, AuthorGetRequestDto>();
            CreateMap<AuthorCreateRequestDto, Author>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<AuthorEditRequestDto, Author>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Author, AuthorCreateRequestDto>();
        }
    }
}
