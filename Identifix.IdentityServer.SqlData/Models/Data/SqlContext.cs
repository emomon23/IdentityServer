using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identifix.IdentityServer.Infrastructure;

namespace Identifix.IdentityServer.Models.Data
{
    public class SqlContext : DbContext
    {
        //Enable Migrations needed a default constructor
        public SqlContext() : base(ConfigurationManager.ConnectionStrings["UserDatabase"].ConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public SqlContext(IDatabaseSettings settings) : base(settings.UserDatabaseConnectionString)
        {
            Guard.IsNotNull(settings, "settings");
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public virtual IDbSet<UserAccount> Users { get; set; }
        public virtual IDbSet<Shop> Shops { get; set; }
        public virtual IDbSet<Address> Addresses { get; set; }
        public virtual IDbSet<Country> Countries { get; set; }
        public virtual IDbSet<State> States { get; set; }

        public virtual IDbSet<ResetPasswordLink> ResetPasswordLinks {get;set;}
    }
}
