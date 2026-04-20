using AutoMapper;
using POC.Application.DTO;
using POC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Products, ProductDto>().ReverseMap();
            CreateMap<Products, UpdateProductDto>().ReverseMap();
            CreateMap<Products, CreateProductDto>().ReverseMap();

        }
    }
}
