using CodeTest.App.Models;

namespace CodeTest.App.Services;

public interface IWriter : IDisposable
{
    void WritePerson(Person person);
}
