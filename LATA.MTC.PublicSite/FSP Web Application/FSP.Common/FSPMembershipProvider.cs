using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Security;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.Data;
using System.Diagnostics;
using FSP.Domain.Model;


namespace FSP.Common
{
    public class FSPMembershipProvider : System.Web.Security.MembershipProvider
    {
        private int newPasswordLength = 8;
        private string connectionString;
        private MachineKeySection machineKey;

        #region properties
        private bool pWriteExceptionsToEventLog;
        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }
        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }
        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }
        private int pMinRequiredNonAlphanumericCharacters;
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }
        private int pMinRequiredPasswordLength;
        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }
        private string pPasswordStrengthRegularExpression;
        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //
            // Initialize OdbcConnection.
            //

            ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;


            // Get encryption and decryption key information from the configuration.
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");
            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");


        }
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="lastName"></param>
        /// <param name="nothing"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username,
                 string password,
                 string email,
                 string passwordQuestion,
                 string passwordAnswer,
                 bool isApproved,
                 object providerUserKey,
                 out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(email, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }



            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(email, false);

            if (u == null)
            {
                if (providerUserKey == null)
                {
                    providerUserKey = Guid.NewGuid();
                }
                else
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                try
                {

                    using (FSPDataContext db = new FSPDataContext())
                    {
                        User user = new User();
                        user.Email = email;
                        user.Password = this.EncodePassword(password);
                        user.DateCreated = DateTime.Now;
                        user.IsApproved = isApproved;
                        db.Users.InsertOnSubmit(user);
                        db.SubmitChanges();

                        status = MembershipCreateStatus.Success;

                    }

                }
                catch
                {
                    status = MembershipCreateStatus.ProviderError;
                }

                return GetUser(email, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }


            return null;
        }


        public MembershipUser CreateFSPUser(Guid userId, String email, String password, Guid roleId, bool isApproved, out MembershipCreateStatus status,
            String firstName = "", String lastName = "", String address = "", String city = "", String state = "", String zip = "", String phoneNumber = "")
        {
            ValidatePasswordEventArgs args =
             new ValidatePasswordEventArgs(email, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }



            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(email, false);

            if (u == null)
            {
                if (userId == null)
                {
                    userId = Guid.NewGuid();
                }

                try
                {

                    using (FSPDataContext db = new FSPDataContext())
                    {
                        if (roleId == Guid.Empty)
                            roleId = db.Roles.Where(p => p.RoleName == "Viewer").FirstOrDefault().RoleID;

                        User user = new User();
                        user.RoleID = roleId;
                        user.UserID = userId;
                        user.Email = email;
                        user.FirstName = firstName;
                        user.LastName = lastName;
                        user.Password = this.EncodePassword(password);
                        user.DateCreated = DateTime.Now;
                        user.IsApproved = isApproved;
                        user.Address = address;
                        user.City = city;
                        user.State = state;
                        user.Zip = zip;
                        user.PhoneNumber = phoneNumber;
                        user.WantsNotification = true;

                        db.Users.InsertOnSubmit(user);
                        db.SubmitChanges();


                        status = MembershipCreateStatus.Success;

                    }

                }
                catch
                {
                    status = MembershipCreateStatus.ProviderError;
                }

                return GetUser(email, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }


            return null;
        }

        /// <summary>
        /// Get Current User Context
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            MembershipUser u = null;

            try
            {
                using (FSPDataContext db = new FSPDataContext())
                {
                    var users = from q in db.Users
                                where q.Email.Equals(email) && q.IsApproved == true
                                select q;


                    if (users.Count() > 0)
                    {
                        User user = users.FirstOrDefault();

                        u = GetUserFromReader(user);

                        if (userIsOnline)
                        {
                            user.LastActivityDate = DateTime.Today;
                            db.SubmitChanges();
                        }
                    }

                }

            }
            catch { }


            return u;
        }

        /// <summary>
        /// Membership User Context
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private MembershipUser GetUserFromReader(User user)
        {
            object userId = user.UserID;
            string email = user.Email;
            string firstName = user.FirstName;
            string lastName = user.LastName;

            DateTime creationDate = Convert.ToDateTime(user.DateCreated);

            DateTime lastLoginDate = new DateTime();
            if (user.LastLoginDate != null)
                lastLoginDate = Convert.ToDateTime(user.LastLoginDate);

            DateTime lastActivityDate = new DateTime();
            if (user.LastActivityDate != null)
                lastActivityDate = Convert.ToDateTime(user.LastActivityDate);

            MembershipUser u = new MembershipUser(this.Name,
                                                  email,
                                                  userId,
                                                  email,
                                                  "",
                                                  "",
                                                  true,
                                                  false,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  DateTime.MinValue,
                                                  DateTime.MinValue);

            return u;
        }

        /// <summary>
        /// Get User By Email/userName
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            String UserFullName = String.Empty;

            try
            {
                using (FSPDataContext db = new FSPDataContext())
                {
                    var users = from q in db.Users
                                where q.Email.Equals(email)
                                select q;

                    if (users.Count() > 0)
                    {
                        User user = users.FirstOrDefault();
                        UserFullName = user.FirstName + " " + user.LastName;
                    }
                }

            }
            catch { }

            if (UserFullName == null)
                UserFullName = String.Empty;

            return UserFullName;
        }

        /// <summary>
        /// When user logs in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string email, string password)
        {
            bool isValid = false;
            String pwd = String.Empty;
            try
            {
                using (FSPDataContext db = new FSPDataContext())
                {
                    //check all APPROVED users
                    var users = from q in db.Users
                                where q.Email.Equals(email) && q.IsApproved == true
                                select q;

                    if (users.Count() > 0)
                    {
                        User user = users.FirstOrDefault();

                        pwd = user.Password;

                        if (CheckPassword(password, pwd))
                        {
                            isValid = true;

                            user.LastLoginDate = DateTime.Now;
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch { }


            return isValid;
        }


        #region password
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword =
              System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);


            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");


            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT PasswordAnswer, IsLockedOut FROM Users " +
                  " WHERE Username = ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            int rowsAffected = 0;
            string passwordAnswer = "";
            OdbcDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(1))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    passwordAnswer = reader.GetString(0);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }

                if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                {
                    UpdateFailureCount(username, "passwordAnswer");

                    throw new MembershipPasswordException("Incorrect password answer.");
                }

                OdbcCommand updateCmd = new OdbcCommand("UPDATE Users " +
                    " SET Password = ?, LastPasswordChangedDate = ?" +
                    " WHERE Username = ? AND ApplicationName = ? AND IsLockedOut = False", conn);

                updateCmd.Parameters.Add("@Password", OdbcType.VarChar, 255).Value = EncodePassword(newPassword);
                updateCmd.Parameters.Add("@LastPasswordChangedDate", OdbcType.DateTime).Value = DateTime.Now;
                updateCmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
                updateCmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

                rowsAffected = updateCmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "ResetPassword");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (rowsAffected > 0)
            {
                return newPassword;
            }
            else
            {
                throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            }
        }
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT Password, PasswordAnswer, IsLockedOut FROM Users " +
                  " WHERE Username = ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            string password = "";
            string passwordAnswer = "";
            OdbcDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(2))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    password = reader.GetString(0);
                    passwordAnswer = reader.GetString(1);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "GetPassword");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }


            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }

        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public override bool ChangePassword(string email, string oldPwd, string newPwd)
        {
            try
            {
                if (!ValidateUser(email, oldPwd))
                    return false;


                ValidatePasswordEventArgs args =
                  new ValidatePasswordEventArgs(email, newPwd, true);

                OnValidatingPassword(args);

                if (args.Cancel)
                    if (args.FailureInformation != null)
                        throw args.FailureInformation;
                    else
                        throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

                using (FSPDataContext db = new FSPDataContext())
                {
                    var users = from q in db.Users
                                where q.Email.Equals(email) && q.Password.Equals(EncodePassword(oldPwd))
                                select q;

                    if (users.Count() > 0)
                    {
                        User user = users.FirstOrDefault();
                        user.Password = EncodePassword(newPwd);
                        db.SubmitChanges();

                        return true;

                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch { return false; }

        }

        public override bool ChangePasswordQuestionAndAnswer(string username,
                      string password,
                      string newPwdQuestion,
                      string newPwdAnswer)
        {
            if (!ValidateUser(username, password))
                return false;

            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("UPDATE Users " +
                    " SET PasswordQuestion = ?, PasswordAnswer = ?" +
                    " WHERE Username = ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@Question", OdbcType.VarChar, 255).Value = newPwdQuestion;
            cmd.Parameters.Add("@Answer", OdbcType.VarChar, 255).Value = EncodePassword(newPwdAnswer);
            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;


            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");
                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }
        public string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }
        public string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion

        #region un-used
        public override MembershipUser GetUser(object userId, bool userIsOnline)
        {
            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand("SELECT ApplicationUserId, Email, FirstName," +
            //      " LastName, PhoneNumber, Address, City, State," +
            //      " Zip, Country, WantsNotification" +
            //      " FROM ApplicationUsers WHERE ApplicationUserId = " + userId, conn);

            MembershipUser u = null;
            //SqlDataReader reader = null;

            //try
            //{
            //    conn.Open();

            //    reader = cmd.ExecuteReader();

            //    if (reader.HasRows)
            //    {
            //        reader.Read();
            //        u = GetUserFromReader(reader);

            //        if (userIsOnline)
            //        {
            //            SqlCommand updateCmd = new SqlCommand("UPDATE ApplicationUsers " +
            //                      "SET LastActivityDate = " + DateTime.Now + " WHERE ApplicationUserId = " + userId, conn);

            //            updateCmd.ExecuteNonQuery();
            //        }
            //    }

            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        //WriteToEventLog(e, "GetUser(Object, Boolean)");

            //        throw new ProviderException(exceptionMessage);
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null) { reader.Close(); }

            //    conn.Close();
            //}

            return u;
        }
        public override bool UnlockUser(string username)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("UPDATE Users " +
                                              " SET IsLockedOut = False, LastLockedOutDate = ? " +
                                              " WHERE Username = ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@LastLockedOutDate", OdbcType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "UnlockUser");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }
        public override void UpdateUser(MembershipUser user)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("UPDATE Users " +
                    " SET Email = ?, Comment = ?," +
                    " IsApproved = ?" +
                    " WHERE Username = ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@Email", OdbcType.VarChar, 128).Value = user.Email;
            cmd.Parameters.Add("@Comment", OdbcType.VarChar, 255).Value = user.Comment;
            cmd.Parameters.Add("@IsApproved", OdbcType.Bit).Value = user.IsApproved;
            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = user.UserName;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;


            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "UpdateUser");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        private void UpdateFailureCount(string username, string failureType)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT FailedPasswordAttemptCount, " +
                                              "  FailedPasswordAttemptWindowStart, " +
                                              "  FailedPasswordAnswerAttemptCount, " +
                                              "  FailedPasswordAnswerAttemptWindowStart " +
                                              "  FROM Users " +
                                              "  WHERE Username = ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            OdbcDataReader reader = null;
            DateTime windowStart = new DateTime();
            int failureCount = 0;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (failureType == "password")
                    {
                        failureCount = reader.GetInt32(0);
                        windowStart = reader.GetDateTime(1);
                    }

                    if (failureType == "passwordAnswer")
                    {
                        failureCount = reader.GetInt32(2);
                        windowStart = reader.GetDateTime(3);
                    }
                }

                reader.Close();

                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a new password failure count from 1 and a new window starting now.

                    if (failureType == "password")
                        cmd.CommandText = "UPDATE Users " +
                                          "  SET FailedPasswordAttemptCount = ?, " +
                                          "      FailedPasswordAttemptWindowStart = ? " +
                                          "  WHERE Username = ? AND ApplicationName = ?";

                    if (failureType == "passwordAnswer")
                        cmd.CommandText = "UPDATE Users " +
                                          "  SET FailedPasswordAnswerAttemptCount = ?, " +
                                          "      FailedPasswordAnswerAttemptWindowStart = ? " +
                                          "  WHERE Username = ? AND ApplicationName = ?";

                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@Count", OdbcType.Int).Value = 1;
                    cmd.Parameters.Add("@WindowStart", OdbcType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
                    cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

                    if (cmd.ExecuteNonQuery() < 0)
                        throw new ProviderException("Unable to update failure count and window start.");
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.

                        cmd.CommandText = "UPDATE Users " +
                                          "  SET IsLockedOut = ?, LastLockedOutDate = ? " +
                                          "  WHERE Username = ? AND ApplicationName = ?";

                        cmd.Parameters.Clear();

                        cmd.Parameters.Add("@IsLockedOut", OdbcType.Bit).Value = true;
                        cmd.Parameters.Add("@LastLockedOutDate", OdbcType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
                        cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to lock out user.");
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        if (failureType == "password")
                            cmd.CommandText = "UPDATE Users " +
                                              "  SET FailedPasswordAttemptCount = ?" +
                                              "  WHERE Username = ? AND ApplicationName = ?";

                        if (failureType == "passwordAnswer")
                            cmd.CommandText = "UPDATE Users " +
                                              "  SET FailedPasswordAnswerAttemptCount = ?" +
                                              "  WHERE Username = ? AND ApplicationName = ?";

                        cmd.Parameters.Clear();

                        cmd.Parameters.Add("@Count", OdbcType.Int).Value = failureCount;
                        cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
                        cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to update failure count.");
                    }
                }
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "UpdateFailureCount");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("DELETE FROM Users " +
                    " WHERE Username = ? AND Applicationname = ?", conn);

            cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();

                if (deleteAllRelatedData)
                {
                    // Process commands to delete all data for the user in the database.
                }
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "DeleteUser");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM ApplicationUsers", conn);
            MembershipUserCollection users = new MembershipUserCollection();

            //SqlDataReader reader = null;
            //totalRecords = 0;

            //try
            //{
            //    conn.Open();
            totalRecords = 5;// (int)cmd.ExecuteScalar();

            //    if (totalRecords <= 0) { return users; }

            //    cmd.CommandText = "SELECT ApplicationUserId, Email, Password," +
            //             " FirstName, LastName, PhoneNumber, Address, City," +
            //             " State, Zip, Country " +
            //             " FROM ApplicationUsers " +
            //             " ORDER BY Email Asc";

            //    reader = cmd.ExecuteReader();

            //    int counter = 0;
            //    int startIndex = pageSize * pageIndex;
            //    int endIndex = startIndex + pageSize - 1;

            //    while (reader.Read())
            //    {
            //        if (counter >= startIndex)
            //        {
            //            MembershipUser u = GetUserFromReader(reader);
            //            users.Add(u);
            //        }

            //        if (counter >= endIndex) { cmd.Cancel(); }

            //        counter++;
            //    }
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        //WriteToEventLog(e, "GetAllUsers ");

            //        throw new ProviderException(exceptionMessage);
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null) { reader.Close(); }
            //    conn.Close();
            //}

            return users;
        }
        public override int GetNumberOfUsersOnline()
        {

            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT Count(*) FROM Users " +
                    " WHERE LastActivityDate > ? AND ApplicationName = ?", conn);

            cmd.Parameters.Add("@CompareDate", OdbcType.DateTime).Value = compareTime;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            int numOnline = 0;

            try
            {
                conn.Open();

                numOnline = (int)cmd.ExecuteScalar();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    //WriteToEventLog(e, "GetNumberOfUsersOnline");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return numOnline;
        }

        public override MembershipUserCollection FindUsersByName(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {

            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM ApplicationUsers WHERE Username LIKE '" + emailToMatch + "'", conn);
            MembershipUserCollection users = new MembershipUserCollection();

            //SqlDataReader reader = null;

            //try
            //{
            //    conn.Open();
            totalRecords = 5; // (int)cmd.ExecuteScalar();

            //    if (totalRecords <= 0) { return users; }

            //    cmd.CommandText = "SELECT ApplicationUserId, Email, Password," +
            //           " FirstName, LastName, PhoneNumber, Address, City," +
            //           " State, Zip, Country " +
            //           " FROM ApplicationUsers " +
            //            " WHERE Username LIKE '" + emailToMatch + "'" +
            //           " ORDER BY Email Asc";

            //    reader = cmd.ExecuteReader();

            //    int counter = 0;
            //    int startIndex = pageSize * pageIndex;
            //    int endIndex = startIndex + pageSize - 1;

            //    while (reader.Read())
            //    {
            //        if (counter >= startIndex)
            //        {
            //            MembershipUser u = GetUserFromReader(reader);
            //            users.Add(u);
            //        }

            //        if (counter >= endIndex) { cmd.Cancel(); }

            //        counter++;
            //    }
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        //WriteToEventLog(e, "FindUsersByName");

            //        throw new ProviderException(exceptionMessage);
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null) { reader.Close(); }

            //    conn.Close();
            //}

            return users;
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM ApplicationUsers " +
            //                                  "WHERE Email LIKE '" + emailToMatch + "'", conn);
            MembershipUserCollection users = new MembershipUserCollection();

            //SqlDataReader reader = null;
            //totalRecords = 0;

            //try
            //{
            //    conn.Open();
            totalRecords = 5; // (int)cmd.ExecuteScalar();

            //    if (totalRecords <= 0) { return users; }

            //    cmd.CommandText = "SELECT ApplicationUserId, Email, FirstName," +
            //             " LastName, PhoneNumber, Address, City, State," +
            //             " Zip, Country, WantsNotification " +
            //             " FROM ApplicationUsers " +
            //             " WHERE Email LIKE '" + emailToMatch + "'" +
            //             " ORDER BY Email Asc";

            //    reader = cmd.ExecuteReader();

            //    int counter = 0;
            //    int startIndex = pageSize * pageIndex;
            //    int endIndex = startIndex + pageSize - 1;

            //    while (reader.Read())
            //    {
            //        if (counter >= startIndex)
            //        {
            //            MembershipUser u = GetUserFromReader(reader);
            //            users.Add(u);
            //        }

            //        if (counter >= endIndex) { cmd.Cancel(); }

            //        counter++;
            //    }
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        //WriteToEventLog(e, "FindUsersByEmail");

            //        throw new ProviderException(exceptionMessage);
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null) { reader.Close(); }

            //    conn.Close();
            //}

            return users;
        }

        #endregion

        #endregion
    }
}
