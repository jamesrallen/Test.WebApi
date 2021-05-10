// Test.WebApi/Test.WebApi/MessageDbContext.cs
// James Allen
// 2021/05/09/2:06 PM

using Microsoft.EntityFrameworkCore;
using Test.WebApi.Models;

namespace Test.WebApi.Data
{
    public class MessageDbContext : DbContext
    {

        public MessageDbContext(DbContextOptions<MessageDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}

