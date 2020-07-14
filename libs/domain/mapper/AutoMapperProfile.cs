using AutoMapper;
using PagueVeloz.Domain.Models;
using PagueVeloz.Domain.ViewModels;

namespace PagueVeloz.Domain.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Empresa, EmpresaVM>().ReverseMap();
            CreateMap<Fornecedor, FornecedorVM>().ReverseMap();
            CreateMap<Fone, FoneVM>().ReverseMap();
            CreateMap<Pessoa, PessoaVM>().ReverseMap();
        }
    }
}