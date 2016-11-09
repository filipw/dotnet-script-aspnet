using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Dotnet.Script.Core;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Script.AspNet
{
    public class Startup
    {
        public static string ScriptFullPath;
        private readonly ScriptingHost _scriptHost = new ScriptingHost();
        private readonly Assembly _scriptAssembly;

        public Startup()
        {
            var code = File.ReadAllText(ScriptFullPath);
            var sourceText = SourceText.From(code, Encoding.UTF8);
            var context = new ScriptContext(sourceText, Path.GetDirectoryName(ScriptFullPath), "Release", new List<string>(), ScriptFullPath);

            var logger = new ScriptLogger(Console.Out, false);
            var compiler = new AspNetScriptCompiler(logger);
            var runner = new ScriptRunner(compiler, logger);

            var result = runner.Execute<object, ScriptingHost>(context, _scriptHost).GetAwaiter().GetResult();

            var compilationContext = (compiler.Context as ScriptCompilationContext<object>);
            _scriptAssembly = compilationContext.Script.GetScriptAssembly(compilationContext.Loader);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(_scriptAssembly));
            manager.FeatureProviders.Add(new ScriptControllerFeatureProvider());
            services.AddSingleton(manager);

            var mvcBuilder = _scriptHost.ConfigureServices?.Invoke(services)?.AddControllersAsServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _scriptHost.ConfigureApp?.Invoke(app, env);
        }
    }
}