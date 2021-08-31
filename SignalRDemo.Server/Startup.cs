using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRDemo.Server.Helpers;
using SignalRDemo.Server.Hub;
using SignalRDemo.Server.Services;

namespace SignalRDemo.Server
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

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddSwaggerGen(c => c.SwaggerDefinition());

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton(typeof(ITokenService<>), typeof(TokenService<>));

            services.AddTransient(typeof(ICurrentUserService<>), typeof(CurrentUserService<>));
            services.AddHttpContextAccessor();

            services.AddSignalR()
                .AddHubOptions<Notification>(hubOptions =>
                {
                    hubOptions.EnableDetailedErrors = true;
                });

            services.AddSingleton<IUserIdProvider, BasedUserIdProvider>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // global cors policy
            app.UseCors(options => options
                .SetIsOriginAllowed((x) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());


            app.Use(async (context, next) =>
            {
                
                await next();
            });

            app.UseSwaggerUi();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<Notification>("/hubs/notifications");
            });

        }
    }
}
