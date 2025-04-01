using CodeTest.App.Models;

namespace CodeTest.App.Services;

public interface IPeopleParser : IDisposable
{
    IEnumerable<Person?> ParseNext();
}
