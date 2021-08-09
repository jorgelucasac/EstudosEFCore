using System;
using System.Threading.Tasks;
using Estudos.EFCore.MultiTenant.Data;
using Estudos.EFCore.MultiTenant.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Estudos.EFCore.MultiTenant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MultiTenant", Version = "v1" });
            });

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))
                    //habilitando detalhes de erros
                    .EnableDetailedErrors()
                    //habilitando visualização de dados sensiveis
                    .EnableSensitiveDataLogging()
                    //habilitando a exibição dos logs
                    .LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.AddScoped<ApplicationDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MultiTenant v1"));
            }

            InicarBancoDeDados(app);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InicarBancoDeDados(IApplicationBuilder app)
        {
            using var db = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (var i = 0; i < 5; i++)
            {
                db.Pessoas.Add(new Pessoa { Nome = $"Pessoa {i}" });
                db.Produtos.Add(new Produto { Descricao = $"Produto {i}" });
            }

            db.SaveChanges();
        }
    }
}
