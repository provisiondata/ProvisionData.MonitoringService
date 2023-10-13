// PDSI Monitoring Service
// Copyright (C) 2023 Provision Data Systems Inc.
//
// This program is free software: you can redistribute it and/or modify it under the terms of
// the GNU Affero General Public License as published by the Free Software Foundation, either
// version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License along with this
// program. If not, see <https://www.gnu.org/licenses/>.

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProvisionData.Logging.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ProvisionData.Logging;

public static class ProvisionDataLoggingExtensions
{
	public static IHostBuilder UseMcpLogging([NotNull] this IHostBuilder hostBuilder)
	{
		hostBuilder.ConfigureAppConfiguration((context, builder) =>
		{
			var path = GetPath(context);
			builder.AddJsonFile(path, optional: true, reloadOnChange: true);
		});

		hostBuilder.ConfigureServices((context, services) =>
		{
			var configuration = context.Configuration;

			services.ConfigureLogging(configuration);
		});

		return hostBuilder;

		static String GetPath(HostBuilderContext context)
			=> ProvisionDataApplication.InContainer
			? $"{context.HostingEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}Config{Path.DirectorySeparatorChar}appsettings.Serilog.json"
			: $"{context.HostingEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}appsettings.Serilog.json";
	}

	public static WebAssemblyHostBuilder UseMcpLogging([NotNull] this WebAssemblyHostBuilder builder)
	{
		builder.Services.ConfigureLogging(builder.Configuration);
		return builder;
	}

	public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
	{
		var loggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
		services.AddSingleton(loggingLevelSwitch);

		var loggerConfiguration = new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.MinimumLevel.ControlledBy(loggingLevelSwitch)
			.Enrich.FromLogContext()
			.Enrich.WithApplicationInfo()
			.Enrich.WithMachineName()
			.Enrich.WithEnvironmentName()
			.Enrich.WithCorrelationId()
			.WriteToSeq(configuration, loggingLevelSwitch);

		Log.Logger = loggerConfiguration.CreateLogger();
		services.AddLogging(builder => builder.ClearProviders().AddSerilog());

		Log.Information("Starting {ApplicationName}. " + McpLoggingProperties.InstanceIdFieldName + ": {InstanceId}",
			ProvisionDataApplication.ApplicationName, ProvisionDataApplication.InstanceId);

		Log.Debug("Debug Logging is Enabled");
		Log.Verbose("Verbose Logging is Enabled");
		Log.Information("Containerized: {Containerized}", configuration.InContainer());

		return services;
	}

	private static LoggerConfiguration WriteToSeq(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, LoggingLevelSwitch loggingLevelSwitch)
	{
		var serverUrl = configuration["Seq:ServerUrl"]!;
		var apiKey = configuration["Seq:ApiKey"]!;

		if (!String.IsNullOrWhiteSpace(serverUrl) && !String.IsNullOrWhiteSpace(apiKey))
		{
			loggerConfiguration.WriteTo.Seq(serverUrl, apiKey: apiKey, controlLevelSwitch: loggingLevelSwitch);
		}

		return loggerConfiguration;
	}
}
