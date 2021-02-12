using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ElevenNote.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }//end of constructor
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }//end of method Create

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Conventions
                .Remove<PluralizingTableNameConvention>();

            modelBuilder
                .Configurations
                .Add(new IdentityUserLoginConfiguration())
                .Add(new IdentityUserRoleConfiguration());


        }//end of method OnModelCreating



    }//end of class ApplicationDbContext

    //here is our first bit of add on
    //we need a new class in this namespace

    public class IdentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
    {
        //just the constructor
        public IdentityUserLoginConfiguration()
        {
            HasKey(iur => iur.UserId);
        }//end of constructor

    }//end of class IdentityUserLoginConfiguration


    //and another class

    public class IdentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
    {
        //constructor
        public IdentityUserRoleConfiguration()
        {
            HasKey(iur => iur.UserId);
        }//end of constructor


    }//end of class IdentityUserRoleConfiguration



}//end of namespace