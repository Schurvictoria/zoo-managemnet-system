using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.Services
{
    public class AnimalTransferService : IAnimalTransferService, IBaseService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IEnclosureRepository _enclosureRepo;
        private readonly ILoggingService _logger;

        public AnimalTransferService(
            IAnimalRepository animalRepo, 
            IEnclosureRepository enclosureRepo,
            ILoggingService logger)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
            _logger = logger;
        }

        public async Task<bool> ValidateAsync()
        {
            try
            {
                var animals = await _animalRepo.GetAllAsync();
                var enclosures = await _enclosureRepo.GetAllAsync();
                return animals != null && enclosures != null;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Validation failed", ex);
                return false;
            }
        }

        public async Task TransferAnimalAsync(Guid animalId, Guid fromId, Guid toId)
        {
            try
            {
                var animal = await _animalRepo.GetByIdAsync(animalId.ToString());
                if (animal == null)
                {
                    throw new InvalidOperationException($"Животное с ID {animalId} не найдено");
                }

                var from = await _enclosureRepo.GetByIdAsync(fromId.ToString());
                if (from == null)
                {
                    throw new InvalidOperationException($"Исходный вольер с ID {fromId} не найден");
                }

                var to = await _enclosureRepo.GetByIdAsync(toId.ToString());
                if (to == null)
                {
                    throw new InvalidOperationException($"Целевой вольер с ID {toId} не найден");
                }

                if (animal.Enclosure == null || animal.Enclosure.Id != from.Id)
                {
                    throw new InvalidOperationException("Животное не находится в указанном исходном вольере");
                }

                from.RemoveAnimal(animal);
                if (!to.AddAnimal(animal))
                {
                    throw new InvalidOperationException("Целевой вольер переполнен");
                }

                animal.UpdateEnclosure(to);

                await _animalRepo.UpdateAsync(animal);
                await _enclosureRepo.UpdateAsync(from);
                await _enclosureRepo.UpdateAsync(to);

                await _logger.LogInfoAsync($"Животное {animalId} успешно перемещено из вольера {fromId} в вольер {toId}");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Ошибка при перемещении животного {animalId}", ex);
                throw;
            }
        }
    }
} 