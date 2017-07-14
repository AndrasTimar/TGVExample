using System.Linq;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext Context;

        public UserRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public AppUser FindAppUserByName(string name)
        {
            return Context.Users.Where(x => x.UserName == name)
                .Include(x => x.Reservations)
                .ThenInclude(x => x.Trip)
                .FirstOrDefault();
        }
    }
}