namespace CodeTest.App.Services;

public class PersonConverter(IPeopleParser parser, IWriter writer) : IConverter, IDisposable
{
    private readonly IPeopleParser _parser = parser;
    private readonly IWriter _writer = writer;

    public void Convert()
    {
        foreach (var person in _parser.ParseNext())
        {
            if (person is null)
            {
                return;
            }

            _writer.WritePerson(person);
        }
    }

    public void Dispose()
    {
        _parser.Dispose();
        _writer.Dispose();
    }
}
