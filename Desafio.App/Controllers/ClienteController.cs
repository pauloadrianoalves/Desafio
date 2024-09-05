using Desafio.App.Api;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.App.Controllers
{
	[Route("clientes")]
    public class ClienteController : Controller
    {
        private readonly ClienteApi _client;

        public ClienteController(ClienteApi client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> Salvar(Models.Cliente cliente)
        {
            try
            {
                var validar = _client.Validar(cliente);
                if (!string.IsNullOrEmpty(validar)) return Json(new { status = false, msg = validar });
                var status = await _client.Gravar(cliente);
                return status
                    ? Json(new { status })
                    : Json(new { status = false, msg = "Não foi possível salvar o cliente. Tente novamente em instantes." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = ex.Message });
            }
        }

        [HttpPost("editar")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var ficha = await _client.Ficha(id);
                return ficha?.Codigo > 0
                    ? Json(new { status = true, msg = ficha })
                    : Json(new { status = false, msg = "Cliente não encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = ex.Message });
            }
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var status = await _client.Excluir(id);
                return status
                    ? Json(new { status })
                    : Json(new { status, msg = "Cliente não encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = ex.Message });
            }
        }

        [HttpPost("carregar")]
        public async Task<IActionResult> Carregar()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var pagina_inicio = Request.Form["start"].FirstOrDefault();
                var regpagina = Request.Form["length"].FirstOrDefault();
                var coluna = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var ordenacao = Request.Form["order[0][dir]"].FirstOrDefault();
                var pesquisar = Request.Form["search[value]"].FirstOrDefault();
                int pagina_tamanho = regpagina != null ? Convert.ToInt32(regpagina) : 0;
                int pagina_atual = pagina_inicio != null ? Convert.ToInt32(pagina_inicio) : 0;
                var clientes = await _client.CarregarLista(pagina_atual, pagina_tamanho, coluna, ordenacao, pesquisar);
                var lista = clientes?.Cliente != null ? clientes.Cliente : new List<Models.Cliente>();
                var total_registros = clientes?.Quant != null ? clientes.Quant : 0;
                return Json(new { draw = draw, recordsFiltered = total_registros, recordsTotal = total_registros, data = lista });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
