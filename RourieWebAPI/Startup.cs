using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using DBAccessLibrary;
using Microsoft.Extensions.Hosting;

namespace RourieWebAPI
{
    public class Startup
    {
        //Configuration is injected
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Dependency injection service container
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddMvc();

            services.AddDbContext<DBAccessLibrary.DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DBConnection")),ServiceLifetime.Transient);

            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
        }

        /*MIDDILEWARES are pieces of software that handles request and responses
         Uses request and response pipelines, shortcicuits if needed to the next middleware, the order is important
         Examples: Logging, StaticFiles, Authorization, Authentication, MVC, etc.
        */

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //using Ilogger middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //request pipeline with some middlewares
            if (env.IsDevelopment()) //or env.isEnvironment("Development") //or custom environment
            {
                app.UseDeveloperExceptionPage();
            }
            else app.UseExceptionHandler("/Error");

            app.UseRouting();

            /*
            app.UseDefaultFiles();// to show default/index.html by defaulr on the root request
            app.UseStaticFiles(); //this middleware is used to show static files
            */
            //or simply
            app.UseFileServer();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Companies}/{action=Index}/{id?}");
            });


        }
    }
}
