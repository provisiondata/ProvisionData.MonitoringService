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

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProvisionData.Internet;

namespace ProvisionData.MonitoringService.EntityFrameworkCore;

public static class ValueConverters
{
	public static readonly ValueConverter<DomainName, String> DomainNameToString
		= new(
			domain => domain.ToString(),
			value => new DomainName(value)
		);
}
