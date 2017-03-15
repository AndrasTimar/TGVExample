using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using Microsoft.AspNetCore.Identity;

namespace AndrasTimarTGV.Models.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public AppUser FindAppUserByName(string name) {
            return  _userRepository.FindAppUserByName(name);
        }
    }
}
