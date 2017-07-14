using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface IUserRepository
    {
        AppUser FindAppUserByName(string name);
    }
}