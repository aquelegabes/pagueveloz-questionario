using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PagueVeloz.Data.Context;
using PagueVeloz.Data.Repository;
using PagueVeloz.Data.UoW;
using PagueVeloz.Domain.Interfaces;
using PagueVeloz.Domain.Mapper;
using PagueVeloz.Domain.Models;

namespace PagueVeloz.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void RegisterServices(IServiceCollection services)
        {
            // context
            services.AddDbContext<PagueVelozContext>(opts =>
                opts.UseSqlite(Configuration["ConnectionString:Local"]));

            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // repos
            services.AddScoped<IRepository<Empresa>, Repository<Empresa>>();
            services.AddScoped<IRepository<Pessoa>, Repository<Pessoa>>();
            services.AddScoped<IRepository<Fone>, Repository<Fone>>();
            services.AddScoped<IRepository<Fornecedor>, Repository<Fornecedor>>();

            // mapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            RegisterServices(services);

            services.AddCors(c =>
            {
                c.AddPolicy("AllowHeader", opts => opts.AllowAnyHeader());
                c.AddPolicy("AllowOrigin", opts => opts.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(opts => 
            {
                opts.AllowAnyHeader();
                opts.AllowAnyHeader();
            });
        }
    }
}
