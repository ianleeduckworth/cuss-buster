using Autofac;
using Autofac.Extensions.DependencyInjection;
using CusBuster.Core.DataAccess;
using CussBuster.Controllers;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Helpers;
using CussBuster.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CussBuster
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
		public IContainer ApplicationContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();

			var builder = new ContainerBuilder();
			builder.Populate(services);
			RegisterDependencies(builder);

			this.ApplicationContainer = builder.Build();
			return new AutofacServiceProvider(this.ApplicationContainer);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

		private void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<DefaultController>().InstancePerLifetimeScope();
			builder.RegisterType<MainHelper>().As<IMainHelper>().InstancePerLifetimeScope();
			builder.RegisterType<WordLoader>().As<IWordLoader>().InstancePerLifetimeScope();
			builder.RegisterType<AuthChecker>().As<IAuthChecker>().InstancePerLifetimeScope();
			builder.RegisterType<AuditWriter>().As<IAuditWriter>().InstancePerLifetimeScope();

			builder.Register(x => new AppSettings
			{
				CharacterLimit = int.Parse(Configuration.GetSection("AppSettings")["CharacterLimit"])
			}).As<IAppSettings>().SingleInstance();

			builder.Register(c => new BadWordCache
			{
				Words = c.Resolve<IWordLoader>().Load()
			}).As<IBadWordCache>().SingleInstance();

			builder.RegisterType<CussBusterContext>().InstancePerLifetimeScope();
		}
	}
}
