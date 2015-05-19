using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;

namespace LibraryTest.Logging
{
    public static class LogManager
    {
        private static readonly ILog DefautLog;

        static LogManager()
        {
            //TODOVH remove after tests
            //Debugger.Break();

            //TODOVH Is DefaultLog valid here?

            //TODOVH Merge logger features with dev - branch
            //TODOVH Check if configuration not exists. Is log nothing?
            //TODOVH Replace configuration manager with Application manager
            string logConfigPath = ConfigurationManager.AppSettings["logConfigPath"];
            if (String.IsNullOrEmpty(logConfigPath))
            {
                logConfigPath = "log4net.config";
            }

            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Path.IsPathRooted(logConfigPath))
            {
                logConfigPath = Path.Combine(appPath, logConfigPath);
            }

            var configFile = new FileInfo(logConfigPath);
            XmlConfigurator.ConfigureAndWatch(configFile);
            DefautLog = Logger.GetRoot();
        }

        public static ILog Default
        {
            get { return DefautLog; }
        }

        public static ILog GetLogger(string name)
        {
            Console.WriteLine("GetLogger Assembly {0}", Assembly.GetCallingAssembly());
            return new Logger(Assembly.GetCallingAssembly(), name);
        }

        public static ILog GetLogger(Type type)
        {
            return new Logger(type);
        }

        #region Nested type: Logger

        private class Logger : ILog
        {
            private readonly ILogger log;
            private readonly Type thisType = typeof(Logger);

            private Logger()
            {
                Hierarchy h = (Hierarchy)log4net.LogManager.GetRepository();
                log = h.Root;
            }

            public Logger(Assembly assembly, string name)
            {
                Console.WriteLine("Assembly {0}", Assembly.GetCallingAssembly());
                log = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), name);
            }

            public Logger(Type type)
            {
                log = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), type);
            }

            static public ILog GetRoot()
            {
                return new Logger();
            }

            public bool IsInfoEnabled
            {
                get
                {
                    return log.IsEnabledFor(Level.Info);
                }
            }

            public bool IsDebugEnabled
            {
                get
                {
                    return log.IsEnabledFor(Level.Debug);
                }
            }

            public bool IsErrorEnabled
            {
                get
                {
                    return log.IsEnabledFor(Level.Error);
                }
            }

            public bool IsWarnEnabled
            {
                get
                {
                    return log.IsEnabledFor(Level.Warn);
                }
            }

            public void Info(object message)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Info, stringMessage, null);
            }

            public void Info(object message, Exception exception)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Info, stringMessage, exception);
            }

            public void InfoFormat(string format, params object[] args)
            {
                Log(thisType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }

            public void Debug(object message)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Debug, stringMessage, null);
            }

            public void Debug(object message, Exception exception)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Debug, stringMessage, exception);
            }

            public void DebugFormat(string format, params object[] args)
            {
                Log(thisType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }

            public void Warn(object message)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Warn, stringMessage, null);
            }

            public void Warn(object message, Exception exception)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Warn, stringMessage, exception);
            }

            public void WarnFormat(string format, params object[] args)
            {
                Log(thisType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }

            public void Error(object message)
            {
                var ex = message as Exception;
                if (ex != null)
                {
                    Log(thisType, Level.Error, ex.Message, ex);
                }
                else
                {
                    var stringMessage = GetMessage(message);
                    Log(thisType, Level.Error, stringMessage, null);
                }
            }

            public void Error(object message, Exception exception)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Error, stringMessage, exception);
            }

            public void ErrorFormat(string format, params object[] args)
            {
                Log(thisType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }

            public void Fatal(object message)
            {
                var ex = message as Exception;
                if (ex != null)
                {
                    Log(thisType, Level.Fatal, ex.Message, ex);
                }
                else
                {
                    var stringMessage = GetMessage(message);
                    Log(thisType, Level.Fatal, stringMessage, null);
                }
            }

            public void Fatal(object message, Exception exception)
            {
                var stringMessage = GetMessage(message);
                Log(thisType, Level.Fatal, stringMessage, exception);
            }

            public void FatalFormat(string format, params object[] args)
            {
                Log(thisType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }

            private static string GetMessage(object message)
            {
                var stringMessage = message as string ?? ObjectDumper.ToString(message);
                return stringMessage;
            }

            private void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
            {
                log.Log(callerStackBoundaryDeclaringType, level, message, exception);
            }
        }

        #endregion
    }
}
