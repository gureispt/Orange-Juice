using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Domain.Entities;
using OrangeJuiceBank.Domain.Enums;
using OrangeJuiceBank.Domain.Interfaces;

namespace OrangeJuiceBank.Application.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly ICarteiraRepository _carteiraRepository;

        public CarteiraService(ICarteiraRepository carteiraRepository)
        {
            _carteiraRepository = carteiraRepository;
        }

        public async Task<IEnumerable<Carteira>> ObterCarteiraUsuarioAsync(Guid usuarioId)
        {
            return await _carteiraRepository.GetByUsuarioIdAsync(usuarioId);
        }
    }
}