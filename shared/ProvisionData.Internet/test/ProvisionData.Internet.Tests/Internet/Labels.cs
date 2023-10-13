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

public class Labels
{
	[Fact]
	public void Must_be_comparable()
	{
		Label a = "example.com.";
		Label b = "example.com.";
		Label c = "apple.com.";
		Label d = "orange.com.";

		a.CompareTo(b).ShouldBe(0);
		b.CompareTo(a).ShouldBe(0);

		b.CompareTo(c).ShouldBe(1);
		b.CompareTo(d).ShouldBe(-1);
	}

	[Fact]
	public void Must_be_equatable()
	{
		Label a = "example.com.";
		Label b = "example.com.";
		Label c = "apple.com.";
		Label d = "orange.com.";

		(a == b).ShouldBeTrue();
		(b == a).ShouldBeTrue();
		a.Equals(b).ShouldBeTrue();
		b.Equals(a).ShouldBeTrue();

		(c != d).ShouldBeTrue();
		(d != c).ShouldBeTrue();
		c.Equals(d).ShouldBeFalse();
		d.Equals(c).ShouldBeFalse();
	}

	[Fact]
	public void Must_be_assignable_to_String()
	{
		String s = new Label("example.com.");
		s.ShouldBe("example.com.");
	}

	[Fact]
	public void Must_be_assignable_from_String()
	{
		Label a = "example.com.";
		("example.com." == a).ShouldBeTrue();
	}

	[Fact]
	public void Must_be_comparable_to_String()
	{
		var d = new Label("example.com.");
		d.CompareTo("example.com.").ShouldBe(0);
	}

	[Fact]
	public void Must_throw_ArgumentException_when_domain_is_empty_or_whitespace()
	{
		Assert.Throws<ArgumentException>(() => new Label(""));

		Assert.Throws<ArgumentException>(() => new Label("   "));
	}

	[Fact]
	public void Must_throws_ArgumentNullException_when_domain_null()
	{
		Assert.Throws<ArgumentNullException>(() => new Label(null!));
	}
}
