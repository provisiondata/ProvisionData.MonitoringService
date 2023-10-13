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

using System.Text.RegularExpressions;

namespace ProvisionData.Internet;

public partial record Serial(UInt32 Value)
{
	[GeneratedRegex("[^0-9]")]
	private static partial Regex Numeric();

	public static readonly Serial Epoch = From(1970, 1, 1, 0);

	public static Serial From(DateOnly utc)
		=> From(utc.Year, utc.Month, utc.Day);
	public static Serial From(DateTime utc)
		=> From(utc.Year, utc.Month, utc.Day);
	public static Serial From(DateTimeOffset utc)
		=> From(utc.Year, utc.Month, utc.Day);
	public static Serial From(Int32 year, Int32 month, Int32 day, Int32 sequence = 0)
		=> new(ToUInt32(year, month, day, sequence));
	public static Serial From(UInt32 n) => new(n);

	private static UInt32 ToUInt32(Int32 year, Int32 month, Int32 day, Int32 sequence)
		=> Convert.ToUInt32(year * 1000000u + month * 10000u + day * 100u + sequence);

	public static Boolean operator <=(Serial a, Serial b) => a.Value <= b.Value;
	public static Boolean operator >=(Serial a, Serial b) => a.Value >= b.Value;
	public static Boolean operator <(Serial a, Serial b) => a.Value < b.Value;
	public static Boolean operator >(Serial a, Serial b) => a.Value > b.Value;

	public static implicit operator Serial(UInt32 n) => new(n);
	public static implicit operator UInt32(Serial serial) => serial.Value;

	public static implicit operator String(Serial serial) => serial.ToString();

	public static Serial Parse(String value)
	{
		ArgumentNullException.ThrowIfNull(value);

		var normalized = Numeric().Replace(value, String.Empty);
		return String.IsNullOrWhiteSpace(normalized)
			? throw new ArgumentException($"'{value}' cannot be parsed as Serial", nameof(value))
			: new(UInt32.Parse(normalized, CultureInfo.InvariantCulture));
	}

	public Int32 CompareTo(Serial other) => Value.CompareTo(other.Value);

	public String ToFormattedString()
	{
		var year = Convert.ToInt32(Value / 1000000);
		var month = Convert.ToInt32((Value - (year * 1000000)) / 10000);
		var day = Convert.ToInt32((Value - (year * 1000000) - (month * 10000)) / 100);
		var sequence = Convert.ToInt32(Value - (year * 1000000) - (month * 10000) - (day * 100));

		return $"{year}-{month.ToString("00", CultureInfo.InvariantCulture)}-{day.ToString("00", CultureInfo.InvariantCulture)}-{sequence.ToString("00", CultureInfo.InvariantCulture)}";
	}

	public override String ToString() => Value.ToString(CultureInfo.InvariantCulture);
}
