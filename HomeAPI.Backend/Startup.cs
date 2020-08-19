using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using HomeAPI.Backend.Common;
using HomeAPI.Backend.Data;
using HomeAPI.Backend.Data.Lighting;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers.Lighting;
using HomeAPI.Backend.Providers.Sensors;
using HomeAPI.Backend.Providers.Weather;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
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
			AddDbContext(services);

			services.AddTransient<IDateTimeProvider, DateTimeProvider>();

			services.AddScoped<IAsyncRepository<LightScene>, LightSceneRepository>();
			services.AddTransient<IHueLightProvider, HueLightProvider>();
			services.AddTransient<IHueLightStateUpdateFactory, HueLightStateUpdateFactory>();

			services.AddTransient<IHueSensorProvider, HueSensorProvider>();

			services.AddTransient<IOWMProvider, OWMProvider>();

			services.Configure<HueOptions>(Configuration.GetSection("HueOptions"));
			services.Configure<OWMOptions>(Configuration.GetSection("OWMOptions"));

			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddHttpClient();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			app.UseAuthentication();

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

		private void AddDbContext(IServiceCollection services)
		{
			string dbPath = Path.Combine(Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "HOME"), "HomeAPI_Data");
			if (!dbPath.EndsWith(Path.DirectorySeparatorChar) && !dbPath.EndsWith(Path.AltDirectorySeparatorChar))
			{
				dbPath += Path.DirectorySeparatorChar;
			}

			Console.WriteLine($"DB path: {dbPath}");

			if (!Directory.Exists(dbPath))
			{
				Directory.CreateDirectory(dbPath);
				Console.WriteLine("DB directory created");
			}

			string connectionString = string.Format(Configuration.GetConnectionString("Data"), dbPath);

			services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));
		}
	}
}
