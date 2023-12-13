using AutoMapper;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.GenreDto;

namespace BookStoreAPI.Profiles
{
    public class GenreProfiles : Profile
    {
        public GenreProfiles()
        {
            CreateMap<Genre, GenreGetRequestDto>();
            CreateMap<GenreCreateRequestDto, Genre>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<GenreEditRequestDto, Genre>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Genre, GenreCreateRequestDto>();
        }
    }
}
