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

public record TTL : IComparable<TTL>, IComparable<Int32>
{
	private readonly Int32 _value;

	public TTL(Int32 value)
	{
		if (value < 0)
			throw new ArgumentOutOfRangeException(nameof(value));

		_value = value;
	}

	public static TTL Parse(String v)
	{
		return Int32.TryParse(v, out var result)
			? (TTL)result
			: throw new ArgumentException($"Don't know how to convert \"{v}\" into a TTL.");
	}

	public static implicit operator Int32(TTL ttl) => ttl._value;

	public static implicit operator TTL(Int32 n) => new(n);

	public static implicit operator String(TTL ttl) => ttl._value.ToString(CultureInfo.InvariantCulture);

	public static Boolean operator <=(TTL a, TTL b) => a._value <= b._value;
	public static Boolean operator >=(TTL a, TTL b) => a._value >= b._value;
	public static Boolean operator <(TTL a, TTL b) => a._value < b._value;
	public static Boolean operator >(TTL a, TTL b) => a._value > b._value;

	public Int32 CompareTo(TTL? other) => _value.CompareTo(other?._value);

	public Int32 CompareTo(Object? obj) => obj is TTL ttl ? CompareTo(ttl) : 1;

	public Int32 CompareTo(Int32 other) => _value.CompareTo(other);

	public override Int32 GetHashCode() => _value.GetHashCode();

	public override String ToString() => _value.ToString(CultureInfo.InvariantCulture);

	public String HumanReadable()
	{
		var weeks = Math.Floor(_value / 604800D);
		var days = Math.Floor((_value - weeks * 604800D) / 86400D);
		var hours = Math.Floor((_value - weeks * 604800D - days * 86400D) / 3600);
		var minutes = Math.Floor((_value - weeks * 604800D - days * 86400D - hours * 3600) / 60);
		var seconds = _value - weeks * 604800D - days * 86400D - hours * 3600 - minutes * 60;
		var time = "";
		if (weeks > 0)
			time += weeks + "w";

		if (days > 0)
			time += days + "d";

		if (hours > 0)
			time += hours + "h";

		if (minutes > 0)
			time += minutes + "m";

		if (seconds > 0)
			time += seconds + "s";

		return _value > 0 ? time : "0";
	}
}
