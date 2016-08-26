using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Logging
{
    class NLogConfigurator
    {
        /// <summary>
        /// Configures all NLog logging targets and rules.
        /// </summary>
        public static void ConfigureAll()
        {
            try {
                InternalLogger.LogLevel = LogLevel.Trace;
                InternalLogger.LogWriter = new StreamWriter(Android.OS.Environment.ExternalStorageDirectory + "/internallog.txt");
                LoggingConfiguration config = new LoggingConfiguration();
                config = ConfigureConsole(config);
                config = ConfigureDebugger(config);
                LogManager.Configuration = config;
                Debug.Write("Logger linked!");
            }
            catch (Exception e)
            {
                Debug.Write(e.StackTrace);
                Debug.Write(e.InnerException);
                Debug.Flush();
                throw e;
            }
}

        /// <summary>
        /// Configures the console logging target and rules. Console in this case means the device's own console; use logcat.
        /// </summary>
        /// <param name="config">The config to alter.</param>
        public static LoggingConfiguration ConfigureConsole(LoggingConfiguration config)
        {
            #region TargetSettings
            ConsoleTarget consoleTarget = new ConsoleTarget();

            consoleTarget.Name = "Console";
            // Example lines:
            // 11:03:39.532|DEBUG|                SomeLoggerName: Here is some logger output, bladieblah.
            // 08:48:01.000| WARN|ALoggerNameThatIsWayTooLongToD: Another logger output from a logger with a very long name.
            consoleTarget.Layout = @"${date:format=HH\:mm\:ss.fff}|${pad:padding=5:inner=${level:uppercase=true}}|${pad:padding=30:fixedLength=True:inner=${logger}}: ${message}";
            #endregion

            #region Rules
            LoggingRule rule1 = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            #endregion

            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(rule1);

            return config;
        }

        /// <summary>
        /// Configure the debugger logging target and rules. The debugger target outputs to Visual Studio's Output window.
        /// </summary>
        /// <param name="config">The config to alter.</param>
        public static LoggingConfiguration ConfigureDebugger(LoggingConfiguration config)
        {
            #region TargetSettings
            DebuggerTarget debuggerTarget = new DebuggerTarget();

            debuggerTarget.Name = "Debugger";
            // Example lines:
            // 11:03:39.532|DEBUG|                SomeLoggerName: Here is some logger output, bladieblah.
            // 08:48:01.000| WARN|ALoggerNameThatIsWayTooLongToD: Another logger output from a logger with a very long name.
            debuggerTarget.Layout = @"${date:format=HH\:mm\:ss.fff}|${pad:padding=5:inner=${level:uppercase=true}}|${pad:padding=30:fixedLength=True:inner=${logger}}: ${message}";
            #endregion

            #region Rules
            LoggingRule rule1 = new LoggingRule("*", LogLevel.Trace, debuggerTarget);
            #endregion

            config.AddTarget("debugger", debuggerTarget);
            config.LoggingRules.Add(rule1);

            return config;
        }
    }
}
