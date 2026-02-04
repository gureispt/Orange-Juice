using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AtivoController : ControllerBase
    {
        private readonly IAtivoService _ativoService;

        public AtivoController(IAtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var ativos = await _ativoService.ObterTodosAsync();
            return Ok(ativos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var ativo = await _ativoService.ObterPorIdAsync(id);
            if (ativo == null)
                return NotFound("Aitvo não encontrado");

            return Ok(ativo);
        }

        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> ObterPorTipo(TipoAtivo tipo)
        {
            var ativos = await _ativoService.ObterPorTipoAsync(tipo);
            return Ok(ativos);
        }

        [HttpPost("comprar")]
        public async Task<IActionResult> ComprarAtivo([FromBody] CompraVendaRequest request)
        {
            try
            {
                await _ativoService.ComprarAtivoAsync(request.UsuarioId, request.AtivoId, request.Quantidade);
                return Ok("Compra realizada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("vender")]
        public async Task<IActionResult> VenderAtivo([FromBody] CompraVendaRequest request)
        {
            try
            {
                await _ativoService.VenderAtivoAsync(request.UsuarioId, request.AtivoId, request.Quantidade);
                return Ok("Venda realizada com sucesso");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class CompraVendaRequest
    {
        public Guid UsuarioId { get; set; }
        public Guid AtivoId { get; set; }
        public int Quantidade { get; set; }
    }
}