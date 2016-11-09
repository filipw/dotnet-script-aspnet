using System.Collections.Generic;
using System.Reflection;
using Dotnet.Script.Core;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace Dotnet.Script.AspNet
{
    public class AspNetScriptCompiler : ScriptCompiler
    {
        private IEnumerable<string> _namespaces = new[]
        {
            "System",
            "System.IO",
            "System.Linq",
            "System.Collections.Generic",
            "Microsoft.Extensions.DependencyInjection",
            "Microsoft.AspNetCore.Builder",
            "Microsoft.AspNetCore.Mvc"
        };

        public AspNetScriptCompiler(ScriptLogger logger) : base(logger)
        {
        }

        public override ScriptOptions CreateScriptOptions(ScriptContext context)
        {
            var opts = base.CreateScriptOptions(context).AddImports(_namespaces);

            var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            var inheritedAssemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);

            foreach (var inheritedAssemblyName in inheritedAssemblyNames)
            {
                var assembly = Assembly.Load(inheritedAssemblyName);
                opts = opts.AddReferences(assembly);
            }

            return opts;
        }

        public override ScriptCompilationContext<TReturn> CreateCompilationContext<TReturn, THost>(ScriptContext context)
        {
            var result = base.CreateCompilationContext<TReturn, THost>(context);
            Context = result;
            return result;
        }

        public object Context { get; private set; }
    }
}