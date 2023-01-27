using Desafio.Application.Mapping;

namespace Desafio.Api.Extensions
{
    public static class StartupServices
    {
        public static void InjectDependencies(this IServiceCollection services)
        {
            #region Application
            services.AddScoped<Application.Service.IClienteService, Application.Service.ClienteService>();
            #endregion

            #region Persistence
            services.AddScoped<Persistence.IBaseRepository, Persistence.BaseRepository>();
            services.AddScoped<Persistence.Contract.IClientePersist, Persistence.Contract.ClientePersist>();
            #endregion
        }

        public static void AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DesafioProfile));
        }
    }
}
