using OrangeJuiceBank.Domain.Entities;

namespace OrangeJuiceBank.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorCPFAsync(string cpf);
        Task<IEnumerable<Usuario>> ObterTodosAsync();
    }
}