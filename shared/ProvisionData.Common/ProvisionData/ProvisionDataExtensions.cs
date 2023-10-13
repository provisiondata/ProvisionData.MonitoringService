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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ProvisionData;

public static class ProvisionDataExtensions
{
	private const String InContainerEnvironmentVariableName = "DOTNET_RUNNING_IN_CONTAINER";

	/// <summary>
	/// Get a value that indicates if the current AppDomain is in a comtainer.
	/// </summary>
	/// <returns><see langword="true"/> if the environment is a container, otherwise <see langword="false"/>.</returns>
	public static Boolean InContainer()
		=> Environment.GetEnvironmentVariable(InContainerEnvironmentVariableName) is String value
		&& Boolean.TryParse(value, out var result)
		&& result;

	/// <summary>
	/// Checks if the current host environment name is a container.
	/// </summary>
	/// <param name="configuration">An instance of <see cref="IConfiguration"/>.</param>
	/// <returns><see langword="true"/> if the environment is a container, otherwise <see langword="false"/>.</returns>
	public static Boolean InContainer(this IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration);

		return InContainer();
	}

	/// <summary>
	/// Checks if the current host environment name is a container.
	/// </summary>
	/// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
	/// <returns><see langword="true"/> if the environment is a container, otherwise <see langword="false"/>.</returns>
	public static Boolean IsContainer(this IHostEnvironment hostEnvironment)
	{
		ArgumentNullException.ThrowIfNull(hostEnvironment);

		return InContainer();
	}
}
