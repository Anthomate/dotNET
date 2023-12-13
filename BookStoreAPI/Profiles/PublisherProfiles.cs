using AutoMapper;
using BookStoreAPI.Entities.BookEntities;
using BookStoreAPI.Models.Dto.PublisherDto;

namespace BookStoreAPI.Profiles
{
    public class PublisherProfiles : Profile
    {
        public PublisherProfiles()
        {
            CreateMap<Publisher, PublisherGetRequestDto>();
            CreateMap<PublisherCreateRequestDto, Publisher>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PublisherEditRequestDto, Publisher>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Publisher, PublisherCreateRequestDto>();
        }
    }
}
