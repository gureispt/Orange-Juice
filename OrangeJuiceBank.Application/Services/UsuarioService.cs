using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Interfaces;

namespace OrangeJuiceBank.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
        {
            //VALIDAÇÕES 
            var existeEmail = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (existeEmail != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var existeCPF = await _usuarioRepository.GetByCPFAsync(usuario.CPF);
            if (existeCPF != null)
            {
                throw new Exception("CPF já cadastrado");
            }
            return await _usuarioRepository.AddAsync(usuario);
        }

        public async Task<Usuario?> ObterPorCPFAsync(string cpf)
        {
            return await _usuarioRepository.GetByCPFAsync(cpf);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }
    }
}