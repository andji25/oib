using petShop.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace petShop.Repository
{
    public class JsonPetRepository : IPetRepository
    {
        private const string FilePath = "pets.json";

        public void Add(Pet pet)
        {
            var pets = GetAll();
            pets.Add(pet);
            Save(pets);
        }

        public List<Pet> GetAll()
        {
            if (!File.Exists(FilePath))
                return new List<Pet>();

            var json = File.ReadAllText(FilePath);

            var pets = JsonSerializer.Deserialize<List<Pet>>(json);

            if (pets == null)
                return new List<Pet>();

            return pets;
        }

        public void Update(Pet pet)
        {
            var pets = GetAll();
            var index = pets.FindIndex(p => p.Id == pet.Id);
            pets[index] = pet;
            Save(pets);
        }

        private void Save(List<Pet> pets)
        {
            var json = JsonSerializer.Serialize(pets, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(FilePath, json);
        }
    }
}
