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

using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace ProvisionData.MonitoringService;

public class MonitoringServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
	private readonly ICurrentTenant _currentTenant;

	public MonitoringServiceDataSeedContributor(ICurrentTenant currentTenant)
	{
		_currentTenant = currentTenant;
	}

	public Task SeedAsync(DataSeedContext context)
	{
		/* Instead of returning the Task.CompletedTask, you can insert your test data
		 * at this point!
		 */

		using (_currentTenant.Change(context?.TenantId))
		{
			return Task.CompletedTask;
		}
	}
}
