using ByteartRetail.DataObjects;
using ByteartRetail.Infrastructure.Communication;
using ByteartRetail.ServiceContracts;
using System;
using System.Collections.Specialized;
using System.Web.Security;

namespace ByteartRetail.Web
{
    public class ByteartRetailMembershipProvider : MembershipProvider
    {
        private string applicationName;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval = false;
        private bool requiresQuestionAndAnswer = false;
        private bool requiresUniqueEmail = true;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private int minRequiredPasswordLength;
        private int minRequiredNonalphanumericCharacters;
        private string passwordStrengthRegularExpression;
        private MembershipPasswordFormat passwordFormat = MembershipPasswordFormat.Clear;

        private ByteartRetailMembershipUser ConvertFrom(UserDataObject userObj)
        {
            if (userObj == null)
                return null;

            ByteartRetailMembershipUser user = new ByteartRetailMembershipUser("ByteartRetailMembershipProvider",
                userObj.UserName, 
                userObj.ID, 
                userObj.Email, 
                "", 
                "", 
                true, 
                userObj.IsDisabled ?? true,
                userObj.DateRegistered ?? DateTime.MinValue, 
                userObj.DateLastLogon ?? DateTime.MinValue, 
                DateTime.MinValue,
                DateTime.MinValue, 
                DateTime.MinValue,
                userObj.Contact,
                userObj.PhoneNumber,
                userObj.ContactAddress,
                userObj.DeliveryAddress);

            return user;
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "ByteartRetailMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Membership Provider for ByteartRetail");
            }

            base.Initialize(name, config);

            applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredNonalphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public ByteartRetailMembershipUser CreateUser(string username, 
            string password, 
            string email, 
            string passwordQuestion, 
            string passwordAnswer, 
            bool isApproved, 
            object providerUserKey, 
            string contact, 
            string phoneNumber, 
            AddressDataObject contactAddress, 
            AddressDataObject deliveryAddress, 
            out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            ByteartRetailMembershipUser user = GetUser(username, true) as ByteartRetailMembershipUser;
            if (user == null)
            {
                using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
                {
                    UserDataObjectList userDataObjects = new UserDataObjectList
                    {
                        new UserDataObject
                        {
                            UserName = username,
                            Password = password,
                            Contact = contact,
                            DateLastLogon = null,
                            DateRegistered = DateTime.Now,
                            Email = email,
                            IsDisabled = false,
                            PhoneNumber = phoneNumber,
                            ContactAddress = contactAddress,
                            DeliveryAddress = deliveryAddress
                        }
                    };
                    proxy.Channel.CreateUsers(userDataObjects);
                }
                status = MembershipCreateStatus.Success;
                return GetUser(username, true) as ByteartRetailMembershipUser;
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }
        }

        public override MembershipUser CreateUser(string username, 
            string password, 
            string email, 
            string passwordQuestion, 
            string passwordAnswer, 
            bool isApproved, 
            object providerUserKey, 
            out MembershipCreateStatus status)
        {
            return CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, null, null, null, null, out status);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                try
                {
                    var userDataObject = proxy.Channel.GetUserByName(username, QuerySpec.Empty);
                    proxy.Channel.DeleteUsers(new UserDataObjectList { userDataObject });
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public override bool EnablePasswordReset
        {
            get { return this.enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return this.enablePasswordRetrieval; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection col = new MembershipUserCollection();
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var dataObject = proxy.Channel.GetUserByEmail(emailToMatch, QuerySpec.Empty);
                totalRecords = 1;
                col.Add(this.ConvertFrom(dataObject));
                return col;
            }
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection col = new MembershipUserCollection();
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var dataObject = proxy.Channel.GetUserByName(usernameToMatch, QuerySpec.Empty);
                totalRecords = 1;
                col.Add(this.ConvertFrom(dataObject));
                return col;
            }
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection col = new MembershipUserCollection();
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var dataObjects = proxy.Channel.GetUsers(QuerySpec.Empty);
                if (dataObjects != null)
                {
                    totalRecords = dataObjects.Count;
                    foreach (var dataObject in dataObjects)
                        col.Add(this.ConvertFrom(dataObject));
                }
                else
                    totalRecords = 0;
                return col;
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var dataObject = proxy.Channel.GetUserByName(username, QuerySpec.Empty);
                if (dataObject == null)
                    return null;
                return ConvertFrom(dataObject);
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var dataObject = proxy.Channel.GetUserByKey((Guid)providerUserKey, QuerySpec.Empty);
                if (dataObject == null)
                    return null;
                return ConvertFrom(dataObject);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var dataObject = proxy.Channel.GetUserByEmail(email, QuerySpec.Empty);
                if (dataObject == null)
                    return null;
                return dataObject.UserName;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return this.maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return this.minRequiredNonalphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return this.minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return this.passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return this.passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return this.passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return this.requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return this.requiresUniqueEmail; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return proxy.Channel.ValidateUser(username, password);
            }
        }
    }
}