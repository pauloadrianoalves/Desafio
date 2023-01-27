using AutoMapper;

namespace Desafio.Application.Mapping
{
    public class DesafioProfile : Profile
    {
        public DesafioProfile()
        {
            CreateMap<Domain.Cliente, Dtos.Cliente>();

            CreateMap<Dtos.Cliente, Domain.Cliente>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome.ToUpper()))
                .ForMember(dest => dest.Rua, opt => opt.MapFrom(src => src.Rua.ToUpper()))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero.ToUpper()))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.Bairro.ToUpper()))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Cidade.ToUpper()))
                .ForMember(dest => dest.Uf, opt => opt.MapFrom(src => src.Uf.ToUpper()));
        }
    }
}
