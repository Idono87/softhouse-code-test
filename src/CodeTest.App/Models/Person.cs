namespace CodeTest.App.Models;

public record Person
{
    public required string First { get; init; }
    public required string? Last { get; init; }

    public Address? Address { get; set; }
    public PhoneNumbers? PhoneNumbers { get; set; }

    public List<FamilyMember> FamilyMembers { get; } = [];
}
