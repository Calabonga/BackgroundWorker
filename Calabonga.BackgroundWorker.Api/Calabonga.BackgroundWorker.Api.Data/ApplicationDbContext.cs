using Calabonga.BackgroundWorker.Api.Data.Base;
using Calabonga.BackgroundWorker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calabonga.BackgroundWorker.Api.Data
{
    /// <summary>
    /// Database context for current application
    /// </summary>
    public class ApplicationDbContext : DbContextBase, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region System

        public DbSet<Work> Works { get; set; }
        
        public DbSet<ApplicationUserProfile> Profiles { get; set; }
        
        public DbSet<MicroservicePermission> Permissions { get; set; }

        #endregion
    }
}