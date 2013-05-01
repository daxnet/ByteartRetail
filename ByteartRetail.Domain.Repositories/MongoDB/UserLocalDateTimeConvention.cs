using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Reflection;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    /// <summary>
    /// Represents the Bson serialization convention that serializes the <see cref="System.DateTime"/> value
    /// by using the local date/time kind.
    /// </summary>
    public class UseLocalDateTimeConvention : IMemberMapConvention
    {
        #region IMemberMapConvention Members
        /// <summary>
        /// Applies the specified member map convention.
        /// </summary>
        /// <param name="memberMap">The member map convention.</param>
        public void Apply(BsonMemberMap memberMap)
        {
            IBsonSerializationOptions options = null;
            switch (memberMap.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)memberMap.MemberInfo;
                    if (propertyInfo.PropertyType == typeof(DateTime) ||
                        propertyInfo.PropertyType == typeof(DateTime?))
                        options = new DateTimeSerializationOptions(DateTimeKind.Local);
                    break;
                case MemberTypes.Field:
                    FieldInfo fieldInfo = (FieldInfo)memberMap.MemberInfo;
                    if (fieldInfo.FieldType == typeof(DateTime) ||
                        fieldInfo.FieldType == typeof(DateTime?))
                        options = new DateTimeSerializationOptions(DateTimeKind.Local);
                    break;
                default:
                    break;
            }
            memberMap.SetSerializationOptions(options);
        }

        #endregion

        #region IConvention Members
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return this.GetType().Name; }
        }

        #endregion
    }
}
