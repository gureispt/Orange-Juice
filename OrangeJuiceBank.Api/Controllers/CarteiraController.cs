using Microsoft.AspNetCore.Mvc;
using OrangeJuiceBank.Api.DTOs;
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

                //MAPEANDO DTO
                var carteiraDTO = carteira.Select(c => new CarteiraDTO
                {
                    Id = c.Id,
                    AtivoId = c.AtivoId,
                    Quantidade = c.Quantidade,
                    PrecoMedioCompra = c.PrecoMedioCompra,
                    DataPrimeiraCompra = c.DataPrimeiraCompra,
                    DataUltimaAtualizacao = c.DataUltimaAtualizacao,
                    CodigoAtivo = c.Ativo.Codigo,
                    NomeAtivo = c.Ativo.Nome,
                    PrecoAtualAtivo = c.Ativo.PrecoAtual,
                    TipoAtivo = (int)c.Ativo.TipoAtivo
                });
                return Ok(carteiraDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = $"Erro ao buscar sua carteira: {ex.Message}" });
            }
        }
    }

}