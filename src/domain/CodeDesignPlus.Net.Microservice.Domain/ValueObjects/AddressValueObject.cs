namespace CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

public sealed partial class AddressValueObject
{
    public AddressValueObject(string country, string state, string city, string address, int codePostal)
    {
        this.Country = country;
        this.State = state;
        this.City = city;
        this.Address = address;
        this.CodePostal = codePostal;
    }

    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string Address { get; private set; }
    public int CodePostal { get; private set; }

    public static AddressValueObject Create(string country, string state, string city, string address, int codePostal)
    {
        DomainGuard.IsNullOrEmpty(country, Errors.CountryIsNull);
        DomainGuard.IsNullOrEmpty(state, Errors.StateIsNull);
        DomainGuard.IsNullOrEmpty(city, Errors.CityIsNull);
        DomainGuard.IsNullOrEmpty(address, Errors.AddressIsNull);
        DomainGuard.IsLessThan(codePostal, 0, Errors.CodePostalIsInvalid);

        return new AddressValueObject(country, state, city, address, codePostal);
    }
}
