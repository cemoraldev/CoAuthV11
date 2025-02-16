using AutoMapper;
using CoAuth.Core.DTOs;
using CoAuth.Core.Entities;

namespace CoAuth.Service;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<UserAppDto, UserApp>().ReverseMap();
    }
}