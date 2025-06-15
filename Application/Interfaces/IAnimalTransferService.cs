using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAnimalTransferService
    {
        Task TransferAnimalAsync(Guid animalId, Guid fromId, Guid toId);
    }
}
