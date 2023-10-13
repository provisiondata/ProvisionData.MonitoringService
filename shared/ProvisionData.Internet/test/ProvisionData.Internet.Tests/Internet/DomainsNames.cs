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

public class DomainNames
{
	private const String LongDomain = "abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.abcdefghijklmnopqrstuvwxyz.com";

	[Fact]
	public void Must_return_the_domain_when_ToString_is_called()
	{
		new DomainName("www.example.com").ToString().ShouldBe("www.example.com");
	}

	[Fact]
	public void Must_be_comparable()
	{
		DomainName a = "example.com";
		DomainName b = "example.com";
		DomainName c = "apple.com";
		DomainName d = "orange.com";

		a.CompareTo(b).ShouldBe(0);
		b.CompareTo(a).ShouldBe(0);

		b.CompareTo(c).ShouldBe(1);
		b.CompareTo(d).ShouldBe(-1);
	}

	[Fact]
	public void Must_be_equatable()
	{
		DomainName a = "example.com";
		DomainName b = "example.com";
		DomainName c = "apple.com";
		DomainName d = "orange.com";

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
	public void Must_be_equatable_to_Empty()
	{
		var a = new DomainName("example.com");
		a.Equals(DomainName.Empty).ShouldBe(false);
	}

	[Fact]
	public void Must_be_equatable_to_String()
	{
		var a = new DomainName("example.com");
		a.Equals("example.com").ShouldBe(true);
	}

	[Fact]
	public void Must_be_assignable_to_String()
	{
		String s = new DomainName("example.com");
		s.ShouldBe("example.com");
	}

	[Fact]
	public void Must_be_assignable_from_String()
	{
		DomainName a = "example.com";
		a.ShouldBe(new DomainName("example.com"));
	}

	[Fact]
	public void Must_be_comparable_to_String()
		=> new DomainName("example.com").CompareTo("example.com").ShouldBe(0);

	[Fact]
	public void Must_throw_ArgumentException_when_domain_exceeds_255_characters()
		=> Assert.Throws<ArgumentException>(() => new DomainName(LongDomain));

	[Fact]
	public void Must_throw_ArgumentException_when_any_label_exceeds_63_characters()
	{
		// First label is too long, 2nd Level Domain
		Assert.Throws<ArgumentException>(() => new DomainName("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz.com"));
		// First label is too long, 3rd Level Domain
		Assert.Throws<ArgumentException>(() => new DomainName("www.abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz.com"));
		// Middle label is too long, 3rd Level Domain
		Assert.Throws<ArgumentException>(() => new DomainName("www.abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz.com"));
		// Last Label is too long, 2nd Level Domain
		Assert.Throws<ArgumentException>(() => new DomainName("example.abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz"));
		// Last Label is too long, 3rd Level Domain
		Assert.Throws<ArgumentException>(() => new DomainName("www.example.abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz"));
	}

	[Fact]
	public void Must_throw_ArgumentException_when_domain_starts_with_a_period()
		=> Assert.Throws<ArgumentException>(() => new DomainName(".example.com"));

	[Fact]
	public void Must_remove_trailing_period_when_domain_ends_with_a_period()
		=> new DomainName("example.com.").ShouldBe(new DomainName("example.com"));

	[Fact]
	public void Must_throw_ArgumentException_when_domain_contains_two_consecutive_periods()
		=> Assert.Throws<ArgumentException>(() => new DomainName("example..com"));

	[Fact]
	public void Must_throw_ArgumentException_when_domain_is_empty()
		=> Assert.Throws<ArgumentException>(() => new DomainName(""));

	[Fact]
	public void Must_throw_ArgumentException_when_domain_is_whitespace()
		=> Assert.Throws<ArgumentException>(() => new DomainName("   "));

	[Fact]
	public void Must_throw_ArgumentException_when_domain_is_a_single_label()
		=> Assert.Throws<ArgumentException>(() => new DomainName("example"));

	[Fact]
	public void Must_throw_ArgumentNullException_when_domain_null()
		=> Assert.Throws<ArgumentNullException>(() => new DomainName(null!));

	[Fact]
	[SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
	public void Must_accept_IPv4_or_IPv6_addresses()
	{
		DomainName a = "192.168.1.1";
		DomainName b = "2001:0db8:0000:0000:0000:ff00:0042:8329";
		DomainName c = "2001:db8:0:0:0:ff00:42:8329";
		DomainName d = "2001:db8::ff00:42:8329";
	}
}
