using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.CommandLineUtils;

namespace Dotnet.Script.AspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            var file = app.Argument("aspnet-script", "Path to CSX script");

            app.OnExecute(() =>
            {

                string workingDirectory;

                if (Path.IsPathRooted(file.Value))
                {
                    workingDirectory = Path.GetDirectoryName(file.Value);
                    Startup.ScriptFullPath = file.Value;
                }
                else
                {
                    var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), file.Value);
                    workingDirectory = Path.GetDirectoryName(absolutePath);
                    Startup.ScriptFullPath = absolutePath;
                }

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(workingDirectory)
                    .UseStartup<Startup>()
                    .Build();

                host.Run();
                return 0;
            });

            app.Execute(args);
        }
    }
}
