using CliniqonFindFriendsAppDataAccess.IRepository;
using CliniqonFindFriendsAppDataAccess.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppBuisinessLogic
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
        {
            //Always use Transient for multi threading purpose
            services.AddTransient(typeof(ISecurityRepository), typeof(SecurityRepository));
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            return services;
        }
    }
}
