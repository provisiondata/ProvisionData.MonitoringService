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

using Serilog.Core;
using Serilog.Events;

namespace ProvisionData.Logging.Logging;

public class ApplicationInfoEnricher : ILogEventEnricher
{
	private LogEventProperty _cachedAppName = default!;
	private LogEventProperty _cachedGroupName = default!;
	private LogEventProperty _cachedInstanceId = default!;

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(GetAppGroupProperty(propertyFactory));
		logEvent.AddPropertyIfAbsent(GetAppNameProperty(propertyFactory));
		logEvent.AddPropertyIfAbsent(GetInstanceIdProperty(propertyFactory));
	}

	private LogEventProperty GetAppGroupProperty(ILogEventPropertyFactory propertyFactory)
		=> _cachedGroupName ??= CreateGroupNameProperty(propertyFactory);

	private static LogEventProperty CreateGroupNameProperty(ILogEventPropertyFactory factory)
		=> factory.CreateProperty(McpLoggingProperties.ApplicationGroupFieldName, ProvisionDataApplication.ApplicationGroup);

	private LogEventProperty GetAppNameProperty(ILogEventPropertyFactory propertyFactory)
		=> _cachedAppName ??= CreateAppNameProperty(propertyFactory);

	private static LogEventProperty CreateAppNameProperty(ILogEventPropertyFactory factory)
		=> factory.CreateProperty(McpLoggingProperties.ApplicationNameFieldName, ProvisionDataApplication.ApplicationName);

	private LogEventProperty GetInstanceIdProperty(ILogEventPropertyFactory propertyFactory)
		=> _cachedInstanceId ??= CreateInstanceIdProperty(propertyFactory);

	private static LogEventProperty CreateInstanceIdProperty(ILogEventPropertyFactory factory)
		=> factory.CreateProperty(McpLoggingProperties.InstanceIdFieldName, ProvisionDataApplication.InstanceId);
}
