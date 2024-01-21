using AutoMapper;
using EmanuelCegidTest.Models;

namespace EmanuelCegidTest.DTOs.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Customers, CustomersDTO>().ReverseMap();
            CreateMap<SalesItems, SalesItemsDTO>().ReverseMap();
        }
    }
}
