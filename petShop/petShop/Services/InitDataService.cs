using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using petShop.Model;

public static class InitDataService
{
    public static void InitUsers(IRepository<User> users)
    {
        if (!users.GetAll().Any())
        {
            users.Add(new User
            {
                Username = "manager",
                PasswordHash = AuthService.Hash("123"),
                Role = Role.Manager
            });
            users.Add(new User
            {
                Username = "seller",
                PasswordHash = AuthService.Hash("123"),
                Role = Role.Seller
            });
        }
    }

    public static void InitPets(IRepository<Pet> pets)
    {
        if (!pets.GetAll().Any())
        {
            pets.Add(new Pet { Name = "Rex", Price = 300, Type = PetType.Mammal });
            pets.Add(new Pet { Name = "Maca", Price = 150, Type = PetType.Mammal });
        }
    }
}
