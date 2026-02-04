using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IContaService _contaService;

        public ContaController(IContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var conta = await _contaService.ObterPorIdAsync(id);
            if (conta == null)
                return NotFound("Conta não encontrada");

            return Ok(conta);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObterPorUsuario(Guid usuarioId)
        {
            var contas = await _contaService.ObterPorUsuarioIdAsync(usuarioId);
            return Ok(contas);
        }

        [HttpPost("{contaId}/depositar")]
        public async Task<IActionResult> Depositar(Guid contaId, [FromBody] decimal valor)
        {
            try
            {
                await _contaService.DepositarAsync(contaId, valor);
                return Ok("Depósito realizado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{contaId}/sacar")]
        public async Task<IActionResult> Sacar(Guid contaId, [FromBody] decimal valor)
        {
            try
            {
                await _contaService.SacarAsync(contaId, valor);
                return Ok("Depósito realizado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transferir-interna")]
        public async Task<IActionResult> TransferirInterna([FromBody] TransferenciaRequest request)
        {
            try
            {
                await _contaService.TransferirInternaAsync(request.ContaOrigemId, request.ContaDestinoId, request.Valor);
                return Ok("Transferência realizada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transferir-externa")]
        public async Task<IActionResult> TransferirExterna([FromBody] TransferenciaRequest request)
        {
            try
            {
                await _contaService.TransferirExternaAsync(request.ContaOrigemId, request.ContaDestinoId, request.Valor);
                return Ok("Transferência realizada com sucesso");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class TransferenciaRequest
    {
        public Guid ContaOrigemId { get; set; }
        public Guid ContaDestinoId { get; set; }
        public decimal Valor { get; set; }
    }
}