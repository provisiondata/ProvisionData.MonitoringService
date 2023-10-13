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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProvisionData.MonitoringService.Domains;

namespace ProvisionData.MonitoringService.EntityFrameworkCore.Configuration;

public class DomainMailExchangerConfiguration : IEntityTypeConfiguration<DomainMailExchanger>
{
	public void Configure(EntityTypeBuilder<DomainMailExchanger> builder)
	{
		builder.ToTable("DomainMailExchangers");
		builder.HasKey(e => new { e.DomainID, e.MailExchangerID });

		builder.Property(e => e.LastSeenUtc)
			   .HasColumnName("LastSeenUtc")
			   .HasColumnType("datetime2(7)")
			   .IsRequired();
	}
}
