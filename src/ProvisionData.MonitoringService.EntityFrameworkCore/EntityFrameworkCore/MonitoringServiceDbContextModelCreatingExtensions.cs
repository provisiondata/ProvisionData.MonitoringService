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

using Microsoft.EntityFrameworkCore;
using ProvisionData.MonitoringService.EntityFrameworkCore.Configuration;
using Volo.Abp;

namespace ProvisionData.MonitoringService.EntityFrameworkCore;

public static class MonitoringServiceDbContextModelCreatingExtensions
{
	public static void ConfigureMonitoringService(this ModelBuilder modelBuilder)
	{
		Check.NotNull(modelBuilder, nameof(modelBuilder));

		modelBuilder.ApplyConfiguration(new DomainConfiguration());
		modelBuilder.ApplyConfiguration(new DomainLocationConfiguration());
		modelBuilder.ApplyConfiguration(new DomainNameserverConfiguration());
		modelBuilder.ApplyConfiguration(new DomainMailExchangerConfiguration());

		modelBuilder.ApplyConfiguration(new LocationConfiguration());
		modelBuilder.ApplyConfiguration(new MailExchangerConfiguration());
		modelBuilder.ApplyConfiguration(new NameserverConfiguration());
	}
}
