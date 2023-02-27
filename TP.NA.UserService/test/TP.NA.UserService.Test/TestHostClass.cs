using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TP.NA.UserService.Application.Abstractions.Repositories;

namespace TP.NA.UserService.Test
{
    public class TestHostClass : WebApplicationFactory<Program>
    {
        private readonly IUserRepository _userRepository;

        public TestHostClass(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(c => _userRepository);
            });

            return base.CreateHost(builder);
        }
    }
}