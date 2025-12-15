using System.Collections.Generic;
using System.Linq;
using petShop.Model;

public class PetService
{
    private IRepository<Pet> _repo;
    private LoggingService _logger;

    public PetService(IRepository<Pet> repo, LoggingService logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public void AddPet(Pet pet)
    {
        if (_repo.GetAll().Count() >= 10)
            throw new System.Exception("U prodavnici može biti max 10 ljubimaca.");

        _repo.Add(pet);
        _logger.Log("Added pet: " + pet.Name);
    }

    public IEnumerable<Pet> GetAll() => _repo.GetAll();
    public IEnumerable<Pet> GetAvailable() => _repo.GetAll().Where(p => !p.Sold);
}
