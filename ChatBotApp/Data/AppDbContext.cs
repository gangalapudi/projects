using ChatBotApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ChatBotApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
