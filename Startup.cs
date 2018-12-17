using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Avanage.SorterFeelLite.UI.Data;
using Avanage.SorterFeelLite.UI.Models;
using Avanage.SorterFeelLite.UI.Services;

namespace Avanage.SorterFeelLite.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<ApplicationDbContext>(options =>
            //      options.UseSqlite("Data Source=avanage_sorter.db"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserRepository, EfUserRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            RunEFSchemaMigrations(app, env);

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RunEFSchemaMigrations(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                       .GetRequiredService<IServiceScopeFactory>()
                       .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();

                    //CreateRoles(context);
                    //CreateDefaultSuperUser(context, serviceScope.ServiceProvider.GetService<IPasswordHasher<ApplicationUser>>());

                    context.SaveChanges();
                }
            }
        }

        private void CreateDefaultSuperUser(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            var defaultSuperUsername = "superuser";
            var defaultSuperUserId = "2caea85c-33d1-43c3-84f7-68e946a09e11";
            if (context.Users.FirstOrDefault(u => u.UserName == defaultSuperUsername) == null)
            {
                var defaultSuperUser = new ApplicationUser
                {
                    Id = defaultSuperUserId,
                    UserName = defaultSuperUsername,
                    Email = "",
                    NormalizedEmail = defaultSuperUsername.ToUpper(),
                    PhoneNumber = "",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedUserName = defaultSuperUsername.ToUpper(),
                    IsActive = true
                };

                var hashedPassword = passwordHasher.HashPassword(defaultSuperUser, "Password");
                defaultSuperUser.PasswordHash = hashedPassword;

                context.Users.Add(defaultSuperUser);

                var superAdminOfficerRoleId = Convert.ToInt16(Roles.ADMIN_SUPERVISOR).ToString();
                var defaultSuperAdminRoleAssigned = context.UserRoles.FirstOrDefault(ur => ur.RoleId == superAdminOfficerRoleId && ur.UserId == defaultSuperUserId) != null;
                if (!defaultSuperAdminRoleAssigned)
                {
                    context.UserRoles.Add(new IdentityUserRole<string>
                    {
                        RoleId = superAdminOfficerRoleId,
                        UserId = defaultSuperUserId
                    });
                }
            }            
        }


        private void CreateDefaultAdminUser(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            var defaultAdminUsername = "admin";
            var defaultAdminUserId = "1caea85c-33d1-43c3-84f7-68e946a09e10";
            if (context.Users.FirstOrDefault(u => u.UserName == defaultAdminUsername) == null)
            {
                var defaultAdminUser = new ApplicationUser
                {
                    Id = defaultAdminUserId,
                    UserName = defaultAdminUsername,
                    Email = "",
                    NormalizedEmail = defaultAdminUsername.ToUpper(),
                    PhoneNumber = "",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedUserName = defaultAdminUsername.ToUpper(),
                };

                var hashedPassword = passwordHasher.HashPassword(defaultAdminUser, "Password");
                defaultAdminUser.PasswordHash = hashedPassword;

                context.Users.Add(defaultAdminUser);
            }

            var adminOfficerRoleId = Convert.ToInt16(Roles.ADMIN_OFFICER).ToString();
            var defaultAdminRoleAssigned = context.UserRoles.FirstOrDefault(ur => ur.RoleId == adminOfficerRoleId && ur.UserId == defaultAdminUserId) != null;
            if (!defaultAdminRoleAssigned)
            {
                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = adminOfficerRoleId,
                    UserId = defaultAdminUserId
                });
            }
        }

        private void CreateRoles(ApplicationDbContext context)
        {
            foreach (var roleKV in EnumUtil.ToKeyValuePairs(typeof(Roles)))
            {
                if (context.Roles.FirstOrDefault(r => r.Id == roleKV.Key) == null)
                {
                    context.Roles.Add(new IdentityRole
                    {
                        Id = roleKV.Key,
                        Name = roleKV.Value.Replace("_", " "),
                        NormalizedName = roleKV.Value.Replace("_", " ").ToUpper()
                    });
                }
            }
        }
    }
}
