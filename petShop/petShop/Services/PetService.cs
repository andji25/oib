using petShop.Model;
using petShop.Repository;
using petShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class PetService : IPetService
{
    private const int MaxPets = 10;

    private readonly IPetRepository petRepository;
    private readonly ILogService logService;

    public PetService(IPetRepository petRepository, ILogService logService)
    {
        this.petRepository = petRepository;
        this.logService = logService;
    }

    public void AddPet(Pet pet)
    {
        logService.Log(LogType.INFO, "Attempt to add pet");

        if (Session.CurrentUser == null)
        {
            logService.Log(LogType.ERROR, "Unauthorized access - no user");
            throw new UnauthorizedAccessException();
        }

        if (Session.CurrentUser.Role != Role.Manager)
        {
            logService.Log(LogType.WARNING, "Non-manager attempted to add pet");
            throw new UnauthorizedAccessException("Only manager can add pets.");
        }

        var pets = petRepository.GetAll();
        if (pets.Count >= MaxPets)
        {
            logService.Log(LogType.WARNING, "Pet shop capacity exceeded");
            throw new InvalidOperationException("Pet shop is full.");
        }

        petRepository.Add(pet);
        logService.Log(LogType.INFO, $"Pet added: {pet.Name}");
    }

    public IReadOnlyCollection<Pet> GetAllPets()
    {
        if (Session.CurrentUser == null)
            throw new UnauthorizedAccessException();

        return petRepository.GetAll().AsReadOnly();
    }

    public IReadOnlyCollection<Pet> GetAvailablePets()
    {
        if (Session.CurrentUser == null)
            throw new UnauthorizedAccessException();

        return petRepository.GetAll().Where(p => !p.Sold).ToList().AsReadOnly();
    }
}