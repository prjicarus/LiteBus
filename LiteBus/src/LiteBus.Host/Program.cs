﻿using LiteBus.Domain;
using LiteBus.Domain.Providers;
using StructureMap;
using System;
using System.Reflection;
using System.ServiceProcess;

namespace LiteBus.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<DomainRegistry>();
            var initializer = container.GetInstance<IInitializeMessageService>();
            var qPathProvider = container.GetInstance<IQueuePathProvider>();
            var serializer = container.GetInstance<ISerializationProvider>();
            var messageHandler = new MessageService(initializer, qPathProvider, serializer, container);
            if (Environment.UserInteractive)
            {
                Console.Write("running as console.");
                var callingAssembly = Assembly.GetExecutingAssembly();
                Console.WriteLine("running from assembly {0}.", callingAssembly.GetName().Name);
                messageHandler.RunAsConsole(args);
            }
            else
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[] { messageHandler };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
