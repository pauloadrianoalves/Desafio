using System.Text.Json.Serialization;

namespace Desafio.App.Models
{
    /// <summary>
    /// Lista de clientes paginada
    /// </summary>
    public class ClienteListar
    {
        /// <summary>
        /// Lista de clientes por página
        /// </summary>
        [JsonPropertyName("cliente")]
        public List<Cliente>? Cliente { get; set; }
       
        /// <summary>
        /// Quantidade total de clientes
        /// </summary>
        [JsonPropertyName("quant")]
        public int Quant { get; set; }
    }
}
