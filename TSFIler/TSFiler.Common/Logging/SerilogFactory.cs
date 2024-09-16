﻿using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Reflection;

namespace TSFiler.Common.Logging;

public static class SerilogFactory
{
    public static SerilogLoggerFactory InitLogging()
    {
        var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var logPath = Path.Combine(executingDir, "logs", "verbose.log");
        var logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.File(logPath,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}")
            .WriteTo.Console(LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message}{NewLine}{Exception}")
            .CreateLogger();
        Log.Logger = logger;
        return new SerilogLoggerFactory(logger);
    }
}
