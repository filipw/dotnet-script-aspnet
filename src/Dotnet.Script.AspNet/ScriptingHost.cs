using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Script.AspNet
{
    public class ScriptingHost
    {
        internal Func<IServiceCollection, IMvcBuilder> ConfigureServices;
        internal Action<IApplicationBuilder, IHostingEnvironment> ConfigureApp;

        public void Services(Func<IServiceCollection, IMvcBuilder> configureServices)
        {
            ConfigureServices = configureServices;
        }

        public void Configure(Action<IApplicationBuilder, IHostingEnvironment> configureApp)
        {
            ConfigureApp = configureApp;
        }
    }
}