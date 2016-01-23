using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.Config
{
    public class AutofacWebAPI
    {
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config,
            RegisterServices(new ContainerBuilder()));
        }
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver =
            new AutofacWebApiDependencyResolver(container);
        }
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // EF DbContext
            builder.RegisterType<EntitiesContext>()
                   .As<DbContext>()
                   .InstancePerApiRequest();

            // Register repositories by using Autofac's OpenGenerics feature
            // More info: http://code.google.com/p/autofac/wiki/OpenGenerics
            builder.RegisterGeneric(typeof(EntityRepository<>))
                   .As(typeof(IEntityRepository<>))
                   .InstancePerApiRequest();

            // Services
            builder.RegisterType<CryptoService>()
            .As<ICryptoService>()
            .InstancePerApiRequest();
            builder.RegisterType<MembershipService>()
            .As<IMembershipService>()
            .InstancePerApiRequest();
            //builder.RegisterType<ShipmentService>()
            //.As<IShipmentService>()
            //.InstancePerApiRequest();
            return builder.Build();
        }
    }
}
