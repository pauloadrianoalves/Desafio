using AutoMapper;

namespace Desafio.Application.Service
{
    public interface IClienteService
    {
        /// <summary>
        /// Cadastra/Atualizar cliente
        /// </summary>
        /// <param name="cliente">Dtos.Cliente</param>
        /// <returns>Dtos.Cliente</returns>
        Task<bool> Gravar(Dtos.Cliente cliente);

        /// <summary>
        /// Excluir cliente
        /// </summary>
        /// <param name="codigo">Código de identificação</param>
        /// <returns>string</returns>
        Task<string> Excluir(int codigo);

        /// <summary>
        /// Carrega registro cliente
        /// </summary>
        /// <param name="codigo">Código de identificação</param>
        /// <returns>(Dtos.Cliente?, string)</returns>
        Task<(Dtos.Cliente?, string)> Registro(int codigo);

        /// <summary>
        /// Retorna lista de clientes paginada
        /// </summary>
        /// <param name="nPaginas">Número da página</param>
        /// <param name="tPagina">Total de registros por página</param>
        /// <param name="oColuna">Coluna de ordenação</param>
        /// <param name="ordem">Tipo de ordenação: asc/desc</param>
        /// <param name="filtro">Filtrar cliente</param>
        /// <returns>List<Dtos.Cliente>, int)</returns>
        Task<(List<Dtos.Cliente>, int)> Listar(int nPaginas, int tPagina, string oColuna, string ordem, string? filtro);
    }

    public class ClienteService : IClienteService
    {
        private readonly Persistence.IBaseRepository _repository;
        private readonly Persistence.Contract.IClientePersist _cliente;
        private readonly IMapper _mapper;

        public ClienteService(
                        Persistence.IBaseRepository repository,
                        Persistence.Contract.IClientePersist cliente,
                        IMapper mapper)
        {
            _repository = repository;
            _cliente = cliente;
            _mapper = mapper;
        }

        public async Task<bool> Gravar(Dtos.Cliente cliente)
        {
            try
            {
                var map = _mapper.Map<Domain.Cliente>(cliente);                           
                if (await _cliente.Existe(cliente.Codigo))
                {
                    var ficha = await _cliente.Registro(cliente.Codigo);
                    map.DataCadastro = ficha?.DataCadastro;
                    _repository.Update(map);
                }
                else
                {
                    _repository.Add(map);
                }
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Excluir(int codigo)
        {
            try
            {
                var cliente = await _cliente.Registro(codigo);
                if (cliente == null) return "Cliente não encontrado para exclusão.";
                _repository.Delete(cliente);
                return await _repository.SaveChangesAsync() ? "" : "Não foi possível excluir o cliente.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<(Dtos.Cliente?, string)> Registro(int codigo)
        {
            try
            {
                var cliente = await _cliente.Registro(codigo);
                return cliente != null
                    ? (_mapper.Map<Dtos.Cliente>(cliente), "")
                    : (null, "Cliente não encontrado.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<Dtos.Cliente>, int)> Listar(int nPaginas, int tPagina, string oColuna, string ordem, string? filtro)
        {
            try
            {
                var cliente = await _cliente.Listar(nPaginas, tPagina, oColuna, ordem, filtro);
                var quant = await _cliente.ListarQuantidade(filtro);
                return (_mapper.Map<List<Dtos.Cliente>>(cliente), quant);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}