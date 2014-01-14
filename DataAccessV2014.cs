using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Developer: Tupaz, Reiner S.
/// Date Created: 02/19/2013
///
/// ********************************************************************************************************
/// REVISION HISTORY:
/// CHANGE DATE:    CHANGED BY:             DESCRIPTION
/// 02/19/2013                              Creation of the class
/// 06/15/2013      Vasay, Brian Albert H.  Revised the naming convention
/// 07/16/2013      Vasay, Brian Albert H.  Included try / catch statements into the methods
/// 08/01/2013      Tupaz, Reiner S.        Included StoredProcReturnDataSet method(s) w/ & w/o parameter(s)
/// 08/01/2013      Tupaz, Reiner S.        Included IsExisting method(s) w/ & w/o parameter(s)
/// 08/01/2013      Tupaz, Reiner S.        Included ReturnIndex method(s) w/ & w/o parameter(s)
/// 08/28/2013      Vasay, Brian Albert H.  Included IsExisting method(s) w/ & w/o parameter(s)
/// 09/02/2013      Vasay, Brian Albert H.  Included ReturnEntryCount method(s) w/ & w/o parameter(s)
/// 11/22/2013      Tupaz, Reiner S.        Included Close if Open Connection to Finally Blocks
/// 12/17/2013      Tupaz, Reiner S.        Included CloseConnection method to close an sql connection
/// 12/17/2013      Tupaz, Reiner S.        Included ReturnReader method(s) w/ & w/o parameter(s)
/// 12/17/2013      Tupaz, Reiner S.        Included StoredProcReturnReader method
/// 01/13/2014      Vasay, Brian Albert H.  Included class constructors w/ connection string argument(s)
/// 01/13/2014      Vasay, Brian Albert H.  Included overload methods w/ respect to the class constructors
/// 01/13/2014      Vasay, Brian Albert H.  Included XML documentation to all methods and properties
/// ********************************************************************************************************
/// </summary>

namespace ISTA.Core
{
    public class DataAccess
    {
        private string connectionstring;
        private SqlConnection con = new SqlConnection();

        /// <summary>
        /// This is to provide access to the database via different SQL commands
        /// </summary>
        /// <param name="strConnectionString">The string variable that will serve as the connection string as declared in the web.config</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataAccess(string strConnectionString)
        {
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            try
            {
                connectionstring = ConfigurationManager.ConnectionStrings[strConnectionString].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to provide access to the database via different sql commands
        /// </summary>
        /// <param name="strDataSource">The name of the SQL database server</param>
        /// <param name="strDatabase">The name of the database that will be accessed</param>
        /// <param name="strUserId">The username for the SQL database server</param>
        /// <param name="strPassword">The password for the SQL database server</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataAccess(string strDataSource, string strDatabase, string strUserId, string strPassword)
        {
            if (strDataSource == null || strDataSource.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strDatabase == null || strDatabase.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strUserId == null || strUserId.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strPassword == null || strPassword.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            try
            {
                connectionstring = "Data Source=" + strDataSource + ";" + "Initial Catalog=" + strDatabase + ";" + "User ID=" + strUserId + ";" + "Password=" + strPassword + ";";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to determine if value(s) returned by a given SQL statement exists
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <returns>A boolean variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public bool IsExisting(string strSQLQuery)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                bool isexisting = false;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isexisting = true;
                }
                con.Close();
                return isexisting;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine if value(s) returned by a given SQL statement exists
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>A boolean variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public bool IsExisting(string strSQLQuery, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                bool isexisting = false;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isexisting = true;
                }
                con.Close();
                return isexisting;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine if value(s) returned by a given SQL statement exists
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>A boolean variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public bool IsExisting(string strSQLQuery, string strConnectionString)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                bool isexisting = false;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isexisting = true;
                }
                con.Close();
                return isexisting;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine if value(s) returned by a given SQL statement exists
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>A boolean variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public bool IsExisting(string strSQLQuery, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                bool isexisting = false;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isexisting = true;
                }
                con.Close();
                return isexisting;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet ReturnDataSet(string strSQLQuery)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet ReturnDataSet(string strSQLQuery, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet ReturnDataSet(string strSQLQuery, string strConnectionString)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet ReturnDataSet(string strSQLQuery, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given stored procedure
        /// </summary>
        /// <param name="strStoredProcedure">The stored procedure for the given method</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet StoredProcReturnDataSet(string strStoredProcedure)
        {
            if (strStoredProcedure == null || strStoredProcedure.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strStoredProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given stored procedure
        /// </summary>
        /// <param name="strStoredProcedure">The stored procedure for the given method</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet StoredProcReturnDataSet(string strStoredProcedure, SqlParameter[] Parameter)
        {
            if (strStoredProcedure == null || strStoredProcedure.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strStoredProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(Parameter);
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given stored procedure
        /// </summary>
        /// <param name="strStoredProcedure">The stored procedure for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet StoredProcReturnDataSet(string strStoredProcedure, string strConnectionString)
        {
            if (strStoredProcedure == null || strStoredProcedure.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strStoredProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return a set of values from a given stored procedure
        /// </summary>
        /// <param name="strStoredProcedure">The stored procedure for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>A DataSet object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public DataSet StoredProcReturnDataSet(string strStoredProcedure, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strStoredProcedure == null || strStoredProcedure.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                DataSet d = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand(strStoredProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(Parameter);
                SqlDataAdapter dataadapter = new SqlDataAdapter(cmd);
                dataadapter.Fill(d);
                con.Close();
                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine the number of entries returned by an SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <returns>An integer variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnEntryCount(string strSQLQuery)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                int entrycount;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                entrycount = Convert.ToInt32(sdr[0]);
                sdr.Close();
                con.Close();
                return entrycount;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine the number of entries returned by an SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>An integer variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnEntryCount(string strSQLQuery, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                int entrycount;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                entrycount = Convert.ToInt32(sdr[0]);
                sdr.Close();
                con.Close();
                return entrycount;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine the number of entries returned by an SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>An integer variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnEntryCount(string strSQLQuery, string strConnectionString)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                int entrycount;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                entrycount = Convert.ToInt32(sdr[0]);
                sdr.Close();
                con.Close();
                return entrycount;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to determine the number of entries returned by an SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>An integer variable</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnEntryCount(string strSQLQuery, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                int entrycount;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                entrycount = Convert.ToInt32(sdr[0]);
                sdr.Close();
                con.Close();
                return entrycount;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return the primary key of an SQL INSERT statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <returns>The primary key</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnIndex(string strSQLQuery)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery + "; SELECT SCOPE_IDENTITY()";

                int id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return the primary key of an SQL INSERT statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The primary key</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnIndex(string strSQLQuery, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery + "; SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddRange(Parameter);

                int id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return the primary key of an SQL INSERT statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>The primary key</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnIndex(string strSQLQuery, string strConnectionString)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery + "; SELECT SCOPE_IDENTITY()";

                int id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return the primary key of an SQL INSERT statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The primary key</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public int ReturnIndex(string strSQLQuery, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery + "; SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddRange(Parameter);

                int id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return value 0 of a given field
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The value of the given field</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public string ReturnField(string strSQLQuery, string strFieldName)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strFieldName == null || strFieldName.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                string data;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                data = sdr[strFieldName].ToString();
                sdr.Close();
                con.Close();
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return value 0 of a given field
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The value of the given field</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public string ReturnField(string strSQLQuery, string strFieldName, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strFieldName == null || strFieldName.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                string data;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                data = sdr[strFieldName].ToString();
                sdr.Close();
                con.Close();
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return value 0 of a given field
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The value of the given field</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public string ReturnField(string strSQLQuery, string strConnectionString, string strFieldName)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strFieldName == null || strFieldName.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                string data;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                data = sdr[strFieldName].ToString();
                sdr.Close();
                con.Close();
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return value 0 of a given field
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The value of the given field</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public string ReturnField(string strSQLQuery, string strConnectionString, string strFieldName, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strFieldName == null || strFieldName.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                string data;
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                data = sdr[strFieldName].ToString();
                sdr.Close();
                con.Close();
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to return an SqlDataReader object from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <returns>An SqlDataReader object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public SqlDataReader ReturnReader(string strSQLQuery)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to return an SqlDataReader object from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>An SqlDataReader object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public SqlDataReader ReturnReader(string strSQLQuery, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery;
                cmd.Parameters.AddRange(Parameter);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to return an SqlDataReader object from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>An SqlDataReader object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public SqlDataReader ReturnReader(string strSQLQuery, string strConnectionString)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to return an SqlDataReader object from a given SQL statement
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>An SqlDataReader object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public SqlDataReader ReturnReader(string strSQLQuery, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQLQuery;
                cmd.Parameters.AddRange(Parameter);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to return an SqlDataReader object from a given SQL statement
        /// </summary>
        /// <param name="strStoredProcedure">The stored procedure for the given method</param>
        /// <returns>An SqlDataReader object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public SqlDataReader StoredProcReturnReader(string strStoredProceduce)
        {
            if (strStoredProceduce == null || strStoredProceduce.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                SqlCommand cmd = new SqlCommand(strStoredProceduce, con);
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to return an SqlDataReader object from a given SQL statement
        /// </summary>
        /// <param name="strStoredProcedure">The stored procedure for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <returns>An SqlDataReader object</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public SqlDataReader StoredProcReturnReader(string strStoredProceduce, string strConnectionString)
        {
            if (strStoredProceduce == null || strStoredProceduce.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand(strStoredProceduce, con);
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This is to handle INSERT, UPDATE, and DELETE SQL statements
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The return code</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public void ExecuteNonQuery(string strSQLQuery)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to handle INSERT, UPDATE, and DELETE SQL statements
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The return code</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public void ExecuteNonQuery(string strSQLQuery, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = connectionstring;
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to handle INSERT, UPDATE, and DELETE SQL statements
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The return code</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public void ExecuteNonQuery(string strSQLQuery, string strConnectionString)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to handle INSERT, UPDATE, and DELETE SQL statements
        /// </summary>
        /// <param name="strSQLQuery">The SQL query for the given method</param>
        /// <param name="strConnectionString">The connection string to the relevant database</param>
        /// <param name="Parameter">The parameter(s) for the SQL query</param>
        /// <returns>The return code</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.Exception"/>
        public void ExecuteNonQuery(string strSQLQuery, string strConnectionString, SqlParameter[] Parameter)
        {
            if (strSQLQuery == null || strSQLQuery.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (strConnectionString == null || strConnectionString.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (Parameter == null || Parameter.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            CloseConnection();
            con.ConnectionString = strConnectionString;
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(strSQLQuery, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(Parameter);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// This is to close currently active SQL connections prior or after execution of SQL statements
        /// </summary>
        /// <exception cref="System.Exception"/>
        public void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
