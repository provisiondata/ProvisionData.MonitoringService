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

using ProvisionData.Internet;
using Volo.Abp.Domain.Entities;

namespace ProvisionData.MonitoringService.Domains;

public partial class Domain : AggregateRoot<Guid>
{
	public Guid ID { get; set; }

	public DomainName HostName { get; set; } = DomainName.Empty;

	public DateTimeOffset CreatedUtc { get; set; }

	public ICollection<DomainLocation> Locations { get; set; } = new HashSet<DomainLocation>();

	public ICollection<DomainMailExchanger> MailExchangers { get; set; } = new HashSet<DomainMailExchanger>();

	public ICollection<DomainNameserver> Nameservers { get; set; } = new HashSet<DomainNameserver>();

	public override String ToString() => HostName;
}
