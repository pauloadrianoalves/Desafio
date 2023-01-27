using Desafio.App.Models;
using System.Net;
using System.Text.Json;

namespace Desafio.App.Api
{
    public interface IClienteApi
    {
        /// <summary>
        /// Cadastra/Altera cliente
        /// </summary>
        /// <param name="cliente">Models.Cliente</param>
        /// <returns>bool</returns>
        Task<bool> Gravar(Models.Cliente cliente);

        /// <summary>
        /// Excluir cliente
        /// </summary>
        /// <param name="codigo">Código cliente</param>
        /// <returns>bool</returns>
        Task<bool> Excluir(int codigo);

        /// <summary>
        /// Carrega registro cliente
        /// </summary>
        /// <param name="codigo">Código cliente</param>
        /// <returns>Models.Cliente</returns>
        Task<Models.Cliente> Ficha(int codigo);

        /// <summary>
        /// Retorna lista de clientes paginada
        /// </summary>
        /// <param name="nPaginas">Página atual</param>
        /// <param name="tPagina">Quantidade de registros por página</param>
        /// <param name="oColuna">Coluna de ordenação</param>
        /// <param name="ordem">Tipo de ordenação</param>
        /// <param name="filtro">Filtragem por nome</param>
        /// <returns></returns>
        Task<ClienteListar> CarregarLista(int nPaginas, int tPagina, string oColuna, string ordem, string filtro = "");

        /// <summary>
        /// Valida se os campos foram preenchidos
        /// </summary>
        /// <param name="cliente">Models.Cliente</param>
        /// <returns>string</returns>
        string Validar(Models.Cliente cliente);
    }

    public class ClienteApi : IClienteApi
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Url de acesso a controller cliente
        /// </summary>
        private readonly string UrlApiCliente = $"{Config.UrlApi}/cliente";

        public ClienteApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Gravar(Models.Cliente cliente)
        {
            try
            {
                var content = JsonContent.Create(cliente);
                var httpResponse = cliente.Codigo == 0
                    ? await _httpClient.PostAsync(UrlApiCliente, content)
                    : await _httpClient.PutAsync(UrlApiCliente, content);
                var status = httpResponse.EnsureSuccessStatusCode();
                return status.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Excluir(int codigo)
        {
            try
            {
                var url = $"{UrlApiCliente}/{codigo}";
                var httpResponse = await _httpClient.DeleteAsync(url);
                var status = httpResponse.EnsureSuccessStatusCode();
                return status.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Cliente?> Ficha(int codigo)
        {
            try
            {
                var url = $"{UrlApiCliente}/{codigo}";
                var httpResponse = await _httpClient.GetAsync(url);
                httpResponse.EnsureSuccessStatusCode();
                var conteudo = await httpResponse.Content.ReadAsStringAsync();
                return !string.IsNullOrEmpty(conteudo)
                        ? JsonSerializer.Deserialize<Cliente>(conteudo)
                        : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClienteListar?> CarregarLista(int nPaginas, int tPagina, string oColuna, string ordem, string filtro = "")
        {
            try
            {
                var url = $"{UrlApiCliente}/{nPaginas}/{tPagina}/{oColuna}/{ordem}";
                url = string.IsNullOrEmpty(filtro) ? url : $"{url}/{filtro}";
                var httpResponse = await _httpClient.GetAsync(url);
                httpResponse.EnsureSuccessStatusCode();
                var conteudo = await httpResponse.Content.ReadAsStringAsync();
                return !string.IsNullOrEmpty(conteudo)
                    ? JsonSerializer.Deserialize<ClienteListar>(conteudo)
                    : null;
            }
            catch (Exception)
            {
                return new ClienteListar();
            }
        }

        public string Validar(Models.Cliente cliente)
        {
            var msg = "";
            if (string.IsNullOrEmpty(cliente.Nome))
            {
                msg = "- Nome é obrigatório.<br>";
            }
            if (string.IsNullOrEmpty(cliente.Rua))
            {
                msg += "- Rua é obrigatório.<br>";
            }
            if (string.IsNullOrEmpty(cliente.Numero))
            {
                msg += "- Número é obrigatório.<br>";
            }
            if (string.IsNullOrEmpty(cliente.Bairro))
            {
                msg += "- Bairro é obrigatório.<br>";
            }
            if (string.IsNullOrEmpty(cliente.Cidade))
            {
                msg += "- Cidade é obrigatório.<br>";
            }
            if (string.IsNullOrEmpty(cliente.Uf))
            {
                msg += "- Uf é obrigatório.";
            }
            return msg;
        }
    }
}
