﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi
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
            JwtService jwtService;

            services.AddSingleton<IConfiguration>(Configuration);

            jwtService = new JwtService(Configuration);
            services.Add(new ServiceDescriptor(typeof(IJwtService), jwtService));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => jwtService.RulesTokenValidation(options));

            DbContextOptions<UserContext> dbOptions;
            DbContextOptionsBuilder<UserContext> dboBuilder = new DbContextOptionsBuilder<UserContext>();
            dboBuilder.UseInMemoryDatabase("WebapiTest");
            dbOptions = dboBuilder.Options;

            services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("WebapiTest"));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminTest", policy => policy.RequireClaim("SuperTester", "true"));
                options.AddPolicy("BlacklistingJwt", policy => policy.Requirements.Add(new BlacklistingJwtMiddleware(new UserContext(dbOptions), Configuration)));
            });

            services.AddSingleton<IAuthorizationHandler, BlacklistingJwtMiddlewareHandler>();


            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    var resolver = opt.SerializerSettings.ContractResolver;
                    if (resolver != null)
                    {
                        var res = resolver as DefaultContractResolver;
                        res.NamingStrategy = new SnakeCaseNamingStrategy();  // <<!-- this removes the camelcasing
                    }
                }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseValidateAndRenewToken();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
