using AutoMapper;
using ByteartRetail.DataObjects;
using ByteartRetail.Domain.Model;

namespace ByteartRetail.Application
{
    public class InversedAddressResolver : ValueResolver<Address, AddressDataObject>
    {
        protected override AddressDataObject ResolveCore(Address source)
        {
            return new AddressDataObject
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
