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
using ProvisionData.Internet;
using ProvisionData.MonitoringService.Domains;
using ProvisionData.MonitoringService.Locations;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ProvisionData.MonitoringService.EntityFrameworkCore;

[ConnectionStringName(MonitoringServiceDbProperties.ConnectionStringName)]
public class MonitoringServiceDbContext : AbpDbContext<MonitoringServiceDbContext>, IMonitoringServiceDbContext
{
	public DbSet<Domain> Domains { get; set; }
	public DbSet<Location> Locations { get; set; }
	public DbSet<MailExchanger> MailExchangers { get; set; }
	public DbSet<Nameserver> Nameservers { get; set; }

	public DbSet<DomainLocation> DomainLocations { get; set; }
	public DbSet<DomainNameserver> DomainNameservers { get; set; }
	public DbSet<DomainMailExchanger> DomainMailExchangers { get; set; }

	public MonitoringServiceDbContext(DbContextOptions<MonitoringServiceDbContext> options)
		: base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ConfigureMonitoringService();
	}
}
