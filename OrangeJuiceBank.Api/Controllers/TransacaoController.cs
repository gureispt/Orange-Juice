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
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        [HttpGet("conta/{contaId}")]
        public async Task<ActionResult> ObterPorConta(Guid contaId)
        {
            try
            {
                var transacoes = await _transacaoService.ObterPorContaAsync(contaId);
                return Ok(transacoes);
            }
            catch (Exception err)
            {
                return BadRequest(new { mensagem = $"Erro ao buscar transações pela conta: {err.Message}" });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObterPorUsuario(Guid usuarioId)
        {
            try
            {
                var transacoes = await _transacaoService.ObterPorUsuarioAsync(usuarioId);
                return Ok(transacoes);
            }catch(Exception err)
            {
                return BadRequest(new { mensagem = $"Erro ao buscar transações por usuario: {err.Message}" });
            }
        }

        [HttpGet("usuario/{usuarioId}/periodo")]
        public async Task<IActionResult> ObterPorPerido(Guid usuarioId, [FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            try
            {
                var transacoes = await _transacaoService.ObterPorPeriodoAsync(usuarioId, dataInicio, dataFim);
                return Ok(transacoes);
            }
            catch(Exception err)
            {
                return BadRequest(new { mensagem = $"Erro ao buscar transações nesse periodo: {err.Message}" });
            }
        }
    }
}