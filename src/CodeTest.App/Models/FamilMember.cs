namespace CodeTest.App.Models;

public record FamilyMember
{
    public required string Name { get; init; }
    public required int BirthYear { get; init; }
    public Address? Address { get; set; }
    public PhoneNumbers? PhoneNumbers { get; set; }
}
