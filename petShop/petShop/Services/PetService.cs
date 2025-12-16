using System;
using System.Collections.Generic;
using System.Linq;
using petShop.Model;
using petShop.Services;

public class PetService : IPetService
{
    private const int MaxPets = 10;
    private readonly List<Pet> pets = new List<Pet>();
    public void AddPet(Pet pet)
    {
        if (Session.CurrentUser == null)
            throw new UnauthorizedAccessException();

        if (Session.CurrentUser.Role != Role.Manager)
            throw new UnauthorizedAccessException("Only manager can add pets.");

        if (pets.Count >= MaxPets)
            throw new InvalidOperationException("Pet shop is full.");

        pets.Add(pet);
    }

    public IReadOnlyCollection<Pet> GetAllPets()
    {
        if (Session.CurrentUser == null)
            throw new UnauthorizedAccessException();

        return pets.AsReadOnly();
    }

    public IReadOnlyCollection<Pet> GetAvailablePets()
    {
        if (Session.CurrentUser == null)
            throw new UnauthorizedAccessException();

        return pets.Where(p => !p.Sold).ToList();
    }
}
