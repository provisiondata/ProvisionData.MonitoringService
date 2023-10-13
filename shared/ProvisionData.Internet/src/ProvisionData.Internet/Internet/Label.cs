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

namespace ProvisionData.Internet;

public class Label : IEquatable<Label>, IComparable<Label>
{
	private readonly String _label;

	public Label(String label)
	{
		ThrowIfInvalid(label);

		_label = label;
	}

	public override Int32 GetHashCode()
		=> _label?.GetHashCode() ?? 0;

	public override String ToString() => _label;

	public Int32 CompareTo(Label? other)
		=> String.Compare(_label, other?._label, StringComparison.InvariantCultureIgnoreCase);

	public Boolean Equals(Label? other)
		=> ReferenceEquals(this, other) || _label.Equals(other?._label, StringComparison.InvariantCultureIgnoreCase);

	public override Boolean Equals(Object? other)
		=> other is not null && (ReferenceEquals(this, other) || other is Label domainName && Equals(domainName));

	public static implicit operator Label(String domainName) => new(domainName);

	public static Label FromString(String domainName) => new(domainName);

	public static implicit operator String(Label domainName) => domainName._label;

	public static String FromDomainName(Label domainName) => domainName._label;

	public static Boolean operator ==(Label a, Label b)
	{
		// If both are null, or both are same instance, return true.
		if (ReferenceEquals(a, b))

			return true;

		// If one is null, but not both, return false.

		if (a is null || b is null)
			return false;

		// Return true if the fields match:
		return a._label == b._label;
	}

	public static Boolean operator !=(Label a, Label b) => !(a == b);

	private static void ThrowIfInvalid(String label)
	{
		if (label == null)
			throw new ArgumentNullException(nameof(label));

		if (String.IsNullOrWhiteSpace(label))
			throw new ArgumentException("Invalid: Must not be Empty or Whitespace", nameof(label));

		if (label.Length > 63)
			throw new ArgumentException("Invalid Length: The length of any one label is limited to between 1 and 63 octets.", nameof(label));
	}
}
