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

public class TTLs
{
	[Fact]
	public void Must_allow_zero()
	{
		Int32 i = new TTL(0);
		i.ShouldBe(0);
	}

	[Fact]
	public void Must_be_constructable_from_Int32()
	{
		var a = new TTL(3600);
		(a == 3600).ShouldBeTrue();
	}

	[Fact]
	public void Must_be_assignable_from_Int32()
	{
		TTL a = 1200;
		(a == 1200).ShouldBeTrue();
	}

	[Fact]
	public void Must_be_assignable_to_Int32()
	{
		Int32 i = new TTL(1200);
		i.ShouldBe(1200);
	}

	[Fact]
	public void Must_be_assignable_to_String()
	{
		String s = new TTL(1200);
		s.ShouldBe("1200");
	}

	[Fact]
	public void Must_be_Equatable()
	{
		var a = new TTL(1200);
		var b = new TTL(1200);
		var c = new TTL(3600);

		a.Equals(a).ShouldBeTrue();
		a.Equals(b).ShouldBeTrue();
		b.Equals(a).ShouldBeTrue();
		a.Equals(c).ShouldBeFalse();
		c.Equals(a).ShouldBeFalse();
	}

	[Fact]
	public void Must_be_Comparable()
	{
		var a = new TTL(1200);
		var b = new TTL(2400);
		var c = new TTL(3600);

		b.CompareTo(a).ShouldBe(1);
		b.CompareTo(b).ShouldBe(0);
		b.CompareTo(c).ShouldBe(-1);
	}

	[Fact]
	public void Must_implement_comparison_operators()
	{
		var a = new TTL(1200);
		var b = new TTL(1200);

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

		b = new TTL(2400);

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
	}

	[Fact]
	public void Must_not_allow_negative_values()
	{
		var a = Record.Exception(() => new TTL(-1));
		a.ShouldBeOfType<ArgumentOutOfRangeException>();

		var b = Record.Exception(() => { TTL a = -1; });
		b.ShouldBeOfType<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void Must_implement_ToString()
	{
		new TTL(0).ToString().ShouldBe("0");
		new TTL(1).ToString().ShouldBe("1");
		new TTL(60).ToString().ShouldBe("60");
		new TTL(3600).ToString().ShouldBe("3600");
		new TTL(86400).ToString().ShouldBe("86400");
		new TTL(604800).ToString().ShouldBe("604800");
		new TTL(694861).ToString().ShouldBe("694861");
	}

	[Fact]
	public void Must_output_Human_Readable_strings()
	{
		new TTL(0).HumanReadable().ShouldBe("0");
		new TTL(1).HumanReadable().ShouldBe("1s");
		new TTL(60).HumanReadable().ShouldBe("1m");
		new TTL(3600).HumanReadable().ShouldBe("1h");
		new TTL(86400).HumanReadable().ShouldBe("1d");
		new TTL(604800).HumanReadable().ShouldBe("1w");
		new TTL(694861).HumanReadable().ShouldBe("1w1d1h1m1s");
	}
}
