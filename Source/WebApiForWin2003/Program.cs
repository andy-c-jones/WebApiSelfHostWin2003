using System;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Topshelf;

namespace WebApiForWin2003
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            ConfigureLogging();
            AppDomain.CurrentDomain.UnhandledException +=
                (s, a) => Log.Error("Unhandled Exception", a.ExceptionObject as Exception);

            Log.Info("[Moogle] Logging enabled");

            HostFactory.Run(hc =>
            {
                hc.Service<MoogleService>(sc =>
                {
                    sc.ConstructUsing(MoogleService.Create);
                    sc.WhenStarted(s => s.Start());
                    sc.WhenStopped(s => s.Stop());
                });

                hc.RunAsLocalSystem();

                hc.SetDescription("A simple, lightweight and flexible Moogle Service");
                hc.SetServiceName("Moogle.Service");
                hc.SetDisplayName("Moogle Service");
            });
        }

        private static void ConfigureLogging()
        {
            XmlConfigurator.Configure();
            if (!Environment.UserInteractive) return;
            if (LogManager.GetRepository().GetAppenders().OfType<ColoredConsoleAppender>().Any()) return;

            var root = ((Hierarchy)LogManager.GetRepository()).Root as IAppenderAttachable;
            if (root == null) return;
            var appender = new ColoredConsoleAppender
            {
                Name = "ConsoleAppender",
                Layout = new PatternLayout("%date{dd/MM/yyyy HH:mm:ss,ffffff} [%thread] %level - %message%newline"),
                Threshold = Level.Info
            };
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Error,
                ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Warn,
                ForeColor = ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Info,
                ForeColor = ColoredConsoleAppender.Colors.White
            });
            appender.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Debug,
                ForeColor = ColoredConsoleAppender.Colors.Blue | ColoredConsoleAppender.Colors.HighIntensity
            });
            appender.ActivateOptions();
            root.AddAppender(appender);
        }
    }
}
