using System.ComponentModel.DataAnnotations;

namespace Desafio.Application.Dtos
{
    public class Cliente
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "Campo é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Campo é obrigatório.")]
        public string? Rua { get; set; }

        [Required(ErrorMessage = "Campo é obrigatório.")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "Campo é obrigatório.")]
        public string? Bairro { get; set; }

        [Required(ErrorMessage = "Campo é obrigatório.")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "Campo é obrigatório.")]
        public string? Uf { get; set; }
    }
}