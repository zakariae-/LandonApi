using System;
using AutoMapper;
using LandonApi.Models;

namespace LandonApi.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomEntity, Room>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100.0m))
                .ForMember(
                    dest => dest.Self, 
                    opt => opt.MapFrom(src =>
                                       Link.To(nameof(Controllers.RoomsController.GetRoomByIdAsync), new { roomId = src.Id })));
        }
    }
}
