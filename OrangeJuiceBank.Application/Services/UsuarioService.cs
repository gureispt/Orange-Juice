using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;
using OrangeJuiceBank.Domain.Interfaces;

namespace OrangeJuiceBank.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IContaRepository _contaRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IContaRepository contaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _contaRepository = contaRepository;
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

            //adicionado o HASH de Senha
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
            usuario.DataCriacao = DateTime.UtcNow;

            //CRIA USUARIO
            var usuarioCriado = await _usuarioRepository.AddAsync(usuario);

            //CRIA 2 CONTAS AUTOMATICAMENTE
            var contaCorrente = new Conta
            {
                Id = Guid.NewGuid(),
                NumeroConta = $"CI-{usuario.CPF.Substring(0, 6)}",
                Saldo = 0,
                TipoConta = TipoConta.Corrente,
                UsuarioId = usuarioCriado.Id,
                DataCriacao = DateTime.UtcNow
            };

            var contaInvestimento = new Conta
            {
                Id = Guid.NewGuid(),
                NumeroConta = $"CI-{usuario.CPF.Substring(0, 6)}",
                Saldo = 0,
                TipoConta = TipoConta.Investimento,
                UsuarioId = usuarioCriado.Id,
                DataCriacao = DateTime.UtcNow
            };

            await _contaRepository.AddAsync(contaCorrente);
            await _contaRepository.AddAsync(contaInvestimento);

            return usuarioCriado;
        }

        public async Task<Usuario?> ValidarLoginAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario == null) return null;

            bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);

            return senhaValida ? usuario : null;
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