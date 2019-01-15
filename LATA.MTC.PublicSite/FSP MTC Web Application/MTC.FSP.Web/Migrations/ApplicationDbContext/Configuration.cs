using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Migrations.ApplicationDbContext
{
    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var adminRole = new ApplicationRole
                {
                    Name = "Admin",
                    IsDeletable = false
                };
                roleManager.Create(adminRole);
            }

            if (!roleManager.RoleExists("MTC"))
            {
                var mtcRole = new ApplicationRole
                {
                    Name = "MTC",
                    IsDeletable = false
                };
                roleManager.Create(mtcRole);
            }

            if (!roleManager.RoleExists("FSPPartner"))
            {
                var fSPPartnerRole = new ApplicationRole
                {
                    Name = "FSPPartner",
                    IsDeletable = false
                };

                roleManager.Create(fSPPartnerRole);
            }

            if (!roleManager.RoleExists("CHPOfficer"))
            {
                var chpOfficerRole = new ApplicationRole
                {
                    Name = "CHPOfficer",
                    IsDeletable = false
                };

                roleManager.Create(chpOfficerRole);
            }

            if (!roleManager.RoleExists("TowContractor"))
            {
                var towContractorRole = new ApplicationRole
                {
                    Name = "TowContractor",
                    IsDeletable = false
                };
                roleManager.Create(towContractorRole);
            }

            if (!roleManager.RoleExists("InVehicleContractor"))
            {
                var inVehicleContractorRole = new ApplicationRole
                {
                    Name = "InVehicleContractor",
                    IsDeletable = false
                };
                roleManager.Create(inVehicleContractorRole);
            }

            if (!roleManager.RoleExists("DataConsultant"))
            {
                var dataConsultantRole = new ApplicationRole
                {
                    Name = "DataConsultant",
                    IsDeletable = false
                };
                roleManager.Create(dataConsultantRole);
            }

            if (!roleManager.RoleExists("GeneralUser"))
            {
                var generalUserRole = new ApplicationRole
                {
                    Name = "GeneralUser",
                    IsDeletable = false
                };
                roleManager.Create(generalUserRole);
            }


            if (!roleManager.RoleExists("Guest"))
            {
                var readerRole = new ApplicationRole
                {
                    Name = "Guest",
                    IsDeletable = false
                };

                roleManager.Create(readerRole);
            }


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var mtcAdmin = userManager.FindByName("mtcAdmin@lata.com");
            if (mtcAdmin == null)
            {
                mtcAdmin = new ApplicationUser
                {
                    UserName = "mtcAdmin@lata.com",
                    Email = "mtcAdmin@lata.com",
                    LastName = "Admin",
                    FirstName = "MTC",
                    Mobile = "",
                    NickName = "",
                    EmailConfirmed = true,
                    PhoneNumber = "",
                    PhoneNumberConfirmed = true
                };

                IdentityUserRole userRole = new IdentityUserRole();
                var adminRole = roleManager.FindByName("Admin");
                userRole.RoleId = adminRole.Id;
                userRole.UserId = mtcAdmin.Id;
                mtcAdmin.Roles.Add(userRole);

                var userResult = userManager.Create(mtcAdmin, "L@t@2014");
            }

            var tkoseogluAdmin = userManager.FindByName("tkoseoglu@live.com");
            if (tkoseogluAdmin == null)
            {
                tkoseogluAdmin = new ApplicationUser
                {
                    UserName = "tkoseoglu@live.com",
                    Email = "tkoseoglu@live.com",
                    LastName = "Koseoglu",
                    FirstName = "Tolga",
                    Mobile = "949.243.6632",
                    NickName = "",
                    EmailConfirmed = true,
                    PhoneNumber = "949.243.6632",
                    PhoneNumberConfirmed = true
                };

                IdentityUserRole userRole = new IdentityUserRole();
                var adminRole = roleManager.FindByName("Admin");
                userRole.RoleId = adminRole.Id;
                userRole.UserId = tkoseogluAdmin.Id;
                tkoseogluAdmin.Roles.Add(userRole);

                var userResult = userManager.Create(tkoseogluAdmin, "Ktk1976^");
            }


            var mtcContractor = userManager.FindByName("mtcContractor@lata.com");
            if (mtcContractor == null)
            {
                mtcContractor = new ApplicationUser
                {
                    UserName = "mtcContractor@lata.com",
                    Email = "mtcContractor@lata.com",
                    LastName = "Contractor",
                    FirstName = "MTC",
                    Mobile = "",
                    NickName = "",
                    EmailConfirmed = true,
                    PhoneNumber = "",
                    PhoneNumberConfirmed = true
                };

                IdentityUserRole userRole = new IdentityUserRole();
                var contractorRole = roleManager.FindByName("Contractor");
                userRole.RoleId = contractorRole.Id;
                userRole.UserId = mtcContractor.Id;
                mtcContractor.Roles.Add(userRole);

                var userResult = userManager.Create(mtcContractor, "L@t@2014");
            }


            var mtcFspPartner = userManager.FindByName("mtcFspPartner@lata.com");
            if (mtcFspPartner == null)
            {
                mtcFspPartner = new ApplicationUser
                {
                    UserName = "mtcFspPartner@lata.com",
                    Email = "mtcFspPartner@lata.com",
                    LastName = "Fsp Partner",
                    FirstName = "MTC",
                    Mobile = "",
                    NickName = "",
                    EmailConfirmed = true,
                    PhoneNumber = "",
                    PhoneNumberConfirmed = true
                };

                IdentityUserRole userRole = new IdentityUserRole();
                var fspPartnerRole = roleManager.FindByName("FSPPartner");
                userRole.RoleId = fspPartnerRole.Id;
                userRole.UserId = mtcFspPartner.Id;
                mtcFspPartner.Roles.Add(userRole);

                var userResult = userManager.Create(mtcFspPartner, "L@t@2014");
            }
        }
    }
}