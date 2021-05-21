using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookomaticDB
{
    public class CookomaticDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseMySQL("Server=localhost;Database=cookomatic;UID=root;Password=");
            //optionBuilder.UseMySQL("Server=192.168.1.109;Database=cookomatic;UID=alumne;Password=alumne");
        }
    }
}
