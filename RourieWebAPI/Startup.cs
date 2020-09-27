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

            //adding authentication middleware
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddMvc();

            //adding our dbcontext
            services.AddDbContext<DBAccessLibrary.DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DBConnection")),ServiceLifetime.Transient);

            //adding our repository services
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
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
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error/500"); //an important middleware

            //any error goes to Error controller. Such as Error/500 or Error/404
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            //or
            //app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHsts();

            app.UseRouting();

            /*
            app.UseDefaultFiles();// to show default/index.html by defaulr on the root request
            app.UseStaticFiles(); //this middleware is used to show static files. 
            */
            //or simply
            app.UseFileServer();

            //these are needed to form authentication
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //the default route pattern is set here
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Companies}/{action=Index}/{id?}");
            });
        }
    }
}
