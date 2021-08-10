using System.Linq;
using System.Text.Json.Serialization;
using Estudos.EFCore.RepositoryUoW.Data;
using Estudos.EFCore.RepositoryUoW.Data.Repositories;
using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Estudos.EFCore.RepositoryUoW
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

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Estudos.EFCore.RepositoryUoW", Version = "v1" });
            });

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estudos.EFCore.RepositoryUoW v1"));
            }

            InicializarBaseDeDados(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InicializarBaseDeDados(IApplicationBuilder app)
        {
            using var db = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ApplicationContext>();

            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(Enumerable.Range(1, 10)
                    .Select(p => new Departamento
                    {
                        Descricao = $"Departamento - {p}",
                        Colaboradores = Enumerable.Range(1, 10)
                            .Select(x => new Colaborador
                            {
                                Nome = $"Colaborador: {x}/{p}"
                            }).ToList()
                    }));

                db.SaveChanges();
            }
        }
    }
}
