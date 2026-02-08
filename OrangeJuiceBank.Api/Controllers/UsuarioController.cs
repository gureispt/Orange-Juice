using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _usuarioService.ValidarLoginAsync(request.Email, request.Senha);

                if (usuario == null) return Unauthorized("Email ou senha inválidos");

                //Não retornar o hash da senha
                usuario.SenhaHash = string.Empty;

                return Ok(usuario);
            } catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastroRequest request)
        {
            try
            {
                var usuario = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Nome,
                    Email = request.Email,
                    CPF = request.CPF,
                    SenhaHash = request.Senha //o service transforma em Hash
                };

                var usuarioCriado = await _usuarioService.CriarUsuarioAsync(usuario);

                usuarioCriado.SenhaHash = string.Empty;

                return CreatedAtAction(nameof(ObterPorId), new { id = usuarioCriado.Id }, usuarioCriado);
            }catch(Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
        }
        
        public class CadastroRequest
        {
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string CPF { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
        }
    }
}