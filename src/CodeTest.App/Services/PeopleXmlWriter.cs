using System.Xml;
using CodeTest.App.Models;

namespace CodeTest.App.Services;

public class PeopleXmlWriter : IWriter, IDisposable
{
    private readonly XmlWriter _xmlWriter;
    private bool _hasStartElement = false;

    public PeopleXmlWriter(XmlWriter xmlWriter)
    {
        _xmlWriter = xmlWriter;
    }

    public void Begin()
    {
        const string ROOT_ELEMENT = "people";
        _xmlWriter.WriteStartElement(ROOT_ELEMENT);

        _hasStartElement = true;
    }

    public void Dispose()
    {
        if (!_hasStartElement)
        {
            Begin();
        }

        End();
        _xmlWriter.Dispose();
    }

    public void End()
    {
        _xmlWriter.WriteEndElement();
        _xmlWriter.Flush();
    }

    public void WritePerson(Person person)
    {
        if (!_hasStartElement)
        {
            Begin();
        }

        const string PERON_ELEMENT = "person";
        _xmlWriter.WriteStartElement(PERON_ELEMENT);

        const string FIRST_NAME_ELEMENT = "firstname";
        _xmlWriter.WriteStartElement(FIRST_NAME_ELEMENT);
        _xmlWriter.WriteString(person.First);
        _xmlWriter.WriteEndElement();

        const string LAST_NAME_ELEMENT = "lastname";
        _xmlWriter.WriteStartElement(LAST_NAME_ELEMENT);
        _xmlWriter.WriteString(person.Last);
        _xmlWriter.WriteEndElement();

        if (person.PhoneNumbers != null)
        {
            WritePhoneNumbers(person.PhoneNumbers);
        }

        if (person.Address != null)
        {
            WriteAddress(person.Address);
        }

        WriteFamilyMembers(person.FamilyMembers);

        _xmlWriter.Flush();
    }

    private void WriteAddress(Address address)
    {
        const string ADDRESS_ELEMENT = "address";
        _xmlWriter.WriteStartElement(ADDRESS_ELEMENT);

        const string STREET_ELEMENT = "street";
        _xmlWriter.WriteStartElement(STREET_ELEMENT);
        _xmlWriter.WriteString(address.Street);
        _xmlWriter.WriteEndElement();

        const string CITY_ELEMENT = "city";
        _xmlWriter.WriteStartElement(CITY_ELEMENT);
        _xmlWriter.WriteString(address.City);
        _xmlWriter.WriteEndElement();

        if (address.ZipCode != null)
        {
            const string ZIP_CODE_ELEMENT = "zipcode";
            _xmlWriter.WriteStartElement(ZIP_CODE_ELEMENT);
            _xmlWriter.WriteString(address.ZipCode);
            _xmlWriter.WriteEndElement();
        }

        _xmlWriter.WriteEndElement();
    }

    private void WriteFamilyMembers(List<FamilyMember> familyMembers)
    {
        foreach (var familyMember in familyMembers)
        {
            const string FAMILY_MEMBER_ELEMENT = "family";
            _xmlWriter.WriteStartElement(FAMILY_MEMBER_ELEMENT);

            const string NAME_ELEMENT = "name";
            _xmlWriter.WriteStartElement(NAME_ELEMENT);
            _xmlWriter.WriteString(familyMember.Name);
            _xmlWriter.WriteEndElement();

            const string BIRTH_YEAR_ELEMENT = "born";
            _xmlWriter.WriteStartElement(BIRTH_YEAR_ELEMENT);
            _xmlWriter.WriteString(familyMember.BirthYear.ToString());
            _xmlWriter.WriteEndElement();

            if (familyMember.Address != null)
            {
                WriteAddress(familyMember.Address);
            }

            if (familyMember.PhoneNumbers != null)
            {
                WritePhoneNumbers(familyMember.PhoneNumbers);
            }

            _xmlWriter.WriteEndElement();
        }
    }

    private void WritePhoneNumbers(PhoneNumbers phoneNumbers)
    {
        const string PHONE_ELEMENT = "phone";
        _xmlWriter.WriteStartElement(PHONE_ELEMENT);

        const string MOBILE_ELEMENT = "mobil";
        _xmlWriter.WriteStartElement(MOBILE_ELEMENT);
        _xmlWriter.WriteString(phoneNumbers.Mobile);
        _xmlWriter.WriteEndElement();

        const string LAND_LINE_ELEMENT = "landline";
        _xmlWriter.WriteStartElement(LAND_LINE_ELEMENT);
        _xmlWriter.WriteString(phoneNumbers.Landline);
        _xmlWriter.WriteEndElement();

        _xmlWriter.WriteEndElement();
    }
}
