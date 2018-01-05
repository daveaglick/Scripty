using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Scripty
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Program program = new Program();
            AppDomain.CurrentDomain.UnhandledException += program.UnhandledExceptionEvent;
            return program.Run(args);
        }

        private readonly Settings _settings = new Settings();
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        private Program()
        {
            _loggerFactory = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((text, logLevel) => logLevel >= LogLevel.Information, true)
            });
            _logger = _loggerFactory.CreateLogger<Program>();
        }

        private async Task<int> Run(string[] args)
        {            
            // Parse the command line if there are args
            if (args.Length > 0)
            {
                try
                {
                    bool hasParseArgsErrors;
                    if (!_settings.ParseArgs(args, out hasParseArgsErrors))
                    {
                        return hasParseArgsErrors ? (int)ExitCode.CommandLineError : (int)ExitCode.Normal;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception while parsing command line arguments");
                    return (int)ExitCode.CommandLineError;
                }
            }
            else
            {
                // Otherwise the settings should come in over stdin
                _settings.ReadStdin();
            }

            // Attach
            if (_settings.Attach)
            {
                Console.WriteLine("Waiting for a debugger to attach (or press a key to continue)...");
                while (!Debugger.IsAttached && !Console.KeyAvailable)
                {
                    Thread.Sleep(100);
                }
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Console.WriteLine("Key pressed, continuing execution");
                }
                else
                {
                    Console.WriteLine("Debugger attached, continuing execution");
                }
            }
        }

        private void UnhandledExceptionEvent(object sender, UnhandledExceptionEventArgs e)
        {
            // Exit with a error exit code
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                _logger.LogCritical(exception, "Unhandled exception");
            }
            Environment.Exit((int)ExitCode.UnhandledError);
        }
    }
}
