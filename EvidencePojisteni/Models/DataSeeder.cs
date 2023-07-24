using EvidencePojisteni.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EvidencePojisteni.DataSeeder
{
    public enum AppMode { Anonymous, User, Admin }

    public static class DataSeeder
    {

        public static async Task RegisterAdmin(this WebApplication webApplication,
                                               string userEmail, string userPassword)
        {
            var adminRoleName = "admin";

            using (var scope = webApplication.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if (!await roleManager.RoleExistsAsync(adminRoleName))
                    await roleManager.CreateAsync(new IdentityRole(adminRoleName));

                IdentityUser user = await userManager.FindByEmailAsync(userEmail);

                if (user is null)
                    user = await CreateUser(userManager, userEmail, userPassword);

                if (!await userManager.IsInRoleAsync(user, adminRoleName))
                    await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }

        private static async Task<IdentityUser> CreateUser(UserManager<IdentityUser> userManager,
                                                           string userEmail, string password)
        {
            IdentityUser user = null;
            var result = await userManager.CreateAsync(new IdentityUser { UserName = userEmail, Email = userEmail }, password);
            if (result.Succeeded)
            {
                user = await userManager.FindByEmailAsync(userEmail);
            }
            return user;
        }

        public static AppMode GetApplicationMode()
        {
            return AppMode.Anonymous;
        }

        public static async Task SeedEmptyDatabase(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var context = new ApplicationDbContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

                if (context.Insured.Any())
                {
                    return;   
                }

                context.Insured.AddRange
                (
                    new Models.Insured { FirstName = "Tomáš", LastName = "Veselý", Email = "tomas.vesely@test.cz", Phone = "123 123 123", Street = "Dlouhá 48", City = "Praha", PostalCode = "190 11" },
                    new Models.Insured { FirstName = "Lucie", LastName = "Novotná", Email = "lucie.novotna@test.cz", Phone = "234 234 234", Street = "Hlavní 7", City = "Plzeň", PostalCode = "301 00" },
                    new Models.Insured { FirstName = "Eva", LastName = "Konečná", Email = "eva.konecna@test.cz", Phone = "345 345 345", Street = "Nová 1", City = "Olomouc", PostalCode = "779 00" },
                    new Models.Insured { FirstName = "Martin", LastName = "Dvořák", Email = "martin.dvorak@test.cz", Phone = "456 456 456", Street = "Stará 99", City = "České Budějovice", PostalCode = "370 01" }
                );
                context.SaveChanges();  

                await CreateUser(userManager, "tomas.vesely@test.cz", "Vesely1.");
                await CreateUser(userManager, "lucie.novotna@test.cz", "Novotna1.");
                await CreateUser(userManager, "eva.konecna@test.cz", "Konecna1.");
                await CreateUser(userManager, "martin.dvorak@test.cz", "Dvorak1.");

                var tomasVesely = await context.Insured.FirstOrDefaultAsync(m => m.Email == "tomas.vesely@test.cz");
                var lucieNovotna = await context.Insured.FirstOrDefaultAsync(m => m.Email == "lucie.novotna@test.cz");
                var evaKonecna = await context.Insured.FirstOrDefaultAsync(m => m.Email == "eva.konecna@test.cz");
                var martinDvorak = await context.Insured.FirstOrDefaultAsync(m => m.Email == "martin.dvorak@test.cz");

                context.Insurance.AddRange
                (
                    new Models.Insurance { Type = "Životní pojištění", Amount = 5000000M, Subject = "Úmrtí", ValidFrom = DateTime.Parse("2017-4-1"), ValidUntil = DateTime.Parse("2047-4-1"), InsuredId = tomasVesely.Id },
                    new Models.Insurance { Type = "Cestovní pojištění", Amount = 3000000M, Subject = "Léčebné výlohy", ValidFrom = DateTime.Parse("2017-4-1"), ValidUntil = DateTime.Parse("2047-4-1"), InsuredId = tomasVesely.Id },
                    new Models.Insurance { Type = "Pojištění majetku", Amount = 1000000M, Subject = "Auto", ValidFrom = DateTime.Parse("2020-5-5"), ValidUntil = DateTime.Parse("2035-5-5"), InsuredId = tomasVesely.Id },
                   
                    new Models.Insurance { Type = "Úrazové pojištění", Amount = 2000000M, Subject = "Úraz", ValidFrom = DateTime.Parse("2005-8-2"), ValidUntil = DateTime.Parse("2035-8-2"), InsuredId = lucieNovotna.Id },

                    new Models.Insurance { Type = "Úrazové pojištění", Amount = 1000000M, Subject = "Úraz", ValidFrom = DateTime.Parse("2009-9-9"), ValidUntil = DateTime.Parse("2039-9-9"), InsuredId = evaKonecna.Id },
                    new Models.Insurance { Type = "Cestovní pojištění", Amount = 4000000M, Subject = "Léčebné výlohy", ValidFrom = DateTime.Parse("2018-8-9"), ValidUntil = DateTime.Parse("2028-8-9"), InsuredId = evaKonecna.Id },                  

                    new Models.Insurance { Type = "Úrazové pojištění", Amount = 1000000M, Subject = "Úraz", ValidFrom = DateTime.Parse("2015-10-7"), ValidUntil = DateTime.Parse("2035-10-7"), InsuredId = martinDvorak.Id },
                    new Models.Insurance { Type = "Pojištění majetku", Amount = 5000000M, Subject = "Dům", ValidFrom = DateTime.Parse("2022-12-12"), ValidUntil = DateTime.Parse("2052-12-12"), InsuredId = martinDvorak.Id }             
                );
                context.SaveChanges();

                var pojisteniLecby = await context.Insurance.FirstOrDefaultAsync(m => m.Subject == "Léčebné výlohy");
                var pojisteniAuta = await context.Insurance.FirstOrDefaultAsync(m => m.Subject == "Auto");

                context.Event.AddRange
                (
                    new Models.InsEvent { Name = "Autonehoda", Description = "Dopravní autonehoda zapříčiněna pojištěncem ...", Date = DateTime.Parse("2021-4-5"), Status = "Zamítnuto", InsuranceId = pojisteniAuta.Id },
                    new Models.InsEvent { Name = "Zlomenina ruky", Description = "Pojištěnec si zlomil ruku během dovolené v Austrálii ...", Date = DateTime.Parse("2023-10-2"), Status = "Vyřizuje se", Amount = 80000M, InsuranceId = pojisteniLecby.Id }
                );
                context.SaveChanges();
            }
        }

    }
}
