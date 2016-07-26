using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FileConvertService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.UseNLog();

                serviceConfig.Service<ConverterService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new ConverterService());

                    serviceInstance.WhenStarted(execute => execute.Start());

                    serviceInstance.WhenStopped(execute => execute.Stop());

                    serviceInstance.WhenPaused(execute => execute.Pause());

                    serviceInstance.WhenContinued(execute => execute.Continue());

                    serviceInstance.WhenCustomCommandReceived((execute, hostControl, commandNumber) => execute.CustomCommand(commandNumber));
                });

                serviceConfig.EnableServiceRecovery(recoveryOption =>
                {
                    recoveryOption.RestartService(1);
                    recoveryOption.RestartService(4);
                    recoveryOption.RunProgram(5, @"d:\someprogram.exe");
                });

                serviceConfig.EnablePauseAndContinue();

                serviceConfig.SetServiceName("AwesomeFileConverter");
                serviceConfig.SetDisplayName("Awesome File Converter");
                serviceConfig.SetDescription("A demo service");

                serviceConfig.StartAutomatically();
            });
        }
    }
}
