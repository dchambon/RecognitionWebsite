using CentricProjectTeam4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CentricProjectTeam4.DAL
{
    public class MyContext : DbContext
    {
        public MyContext() : base("name=DefaultConnection")
        {

        }
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<UserRecognition> UserRecognitions { get; set; }
    }
}