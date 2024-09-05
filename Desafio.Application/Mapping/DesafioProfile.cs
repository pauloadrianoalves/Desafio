using AutoMapper;

namespace Desafio.Application.Mapping
{
    public class DesafioProfile : Profile
    {
        public DesafioProfile()
        {
            CreateMap<Domain.Cliente, Dtos.Cliente>().ReverseMap();
        }
    }
}
