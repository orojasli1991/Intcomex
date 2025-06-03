using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Application.Dto;

namespace Intcomex.ProductsApi.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, CategoryDto>().ReverseMap();
        }
    }
}
