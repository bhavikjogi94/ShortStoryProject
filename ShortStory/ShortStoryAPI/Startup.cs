using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShortStoryBOL;

namespace ShortStoryAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc(x=>x.Filters.Add(new AuthorizeFilter())); for cookie based authentication

            services.AddCors();//This is for Cross Orginis : if try to access from Angular UI then  it will work
            //step 5: applying at application level

            var authPol = new AuthorizationPolicyBuilder()
                               .AddAuthenticationSchemes(new string[] { JwtBearerDefaults.AuthenticationScheme })
                               .RequireAuthenticatedUser()
                               .Build();
            services.AddMvc(x => x.Filters.Add(new AuthorizeFilter(authPol)));

            services.AddDbContext<SSDbContext>();
            services.AddTransient<IStoryDb, StoryDb>();
            //  services.AddScoped<IStoryDb, StoryDb>();
            //services.AddSingleton<IStoryDb, StoryDb>();
            //services.AddResponseCaching();
            services.AddIdentity<SSUser, IdentityRole>()
                .AddEntityFrameworkStores<SSDbContext>()
                .AddDefaultTokenProviders();


            //Step-1 : Create signing key from SecretKey
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));

            //step2 : Create Validation parameters using signinKey
            var tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            //step3 : set authenticaiton type as JwtBearer

            services.AddAuthentication(auth=>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //step4: set validaiton parameter created above
            .AddJwtBearer(jwt=>
            {
                jwt.TokenValidationParameters = tokenValidationParameters;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            //app.UseResponseCaching();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); //This is for Cross Orginis : if try to access from Angular UI then  it will work
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
