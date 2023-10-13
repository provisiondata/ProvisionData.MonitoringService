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

using System.Reflection;

namespace ProvisionData;

public static class ProvisionDataApplication
{
	private const String Undefined = "Undefined";

	public static String ApplicationGroup { get; } = GetApplicationGroup();
	public static String ApplicationName { get; } = GetApplicationName();
	public static String EnvironmentName { get; } = GetEnvironmentName();
	public static Guid InstanceId { get; } = Guid.NewGuid();
	public static Boolean InContainer { get; } = ProvisionDataExtensions.InContainer();

	public static String GetApplicationGroup()
	{
		var name = Assembly.GetEntryAssembly()?.GetName().Name
			?? Undefined;

		var index = name.IndexOf('.');

		return index > 0 ? name[..index] : name;
	}

	public static String GetApplicationName()
		=> Assembly.GetEntryAssembly()?.GetName().Name
		?? Undefined;

	public static String GetEnvironmentName()
		=> Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
		?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
		?? "Production";
}
