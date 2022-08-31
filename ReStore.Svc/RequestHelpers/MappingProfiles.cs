using AutoMapper;
using ReStore.Svc.DTOs;
using ReStore.Svc.Entities;

namespace ReStore.Svc.RequestHelpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<CreateProductDto, Product>();
      CreateMap<UpdateProductDto, Product>();
    }
  }
}