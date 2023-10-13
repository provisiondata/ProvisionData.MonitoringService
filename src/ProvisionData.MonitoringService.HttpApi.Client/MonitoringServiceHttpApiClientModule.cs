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

using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ProvisionData.MonitoringService;

[DependsOn(typeof(MonitoringServiceApplicationContractsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class MonitoringServiceHttpApiClientModule : AbpModule
{
	[SuppressMessage("Style", "IDE0053:Use expression body for lambda expression", Justification = "<Pending>")]
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddHttpClientProxies(
			typeof(MonitoringServiceApplicationContractsModule).Assembly,
			MonitoringServiceRemoteServiceConsts.RemoteServiceName
		);

		Configure<AbpVirtualFileSystemOptions>(options =>
		{
			options.FileSets.AddEmbedded<MonitoringServiceHttpApiClientModule>();
		});

	}
}
