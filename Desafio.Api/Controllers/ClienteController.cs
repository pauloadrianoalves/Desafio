using Microsoft.AspNetCore.Mvc;

namespace Desafio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly Application.Service.IClienteService _cliente;

        public ClienteController(Application.Service.IClienteService cliente)
        {
            _cliente = cliente;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Application.Dtos.Cliente cliente)
        {
            try
            {
                var rsp = await _cliente.Gravar(cliente);
                return rsp ? Ok() : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Application.Dtos.Cliente cliente)
        {
            try
            {
                var rsp = await _cliente.Gravar(cliente);
                return rsp ? Ok() : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(int codigo)
        {
            try
            {
                var msg = await _cliente.Excluir(codigo);
                return msg == "" ? Ok() : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(int codigo)
        {
            try
            {
                var (rsp, msg) = await _cliente.Registro(codigo);
                return rsp != null ? Ok(rsp) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{nPaginas}/{tPagina}/{oColuna}/{ordem}/{filtro}")]
        public async Task<IActionResult> Get(int nPaginas, int tPagina, string oColuna, string ordem, string? filtro = "")
        {
            try
            {
                var (cliente, quant) = await _cliente.Listar(nPaginas, tPagina, oColuna, ordem, filtro);
                return cliente?.Count > 0 ? Ok(new { cliente, quant }) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{nPaginas}/{tPagina}/{oColuna}/{ordem}")]
        public async Task<IActionResult> Get(int nPaginas, int tPagina, string oColuna, string ordem)
        {
            try
            {
                var (cliente, quant) = await _cliente.Listar(nPaginas, tPagina, oColuna, ordem, "");
                return cliente?.Count > 0 ? Ok(new { cliente, quant }) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
