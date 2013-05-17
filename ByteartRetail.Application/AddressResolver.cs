using AutoMapper;
using ByteartRetail.DataObjects;
using ByteartRetail.Domain.Model;

namespace ByteartRetail.Application
{
    public class AddressResolver:ValueResolver<AddressDataObject, Address>
    {
        protected override Address ResolveCore(AddressDataObject source)
        {
            return new Address
            {
                City = source.City,
                Country = source.Country,
                State = source.State,
                Street = source.Street,
                Zip = source.Zip
            };
        }
    }
}
