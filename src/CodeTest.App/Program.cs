using System.Xml;
using CodeTest.App.Exceptions;
using CodeTest.App.Services;

if (args.Count() != 2)
{
    Console.WriteLine("Invalid input. Please pass and input file and an output file");
    return;
}

var inputFile = Path.GetFullPath(args[0]);
var outputFile = Path.GetFullPath(args[1]);

var streamReader = new StreamReader(inputFile);
var streamWriter = new StreamWriter(outputFile);
var textParser = new PeopleTextParser(streamReader);
var xmlWriter = XmlWriter.Create(streamWriter);
var personXmlWriter = new PeopleXmlWriter(xmlWriter);

// Converter disposes of stream when leaving try closure
using var converter = new PersonConverter(textParser, personXmlWriter);

try
{
    converter.Convert();
}
catch (Exception ex)
{
    Console.WriteLine(ex.GetType().Name);
    Console.WriteLine(ex.Message);
    Console.WriteLine("Conversion is partially complete");

    return;
}

Console.WriteLine("Conversion has been completed");
