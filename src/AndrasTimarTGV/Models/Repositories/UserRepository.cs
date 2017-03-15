using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public AppUser FindAppUserByName(string name)
        {
            return _context.Users.Where(x => x.UserName == name)
                .Include(x=>x.Reservations)
                    .ThenInclude(x=>x.Trip)
                .FirstOrDefault();
        }
    }
}
