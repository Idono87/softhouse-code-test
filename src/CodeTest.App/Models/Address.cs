namespace CodeTest.App.Models;

public record Address
{
    public required string Street { get; init; }
    public required string City { get; init; }
    public string? ZipCode { get; set; }
}
