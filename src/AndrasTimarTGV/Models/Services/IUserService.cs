using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface IUserService
    {
        AppUser FindAppUserByName(string name);
    }
}