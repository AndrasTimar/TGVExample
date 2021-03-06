﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public Language DefaultLanguage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}