using Dapper;
using Desafio.Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Desafio.Persistence.Contract
{
    public interface IClientePersist
    {
        /// <summary>
        /// Retorna ficha de cadastro do cliente
        /// </summary>
        /// <param name="codigo">Código do cliente</param>
        /// <returns>Domain.Cliente</returns>
        Task<Domain.Cliente?> Registro(int codigo);

        /// <summary>
        /// Retorna lista paginada
        /// </summary>
        /// <param name="nPaginas">Página atual</param>
        /// <param name="tPagina">Quantidade de registros por página</param>
        /// <param name="oColuna">Coluna de ordenação</param>
        /// <param name="ordem">Tipo de ordenação</param>
        /// <param name="filtro">Filtrar por nome</param>
        /// <returns>List<Cliente></returns>
        Task<List<Cliente>?> Listar(int nPaginas, int tPagina, string oColuna, string ordem, string? filtro);

        /// <summary>
        /// Retorna quantidade de registros encontrados
        /// </summary>
        /// <param name="filtro">Filtrar por nome</param>
        /// <returns>int</returns>
        Task<int> ListarQuantidade(string? filtro);

        /// <summary>
        /// Verifica se um cliente já existe
        /// </summary>
        /// <param name="codigo">Código do cliente</param>
        /// <returns>bool</returns>
        Task<bool> Existe(int codigo);
    }

    public class ClientePersist : IClientePersist
    {
        private readonly Contexts.DesafioContext _context;
        private readonly NpgsqlConnection _conn;

        public ClientePersist(Contexts.DesafioContext context)
        {
            _context = context;
            _conn = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        }

        public async Task<Domain.Cliente?> Registro(int codigo)
        {
            try
            {
                var sql = $"select * from tb_cliente where codigo = '{codigo}'";
                var rsp = await _conn.QueryAsync<Domain.Cliente>(sql);
                return rsp != null ? rsp.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Cliente>?> Listar(int nPaginas, int tPagina, string oColuna, string ordem, string? filtro)
        {
            try
            {
                filtro = !string.IsNullOrEmpty(filtro) ? $"where upper(nome) like '%{filtro.ToUpper()}%'" : "";
                var sql = $"select * from tb_cliente {filtro} order by {oColuna} {ordem} offset {nPaginas} limit {tPagina}";
                var rsp = await _conn.QueryAsync<Domain.Cliente>(sql);
                return rsp != null ? rsp.ToList() : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ListarQuantidade(string? filtro)
        {
            try
            {
                filtro = !string.IsNullOrEmpty(filtro) ? $"where upper(nome) like '%{filtro?.ToUpper()}%'" : "";
                var sql = $"select count(*) from tb_cliente {filtro}";
                var rsp = await _conn.QueryAsync<int>(sql);
                return rsp != null ? rsp.First() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Existe(int codigo)
        {
            try
            {
                return await _context.Cliente.AnyAsync(e => e.Codigo == codigo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
