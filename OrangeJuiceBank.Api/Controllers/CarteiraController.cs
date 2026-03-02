using Microsoft.AspNetCore.Mvc;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarteiraController : ControllerBase
    {
        private readonly ICarteiraService _carteiraService;

        public CarteiraController(ICarteiraService carteiraService)
        {
            _carteiraService = carteiraService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObterCarteiraUsuario(Guid usuarioId)
        {
            try
            {
                var carteira = await _carteiraService.ObterCarteiraUsuarioAsync(usuarioId);
                return Ok(carteira);
            }catch(Exception ex)
            {
                return BadRequest(new { mensagem = $"Erro ao buscar sua carteira: {ex.Message}" });
            }
        }
    }

}