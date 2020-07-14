using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using MySwitch.Core.Migrations;

namespace MySwitch.Core.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            //Database.SetInitializer<ApplicationDbContext>(null);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }


        public DbSet<FinancialInstitution> FinancialInstitutions { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<SinkNode> SinkNodes { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<SourceNode> SourceNodes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Route> Routes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //these methods don't exist in my case
            //modelBuilder.Entity<Transaction>(entity => entity.ToTable("Transaction"));
            //modelBuilder.Entity<Transaction>().ToTable("Transaction");
        }

    }
    }

