using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;

namespace OrangeJuiceBank.Application.Interfaces
{
    public interface ICarteiraService
    {
        Task<IEnumerable<Carteira>> ObterCarteiraUsuarioAsync(Guid usuarioId);
    }
}