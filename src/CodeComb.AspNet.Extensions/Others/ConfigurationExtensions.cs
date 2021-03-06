﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection self, out IConfiguration config, string fileName = "config")
        {
            var _services = self.BuildServiceProvider();
            var appEnv = _services.GetRequiredService<IApplicationEnvironment>();
            var env = _services.GetRequiredService<IHostingEnvironment>();

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(appEnv.ApplicationBasePath, $"{fileName}.json"))
                .AddJsonFile(Path.Combine(appEnv.ApplicationBasePath, $"{fileName}.{env.EnvironmentName}.json"), optional: true);
            var Configuration = builder.Build();
            self.AddSingleton<IConfiguration>(Configuration);
            config = Configuration;

            return self;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection self, string fileName = "config")
        {
            var _services = self.BuildServiceProvider();
            var appEnv = _services.GetRequiredService<IApplicationEnvironment>();
            var env = _services.GetRequiredService<IHostingEnvironment>();

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(appEnv.ApplicationBasePath, $"{fileName}.json"))
                .AddJsonFile(Path.Combine(appEnv.ApplicationBasePath, $"{fileName}.{env.EnvironmentName}.json"), optional: true);
            var Configuration = builder.Build();
            self.AddSingleton<IConfiguration>(Configuration);

            return self;
        }
    }
}
