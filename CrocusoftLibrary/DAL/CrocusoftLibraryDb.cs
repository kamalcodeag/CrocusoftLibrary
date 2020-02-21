using CrocusoftLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrocusoftLibrary.DAL
{
    public class CrocusoftLibraryDb : IdentityDbContext<CustomUser>
    {
        public CrocusoftLibraryDb(DbContextOptions<CrocusoftLibraryDb> options) : base(options)
        {
        }
    }
}
