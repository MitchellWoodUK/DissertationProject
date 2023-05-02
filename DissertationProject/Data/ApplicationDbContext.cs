using DissertationProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DissertationProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUserModel>
    {
        //public DbSet<CustomUserModel> Users { get; set; }
        public DbSet<FamilyModel> Families { get; set; }
        public DbSet<FamilyMembersModel> FamilyMembers { get; set; }
        public DbSet<FamilyTransactionModel> FamilyTransactions { get; set; }
        public DbSet<FamilyBillModel> FamilyBills { get; set; }
        public DbSet<FamilyBudgetModel> FamilyBudgets { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Calls the methods when the database is created.
            base.OnModelCreating(builder);
            SeedAdmin(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            //Method to seed roles into the database.
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "1a43ecdc-e161-4cc2-8476-004e461304fd",
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = "48c7b087-13b9-46bd-b10f-9566e89276ab"
                });
            builder.Entity<IdentityRole>().HasData(
               new IdentityRole()
               {
                   Id = "875ea618-c65e-4eac-9c0f-4a59f1ddaa2c",
                   Name = "Family_Head",
                   NormalizedName = "Family_Head".ToUpper(),
                   ConcurrencyStamp = "7684f586-4a4d-4b14-b09c-a1a936cd4c8d"
               });
            builder.Entity<IdentityRole>().HasData(
              new IdentityRole()
              {
                  Id = "fd32bd40-c6ba-474a-b959-55a3a8941347",
                  Name = "Family_Member",
                  NormalizedName = "Family_Member".ToUpper(),
                  ConcurrencyStamp = "3a8cfa8f-22b3-4323-ae1a-11884ac4ba6d"
              });
            builder.Entity<IdentityRole>().HasData(
             new IdentityRole()
             {
                 Id = "35981c10-0352-46be-9b0b-769ce6d85af9",
                 Name = "Family_Kid",
                 NormalizedName = "Family_Kid".ToUpper(),
                 ConcurrencyStamp = "16e1cc37-4269-4096-880c-53b14399beff"
             });
        }

        private void SeedAdmin(ModelBuilder builder)
        {
            //Method that seeds the admin account and provides it with all the necessary data.
            PasswordHasher<CustomUserModel> hasher = new PasswordHasher<CustomUserModel>();
            CustomUserModel user = new CustomUserModel();
            user.Id = "1a4df6c2-e479-40eb-8135-d492174424f2";
            user.UserName = "admin@moneytree.com";
            user.NormalizedUserName = "admin@moneytree.com".ToUpper();
            user.NormalizedEmail = "admin@moneytree.com".ToUpper();
            user.Email = "admin@moneytree.com";
            user.LockoutEnabled = false;
            user.Fname = "Admin";
            user.Sname = "Admin";
            user.JobName = "Admin";
            user.ConcurrencyStamp = "76a518b4-92f0-4b97-b4c2-86bb109ef976";
            user.PasswordHash = hasher.HashPassword(user, "Admin123!");
            builder.Entity<CustomUserModel>().HasData(user);
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            //Assigns the correct roles to the admin account.
            builder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>()
               {
                   RoleId = "1a43ecdc-e161-4cc2-8476-004e461304fd",
                   UserId = "1a4df6c2-e479-40eb-8135-d492174424f2"
               }
               );
            builder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>()
               {
                   RoleId = "875ea618-c65e-4eac-9c0f-4a59f1ddaa2c",
                   UserId = "1a4df6c2-e479-40eb-8135-d492174424f2"
               }
               );
            builder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>()
               {
                   RoleId = "fd32bd40-c6ba-474a-b959-55a3a8941347",
                   UserId = "1a4df6c2-e479-40eb-8135-d492174424f2"
               }
               );
            builder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>()
               {
                   RoleId = "35981c10-0352-46be-9b0b-769ce6d85af9",
                   UserId = "1a4df6c2-e479-40eb-8135-d492174424f2"
               }
               );
        }
    }
}