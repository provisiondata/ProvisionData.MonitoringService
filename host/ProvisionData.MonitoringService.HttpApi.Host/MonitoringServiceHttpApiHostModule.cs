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

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using ProvisionData.MonitoringService.EntityFrameworkCore;
using ProvisionData.MultiTenancy;
using Rebus.Config;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.EventBus.Rebus;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.RabbitMQ;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.VirtualFileSystem;

namespace ProvisionData.MonitoringService;

[DependsOn(typeof(MonitoringServiceApplicationModule))]
[DependsOn(typeof(MonitoringServiceEntityFrameworkCoreModule))]
[DependsOn(typeof(MonitoringServiceHttpApiModule))]
[DependsOn(typeof(AbpAspNetCoreMvcUiMultiTenancyModule))]
[DependsOn(typeof(AbpAspNetCoreSerilogModule))]
[DependsOn(typeof(AbpAuditLoggingEntityFrameworkCoreModule))]
[DependsOn(typeof(AbpAutofacModule))]
[DependsOn(typeof(AbpCachingStackExchangeRedisModule))]
[DependsOn(typeof(AbpEntityFrameworkCorePostgreSqlModule))]
[DependsOn(typeof(AbpEventBusRebusModule))]
[DependsOn(typeof(AbpPermissionManagementEntityFrameworkCoreModule))]
[DependsOn(typeof(AbpRabbitMqModule))]
[DependsOn(typeof(AbpSettingManagementEntityFrameworkCoreModule))]
[DependsOn(typeof(AbpSwashbuckleModule))]
[DependsOn(typeof(AbpTenantManagementEntityFrameworkCoreModule))]
public class MonitoringServiceHttpApiHostModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		PreConfigure((Action<AbpRebusEventBusOptions>)(options =>
		{
			var section = configuration.GetConfiguration<RebusConfiguration>();

			options.InputQueueName = MonitoringServiceConsts.InputQueueName;
			options.Configurer = rebusConfigurer =>
			{
				var rabbitMqConnectionString = configuration[""];

				rebusConfigurer.Transport(t => t.UseRabbitMq(section.RabbitMQConnectionString, MonitoringServiceConsts.InputQueueName));

				var dbConnectionString = configuration.GetNamedOrDefaultConnectionString(MonitoringServiceConsts.ServiceBusDatabase);
				rebusConfigurer.Subscriptions(s => s.StoreInPostgres(dbConnectionString, MonitoringServiceConsts.ServiceBusSubscriptionsTable));
			};
		}));
	}

	[SuppressMessage("Style", "IDE0053:Use expression body for lambda expression", Justification = "<Pending>")]
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		Configure<AbpRabbitMqOptions>(options =>
		{
			options.Connections.Default.UserName = "user";
			options.Connections.Default.Password = "pass";
			options.Connections.Default.HostName = configuration.InContainer() ? "rabbitmq" : "localhost";
			options.Connections.Default.Port = 5672;
		});

		Configure<AbpDbContextOptions>(options =>
		{
			options.UseNpgsql();
		});

		Configure<AbpMultiTenancyOptions>(options =>
		{
			options.IsEnabled = MultiTenancyConsts.IsEnabled;
		});

		if (hostingEnvironment.IsDevelopment())
		{
			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.ReplaceEmbeddedByPhysical<MonitoringServiceDomainSharedModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						String.Format("..{0}..{0}src{0}ProvisionData.MonitoringService.Domain.Shared", Path.DirectorySeparatorChar)));
				options.FileSets.ReplaceEmbeddedByPhysical<MonitoringServiceDomainModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						String.Format("..{0}..{0}src{0}ProvisionData.MonitoringService.Domain", Path.DirectorySeparatorChar)));
				options.FileSets.ReplaceEmbeddedByPhysical<MonitoringServiceApplicationContractsModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						String.Format("..{0}..{0}src{0}ProvisionData.MonitoringService.Application.Contracts", Path.DirectorySeparatorChar)));
				options.FileSets.ReplaceEmbeddedByPhysical<MonitoringServiceApplicationModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						String.Format("..{0}..{0}src{0}ProvisionData.MonitoringService.Application", Path.DirectorySeparatorChar)));
			});
		}

		context.Services.AddAbpSwaggerGenWithOAuth(
			configuration["AuthServer:Authority"]!,
			new Dictionary<String, String>
			{
				{"MonitoringService", "MonitoringService API"}
			},
			options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "MonitoringService API", Version = "v1" });
				options.DocInclusionPredicate((docName, description) => true);
				options.CustomSchemaIds(type => type.FullName);
			});

		Configure<AbpLocalizationOptions>(options =>
		{
			options.Languages.Add(new LanguageInfo("en", "en", "English"));
			options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
			options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
			options.Languages.Add(new LanguageInfo("es", "es", "Español"));
		});

		context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = configuration["AuthServer:Authority"];
				options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
				options.Audience = "MonitoringService";
			});

		Configure<AbpDistributedCacheOptions>(options =>
		{
			options.KeyPrefix = "MonitoringService:";
		});

		var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("MonitoringService");
		if (!hostingEnvironment.IsDevelopment())
		{
			var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
			dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "MonitoringService-Protection-Keys");
		}

		context.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(builder =>
			{
				builder
					.WithOrigins(
						configuration["App:CorsOrigins"]?
							.Split(",", StringSplitOptions.RemoveEmptyEntries)
							.Select(o => o.RemovePostFix("/"))
							.ToArray() ?? Array.Empty<String>()
					)
					.WithAbpExposedHeaders()
					.SetIsOriginAllowedToAllowWildcardSubdomains()
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
			});
		});
	}

	public override void OnApplicationInitialization(ApplicationInitializationContext context)
	{
		var app = context.GetApplicationBuilder();
		var env = context.GetEnvironment();

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseCorrelationId();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseCors();
		app.UseAuthentication();

		if (MultiTenancyConsts.IsEnabled)
		{
			app.UseMultiTenancy();
		}

		app.UseAbpRequestLocalization();
		app.UseAuthorization();
		app.UseSwagger();
		app.UseAbpSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");

			var configuration = context.GetConfiguration();
			options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
			options.OAuthScopes("MonitoringService");
		});
		app.UseAuditing();
		app.UseAbpSerilogEnrichers();
		app.UseConfiguredEndpoints();
	}
}

public class RebusConfiguration
{
	private static readonly String DefaultRabbitMQConnectionString = "amqp://mcptest:password@localhost:5672/mcptest";
	private static readonly String DefaultQueueName = "mcp";
	private static readonly String DefaultPostgreSQLConnectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=password;Timeout=10;CommandTimeout=20;Application Name=MCP;";
	private static readonly String DefaultSchemaName = "sagas";
	private static readonly String DefaultSagasTableName = "Sagas";
	private static readonly String DefaultSagasIndexTableName = "SagaIndex";
	private static readonly String DefaultTimeoutsDataTableName = "Timeouts";

	public RebusConfiguration()
	{
		// ToDo: This would be so much easier if we had Math.Clamp()
		MaxParallelism = Environment.ProcessorCount < 4 ? 4 : Environment.ProcessorCount < 16 ? 16 : Environment.ProcessorCount;
		NumberOfWorkers = Environment.ProcessorCount < 4 ? 4 : Environment.ProcessorCount < 16 ? 16 : Environment.ProcessorCount;
	}

	public String RabbitMQConnectionString { get; set; } = DefaultRabbitMQConnectionString;

	public String QueueName { get; set; } = DefaultQueueName;

	public String PostgreSQLConnectionString { get; set; } = DefaultPostgreSQLConnectionString;

	public String SchemaName { get; set; } = DefaultSchemaName;

	public String SagasTableName { get; set; } = DefaultSagasTableName;

	public String SagasIndexTableName { get; set; } = DefaultSagasIndexTableName;

	public String TimeoutsTableName { get; set; } = DefaultTimeoutsDataTableName;

	public Boolean AutoCreateTables { get; set; } = true;

	public Int32 MaxParallelism { get; set; }

	public Int32 NumberOfWorkers { get; set; }
}
