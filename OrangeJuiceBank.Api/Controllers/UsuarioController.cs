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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario não encontrado");

            return Ok(usuario);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            var usuario = await _usuarioService.ObterPorEmailAsync(email);
            if (usuario == null)
                return NotFound("Usuario não encontrado");

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                usuario.Id = Guid.NewGuid();
                usuario.DataCriacao = DateTime.UtcNow;
                var usuarioCriado = await _usuarioService.CriarUsuarioAsync(usuario);
                return CreatedAtAction(nameof(ObterPorId), new { id = usuarioCriado.Id }, usuarioCriado);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}