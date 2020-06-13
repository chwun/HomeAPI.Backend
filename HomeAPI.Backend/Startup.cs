using System.Diagnostics.CodeAnalysis;
using HomeAPI.Backend.Data;
using HomeAPI.Backend.Data.Lighting;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeAPI.Backend
{
	[ExcludeFromCodeCoverage]
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
			services.AddDbContext<DataContext>(options => options.UseSqlite(Configuration.GetConnectionString("Data")));

			services.AddScoped<IAsyncRepository<LightScene>, LightSceneRepository>();
			services.AddTransient<IHueProvider, HueProvider>();
			services.AddTransient<IHueLightStateUpdateFactory, HueLightStateUpdateFactory>();

			services.Configure<HueOptions>(Configuration.GetSection("HueOptions"));

			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddHttpClient();
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
		}
	}
}
