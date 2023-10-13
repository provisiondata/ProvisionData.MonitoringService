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

using Shouldly;

namespace ProvisionData.Internet;

public sealed class SerialTests
{
	private static readonly DateTime ArbitraryDateTime = new(1974, 10, 16);

	[Fact]
	public void Must_be_creatable_from_UInt32()
	{
		var s = Serial.From(1900998877u);

		(1900998877u == s).ShouldBeTrue();
	}

	[Fact]
	public void Must_be_creatable_from_DateTime()
	{
		var s = Serial.From(ArbitraryDateTime);
		(1974101600 == s).ShouldBeTrue();
	}

	[Fact]
	public void Must_implement_comparison_operators_correctly()
	{
		var a = Serial.From(1900112200);
		var b = Serial.From(1900112200);

		(a == b).ShouldBeTrue();
		(b == a).ShouldBeTrue();

		(a != b).ShouldBeFalse();
		(b != a).ShouldBeFalse();

		(a >= b).ShouldBeTrue();
		(b >= a).ShouldBeTrue();

		(a <= b).ShouldBeTrue();
		(b <= a).ShouldBeTrue();

		(a > b).ShouldBeFalse();
		(b > a).ShouldBeFalse();

		(a < b).ShouldBeFalse();
		(b < a).ShouldBeFalse();

		b = Serial.From(1900112201);

		(a == b).ShouldBeFalse();
		(b == a).ShouldBeFalse();

		(a != b).ShouldBeTrue();
		(b != a).ShouldBeTrue();

		(a >= b).ShouldBeFalse();
		(b >= a).ShouldBeTrue();

		(a <= b).ShouldBeTrue();
		(b <= a).ShouldBeFalse();

		(a > b).ShouldBeFalse();
		(b > a).ShouldBeTrue();

		(a < b).ShouldBeTrue();
		(b < a).ShouldBeFalse();

		// Day
		var c = Serial.From(1970010100);
		var d = Serial.From(1970010200);

		(c == d).ShouldBeFalse();
		(d == c).ShouldBeFalse();

		(c >= d).ShouldBeFalse();
		(d >= c).ShouldBeTrue();

		(c <= d).ShouldBeTrue();
		(d <= c).ShouldBeFalse();

		(c > d).ShouldBeFalse();
		(d > c).ShouldBeTrue();

		(c < d).ShouldBeTrue();
		(d < c).ShouldBeFalse();

		// Month
		var e = Serial.From(1970010100);
		var f = Serial.From(1970020100);

		(e == f).ShouldBeFalse();
		(f == e).ShouldBeFalse();

		(e >= f).ShouldBeFalse();
		(f >= e).ShouldBeTrue();

		(e <= f).ShouldBeTrue();
		(f <= e).ShouldBeFalse();

		(e > f).ShouldBeFalse();
		(f > e).ShouldBeTrue();

		(e < f).ShouldBeTrue();
		(f < e).ShouldBeFalse();

		// Year
		var g = Serial.From(1970010100);
		var h = Serial.From(1971010100);

		(g == h).ShouldBeFalse();
		(h == g).ShouldBeFalse();

		(g >= h).ShouldBeFalse();
		(h >= g).ShouldBeTrue();

		(g <= h).ShouldBeTrue();
		(h <= g).ShouldBeFalse();

		(g > h).ShouldBeFalse();
		(h > g).ShouldBeTrue();

		(g < h).ShouldBeTrue();
		(h < g).ShouldBeFalse();

		// Complex
		var i = Serial.From(1970123199);
		var j = Serial.From(1971010100);

		(i == j).ShouldBeFalse();
		(j == i).ShouldBeFalse();

		(i >= j).ShouldBeFalse();
		(j >= i).ShouldBeTrue();

		(i <= j).ShouldBeTrue();
		(j <= i).ShouldBeFalse();

		(i > j).ShouldBeFalse();
		(j > i).ShouldBeTrue();

		(i < j).ShouldBeTrue();
		(j < i).ShouldBeFalse();
	}

	[Fact]
	public void Must_output_Human_Readable_strings()
	{
		var s = Serial.From(ArbitraryDateTime);
		s.ToFormattedString().ShouldBe("1974-10-16-00");
	}

	//[Fact]
	//public void Must_increment_when_current_Date_is_less_than_the_Serial_date()
	//{
	//	var s = Serial.From(1974101600u);

	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(1974101601u);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(1974101602u);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(1974101603u);
	//}

	//[Fact]
	//public void Must_increment_when_current_Date_is_equal_the_Serial_date()
	//{
	//	Clock.DateIs(1974, 10, 16);

	//	var s = Serial.From(1974101600u);

	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(1974101601u);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(1974101602u);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(1974101603u);
	//}

	//[Fact]
	//public void Must_increment_when_current_Date_is_greater_than_the_Serial_date()
	//{
	//	Clock.DateIs(2000, 01, 01);

	//	var s = Serial.From(1974101600u);

	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2000010100u);

	//	Clock.DateIs(2000, 01, 02);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2000010200u);

	//	Clock.DateIs(2000, 01, 03);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2000010300u);
	//}

	//[Fact]
	//public void Must_handle_overflow_of_the_sequence()
	//{
	//	Clock.DateIs(1974, 10, 16);

	//	var s = Serial.From(1974101699);

	//	s = s.Increment(ArbitraryDateTime);

	//	s.ShouldBe(1974101700);
	//}

	//[Fact]
	//public void Must_handle_ridiculous_overflow()
	//{
	//	Clock.DateIs(2000, 01, 01);

	//	var s = Serial.From(2018093000);

	//	// Make it overflow
	//	var last = s;
	//	for (var i = 0; i <= 10000; i++)
	//	{
	//		s = s.Increment(ArbitraryDateTime);

	//		(s > last).ShouldBeTrue();

	//		last = s;
	//	}
	//	s.ShouldBe(2019010801);

	//	// New date, still overflowing
	//	Clock.DateIs(2000, 01, 02);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2019010802);

	//	// No longer overflowing
	//	Clock.DateIs(2019, 12, 31);
	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2019123100);
	//}

	[Fact]
	public void Must_parse_strings()
	{
		var a = Serial.Parse("2000012345");
		(2000012345 == a).ShouldBeTrue();

		var b = Serial.Parse("2000-01-23-45");
		(2000012345 == b).ShouldBeTrue();

		var c = Serial.Parse("2013031602");
		(2013031602 == c).ShouldBeTrue();
	}

	[Fact]
	public void Must_not_allow_null_or_empty_strings()
	{
		Assert.Throws<ArgumentNullException>(() => Serial.Parse(null!));

		Assert.Throws<ArgumentException>(() => Serial.Parse(""));
	}

	//[Fact]
	//public void Must_roll_over_correctly_at_the_end_of_the_month()
	//{
	//	var s = Serial.From(2000013199);

	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2000020100);
	//}

	//[Fact]
	//public void Must_roll_over_correctly_at_end_the_end_of_the_year()
	//{
	//	Clock.DateIs(2000, 2, 3);

	//	var s = Serial.From(1999123100);

	//	s = s.Increment(ArbitraryDateTime);
	//	s.ShouldBe(2000020300);
	//}
}
