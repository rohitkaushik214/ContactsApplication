using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Reflection;
using ContactsApplication.Repository;
using Microsoft.Owin.Security.OAuth;
using ContactsApplication.Infrastructure;
using log4net;

[assembly: OwinStartup(typeof(ContactsApplication.Startup))]
namespace ContactsApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = GlobalConfiguration.Configuration;
            ConfigureDependencies(configuration, app);
            ConfigureOauth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }

        private void ConfigureOauth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureDependencies(HttpConfiguration configuration, IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(ContactsRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            //Registering the log4net module.
            builder.RegisterModule(new LogInjectionModule());
            log4net.Config.XmlConfigurator.Configure();

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacLifetimeScopeInjector(container);
            //app.UseAutofacWebApi(configuration);
        }
    }
}