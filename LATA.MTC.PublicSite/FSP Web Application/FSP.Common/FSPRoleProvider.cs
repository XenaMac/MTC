using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using FSP.Domain.Model;

namespace FSP.Common
{
    public sealed class FSPRoleProvider : RoleProvider
    {
        private string pApplicationName;
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            //foreach (string rolename in rolenames)
            //{
            //    if (!RoleExists(rolename))
            //    {
            //        throw new ProviderException("Role name not found.");
            //    }
            //}

            //foreach (string username in usernames)
            //{
            //    if (username.Contains(","))
            //    {
            //        throw new ArgumentException("User names cannot contain commas.");
            //    }

            //    foreach (string rolename in rolenames)
            //    {
            //        if (IsUserInRole(username, rolename))
            //        {
            //            throw new ProviderException("User is already in role.");
            //        }
            //    }
            //}


            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("INSERT INTO UsersInRoles " +
            //        " (Username, Rolename, ApplicationName) " +
            //        " Values(?, ?, ?)", conn);

            //OdbcParameter userParm = cmd.Parameters.Add("@Username", OdbcType.VarChar, 255);
            //OdbcParameter roleParm = cmd.Parameters.Add("@Rolename", OdbcType.VarChar, 255);
            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

            //OdbcTransaction tran = null;

            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    cmd.Transaction = tran;

            //    foreach (string username in usernames)
            //    {
            //        foreach (string rolename in rolenames)
            //        {
            //            userParm.Value = username;
            //            roleParm.Value = rolename;
            //            cmd.ExecuteNonQuery();
            //        }
            //    }

            //    tran.Commit();
            //}
            //catch (OdbcException e)
            //{
            //    try
            //    {
            //        tran.Rollback();
            //    }
            //    catch { }


            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "AddUsersToRoles");
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }

        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            using (FSPDataContext db = new FSPDataContext())
            {
                var roles = from q in db.Roles
                            where q.RoleName.Equals(rolename)
                            select q;

                if (roles.Count() > 0)
                {
                    exists = true;
                }


            }

            return exists;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            //foreach (string rolename in rolenames)
            //{
            //    if (!RoleExists(rolename))
            //    {
            //        throw new ProviderException("Role name not found.");
            //    }
            //}

            //foreach (string username in usernames)
            //{
            //    foreach (string rolename in rolenames)
            //    {
            //        if (!IsUserInRole(username, rolename))
            //        {
            //            throw new ProviderException("User is not in role.");
            //        }
            //    }
            //}


            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("DELETE FROM UsersInRoles " +
            //        " WHERE Username = ? AND Rolename = ? AND ApplicationName = ?", conn);

            //OdbcParameter userParm = cmd.Parameters.Add("@Username", OdbcType.VarChar, 255);
            //OdbcParameter roleParm = cmd.Parameters.Add("@Rolename", OdbcType.VarChar, 255);
            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

            //OdbcTransaction tran = null;

            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    cmd.Transaction = tran;

            //    foreach (string username in usernames)
            //    {
            //        foreach (string rolename in rolenames)
            //        {
            //            userParm.Value = username;
            //            roleParm.Value = rolename;
            //            cmd.ExecuteNonQuery();
            //        }
            //    }

            //    tran.Commit();
            //}
            //catch (OdbcException e)
            //{
            //    try
            //    {
            //        tran.Rollback();
            //    }
            //    catch { }


            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "RemoveUsersFromRoles");
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }

        public override bool IsUserInRole(string email, string rolename)
        {
            bool userIsInRole = false;

            using (FSPDataContext db = new FSPDataContext())
            {
                var roles = from q in db.Users
                            join r in db.Roles on q.RoleID equals r.RoleID
                            where q.Email.Equals(email) && r.RoleName.Equals(rolename)
                            select q;

                if (roles.Count() > 0)
                {
                    userIsInRole = true;
                }


            }
                       
            return userIsInRole;
        }

        public override string[] GetUsersInRole(string rolename)
        {
            //string tmpUserNames = "";

            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("SELECT Username FROM UsersInRoles " +
            //          " WHERE Rolename = ? AND ApplicationName = ?", conn);

            //cmd.Parameters.Add("@Rolename", OdbcType.VarChar, 255).Value = rolename;
            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

            //OdbcDataReader reader = null;

            //try
            //{
            //    conn.Open();

            //    reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        tmpUserNames += reader.GetString(0) + ",";
            //    }
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "GetUsersInRole");
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

            //if (tmpUserNames.Length > 0)
            //{
            //    // Remove trailing comma.
            //    tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
            //    return tmpUserNames.Split(',');
            //}

            return new string[0];
        }

        public override string[] GetRolesForUser(string email)
        {
            string tmpRoleNames = "";

            using (FSPDataContext db = new FSPDataContext())
            {
                var users = from q in db.Users                            
                            where q.Email.Equals(email)
                            select q;

                if (users.Count() > 0)
                {
                    tmpRoleNames = users.FirstOrDefault().Role.RoleName;
                }

            }
                      
            if (tmpRoleNames.Length > 0)
            {             
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            //if (!RoleExists(rolename))
            //{
            //    throw new ProviderException("Role does not exist.");
            //}

            //if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            //{
            //    throw new ProviderException("Cannot delete a populated role.");
            //}

            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("DELETE FROM Roles " +
            //        " WHERE Rolename = ? AND ApplicationName = ?", conn);

            //cmd.Parameters.Add("@Rolename", OdbcType.VarChar, 255).Value = rolename;
            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;


            //OdbcCommand cmd2 = new OdbcCommand("DELETE FROM UsersInRoles " +
            //        " WHERE Rolename = ? AND ApplicationName = ?", conn);

            //cmd2.Parameters.Add("@Rolename", OdbcType.VarChar, 255).Value = rolename;
            //cmd2.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

            //OdbcTransaction tran = null;

            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    cmd.Transaction = tran;
            //    cmd2.Transaction = tran;

            //    cmd2.ExecuteNonQuery();
            //    cmd.ExecuteNonQuery();

            //    tran.Commit();
            //}
            //catch (OdbcException e)
            //{
            //    try
            //    {
            //        tran.Rollback();
            //    }
            //    catch { }


            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "DeleteRole");

            //        return false;
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    conn.Close();
            //}

            return true;
        }

        public override string[] GetAllRoles()
        {
            //string tmpRoleNames = "";

            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("SELECT Rolename FROM Roles " +
            //          " WHERE ApplicationName = ?", conn);

            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

            //OdbcDataReader reader = null;

            //try
            //{
            //    conn.Open();

            //    reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        tmpRoleNames += reader.GetString(0) + ",";
            //    }
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "GetAllRoles");
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

            //if (tmpRoleNames.Length > 0)
            //{
            //    // Remove trailing comma.
            //    tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
            //    return tmpRoleNames.Split(',');
            //}

            return new string[0];
        }

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("SELECT Username FROM UsersInRoles  " +
            //          "WHERE Username LIKE ? AND RoleName = ? AND ApplicationName = ?", conn);
            //cmd.Parameters.Add("@UsernameSearch", OdbcType.VarChar, 255).Value = usernameToMatch;
            //cmd.Parameters.Add("@RoleName", OdbcType.VarChar, 255).Value = rolename;
            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = pApplicationName;

            //string tmpUserNames = "";
            //OdbcDataReader reader = null;

            //try
            //{
            //    conn.Open();

            //    reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        tmpUserNames += reader.GetString(0) + ",";
            //    }
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "FindUsersInRole");
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

            //if (tmpUserNames.Length > 0)
            //{
            //    // Remove trailing comma.
            //    tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
            //    return tmpUserNames.Split(',');
            //}

            return new string[0];
        }

        public override void CreateRole(string rolename)
        {
            //if (rolename.Contains(","))
            //{
            //    throw new ArgumentException("Role names cannot contain commas.");
            //}

            //if (RoleExists(rolename))
            //{
            //    throw new ProviderException("Role name already exists.");
            //}

            //OdbcConnection conn = new OdbcConnection(connectionString);
            //OdbcCommand cmd = new OdbcCommand("INSERT INTO Roles " +
            //        " (Rolename, ApplicationName) " +
            //        " Values(?, ?)", conn);

            //cmd.Parameters.Add("@Rolename", OdbcType.VarChar, 255).Value = rolename;
            //cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

            //try
            //{
            //    conn.Open();

            //    cmd.ExecuteNonQuery();
            //}
            //catch (OdbcException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "CreateRole");
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }
    }
}
