using CodeTest.App.Exceptions;
using CodeTest.App.Models;

namespace CodeTest.App.Services;

public class PeopleTextParser : IPeopleParser, IDisposable
{
    private readonly StreamReader _streamReader;
    private object? _current = null;

    public PeopleTextParser(StreamReader streamReader)
    {
        _streamReader = streamReader;
    }

    public void Dispose()
    {
        _streamReader.Dispose();
    }

    public IEnumerable<Person?> ParseNext()
    {
        ParseLine();
        while (_current != null)
        {
            yield return BuildPerson();
        }
    }

    private object? ParseLine()
    {
        var line = _streamReader.ReadLine();

        if (line == null)
        {
            _current = null;
            return null;
        }

        var type = line[0];

        _current = type switch
        {
            'P' => ParsePerson(line),
            'T' => ParsePhoneNumbers(line),
            'A' => ParseAddress(line),
            'F' => ParseFamilyMember(line),
            _ => throw new ParseException("Invalid data line type"),
        };

        return _current;
    }

    private Address ParseAddress(string line)
    {
        var splitLine = line.Split('|').ToArray();

        if (splitLine.Length != 3 && splitLine.Length != 4)
        {
            throw new ParseException("Expected address to have format 'A|<Street>|<City>|<?Zip Cold>'");
        }

        return new Address
        {
            Street = splitLine[1],
            City = splitLine[2],
            ZipCode = splitLine.Length == 4 ? splitLine[3] : null,
        };
    }

    private FamilyMember ParseFamilyMember(string line)
    {
        var splitLine = line.Split('|').ToArray();

        if (splitLine.Length != 3)
        {
            throw new ParseException("Expected family member to have format 'F|<Name>|<BirthYear>'");
        }

        var isValidBirthYear = int.TryParse(splitLine[2], out var birthYear);

        if (!isValidBirthYear)
        {
            throw new ParseException("Invalid birth year. Expected numeric value");
        }

        return new FamilyMember { Name = splitLine[1], BirthYear = birthYear };
    }

    private Person ParsePerson(string line)
    {
        var splitLine = line.Split('|').ToArray();

        if (splitLine.Length != 3)
        {
            throw new ParseException("Expected person to have the format 'P|<FirstName>|<LastName>'");
        }

        return new Person { First = splitLine[1], Last = splitLine[2] };
    }

    private PhoneNumbers ParsePhoneNumbers(string line)
    {
        var splitLine = line.Split('|').ToArray();

        if (splitLine.Length != 3)
        {
            throw new ParseException("Expected phone numbers to have the format 'T|<MobileNumber>|<LandLineNumber>'");
        }

        return new PhoneNumbers { Mobile = splitLine[1], Landline = splitLine[2] };
    }

    private Person BuildPerson()
    {
        if (_current is Person person)
        {
            BuildPersonChildren(person);
            return person;
        }

        throw new ParseException(
            $"Invalid format exception. Expected person type instead got '{_current?.GetType().Name}'"
        );
    }

    private void BuildPersonChildren(Person person)
    {
        for (var obj = ParseLine(); obj != null; obj = ParseLine())
        {
            switch (obj)
            {
                case Address address:
                    AddAddress(person, address);
                    break;
                case PhoneNumbers phoneNumbers:
                    AddPhone(person, phoneNumbers);
                    break;
                case FamilyMember familyMember:
                    person.FamilyMembers.Add(familyMember);
                    break;
                default:
                    return;
            }
        }
    }

    private void AddAddress(Person person, Address address)
    {
        if (person.FamilyMembers.Count() > 0)
        {
            AddFamilyMemberAddress(person.FamilyMembers.Last(), address);
            return;
        }

        if (person.Address != null)
        {
            throw new ValidationException($"Duplicate address on person '{person.First} {person.Last}'");
        }

        person.Address = address;
    }

    private void AddPhone(Person person, PhoneNumbers phoneNumbers)
    {
        if (person.FamilyMembers.Count() > 0)
        {
            AddFamilyMemberPhoneNumbers(person.FamilyMembers.Last(), phoneNumbers);
            return;
        }

        if (person.PhoneNumbers != null)
        {
            throw new ValidationException($"Duplicate phone numbers on person '{person.First} {person.Last}'");
        }

        person.PhoneNumbers = phoneNumbers;
    }

    private void AddFamilyMemberAddress(FamilyMember familyMember, Address address)
    {
        if (familyMember.Address != null)
        {
            throw new ValidationException($"Duplicate address on family member '{familyMember.Name}'");
        }

        familyMember.Address = address;
    }

    private void AddFamilyMemberPhoneNumbers(FamilyMember familyMember, PhoneNumbers phoneNumbers)
    {
        if (familyMember.PhoneNumbers != null)
        {
            throw new ValidationException($"Duplicate phone numbers on family member '{familyMember.Name}'");
        }

        familyMember.PhoneNumbers = phoneNumbers;
    }
}
