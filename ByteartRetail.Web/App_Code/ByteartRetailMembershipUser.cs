using ByteartRetail.DataObjects;
using System;
using System.Web.Security;

namespace ByteartRetail.Web
{
    /// <summary>
    /// 表示用于Byteart Retail系统的MembershipUser类型。
    /// </summary>
    public class ByteartRetailMembershipUser : MembershipUser
    {
        #region Ctor
        public ByteartRetailMembershipUser(string providerName,
            string name,
            object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockoutDate,
            string contact,
            string phoneNumber,
            string contactAddress_Country,
            string contactAddress_State,
            string contactAddress_City,
            string contactAddress_Street,
            string contactAddress_Zip,
            string deliveryAddress_Country,
            string deliveryAddress_State,
            string deliveryAddress_City,
            string deliveryAddress_Street,
            string deliveryAddress_Zip)
            : base(providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
        {
            this.Contact = contact;
            this.PhoneNumber = phoneNumber;
            this.ContactAddress_City = contactAddress_City;
            this.ContactAddress_Country = contactAddress_Country;
            this.ContactAddress_State = contactAddress_State;
            this.ContactAddress_Street = contactAddress_Street;
            this.ContactAddress_Zip = contactAddress_Zip;
            this.DeliveryAddress_City = deliveryAddress_City;
            this.DeliveryAddress_Country = deliveryAddress_Country;
            this.DeliveryAddress_State = deliveryAddress_State;
            this.DeliveryAddress_Street = deliveryAddress_Street;
            this.DeliveryAddress_Zip = deliveryAddress_Zip;
        }

        public ByteartRetailMembershipUser(string providerName,
            string name,
            object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockoutDate,
            string contact,
            string phoneNumber,
            AddressDataObject contactAddress,
            AddressDataObject deliveryAddress)
            : this(providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate, contact, phoneNumber,
            contactAddress == null ? null : contactAddress.Country,
            contactAddress == null ? null : contactAddress.State,
            contactAddress == null ? null : contactAddress.City,
            contactAddress == null ? null : contactAddress.Street,
            contactAddress == null ? null : contactAddress.Zip,
            deliveryAddress == null ? null : deliveryAddress.Country,
            deliveryAddress == null ? null : deliveryAddress.State,
            deliveryAddress == null ? null : deliveryAddress.City,
            deliveryAddress == null ? null : deliveryAddress.Street,
            deliveryAddress == null ? null : deliveryAddress.Zip) { }
        #endregion

        #region Public Properties
        public string Contact { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactAddress_Country { get; set; }

        public string ContactAddress_State { get; set; }

        public string ContactAddress_City { get; set; }

        public string ContactAddress_Street { get; set; }

        public string ContactAddress_Zip { get; set; }

        public string DeliveryAddress_Country { get; set; }

        public string DeliveryAddress_State { get; set; }

        public string DeliveryAddress_City { get; set; }

        public string DeliveryAddress_Street { get; set; }

        public string DeliveryAddress_Zip { get; set; }
        #endregion

    }
}