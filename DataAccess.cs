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
/// *******************************************************************************************
/// REVISION HISTORY:
/// CHANGE DATE: CHANGED BY: DESCRIPTION
/// 02/19/2013 Creation of the class
/// 06/15/2013 Vasay, Brian Albert H. Revised the naming convention
/// 07/16/2013 Vasay, Brian Albert H. Included try-catch statements into the methods
/// 08/01/2013 Tupaz, Reiner S. Included methods for executing stored procedures
/// 08/01/2013 Tupaz, Reiner S. Included method for CheckIfExisting w/o parameters
/// 08/01/2013 Tupaz, Reiner S. Included method for returning primary key after
/// insert statements are executed
/// 08/28/2013 Vasay, Brian Albert H. Included DetermineIfExisting method w/o parameters
/// 09/02/2013 Vasay, Brian Albert H. Included DetermineEntryCount method w/ & w/o parameters
/// 11/22/2013 Tupaz, Reiner S. Made Methods Static, debugged InsertAndGetIndex
/// 11/22/2013 Tupaz, Reiner S. Added Close if Open Connection to Finally Blocks
/// 12/17/2013 Tupaz, Set SQL Connection to be instanciated outside the methods inside the class
/// 12/17/2013 Tupaz, Added a method ForceConnectionToClose to force connection to close
/// 12/17/2013 Tupaz, Added a method ReturnReader to Return SqlDataReader Objects with 2 overrides
/// 12/17/2013 Tupaz, Added a method ReturnReaderFromStoredProcedure, returns SqlDataReader object
/// *******************************************************************************************
/// </summary>

namespace DBHelpers
{
    public static class DataAccess
    {

        //SqlConnection Declared Here.
        private static SqlConnection myConnection = new SqlConnection();

        // This is to handle insert, update, and delete with parameter(s)
        public static void DataProcessExecuteNonQuery(string strQuery, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {

                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                myCommand.Parameters.AddRange(Parameter);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to handle insert, update, and delete without parameters
        public static void DataProcessExecuteNonQuery(string strQuery, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to return a single value read with the use of parameter(s)
        public static string ReturnData(string strQuery, SqlParameter[] Parameter, string Connection_String, string Field_Name)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                string strData;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                myCommand.Parameters.AddRange(Parameter);
                SqlDataReader sdr = myCommand.ExecuteReader();
                sdr.Read();
                strData = sdr[Field_Name].ToString();
                sdr.Close();
                myConnection.Close();
                return strData;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to return a single value read without the use of parameters
        public static string ReturnData(string strQuery, string Connection_String, string Field_Name)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                string strData;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                SqlDataReader sdr = myCommand.ExecuteReader();
                sdr.Read();
                strData = sdr[Field_Name].ToString();
                sdr.Close();
                myConnection.Close();
                return strData;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to determine if a value returned by the provided SQL statement exists
        public static bool DetermineIfExisting(string strQuery, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                bool validate;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                myCommand.Parameters.AddRange(Parameter);
                SqlDataReader sdr = myCommand.ExecuteReader();
                if (sdr.HasRows)
                {
                    validate = true;
                }
                else
                {
                    validate = false;
                }
                myConnection.Close();
                return validate;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to determine if a value returned by the provided SQL statement exists
        public static bool DetermineIfExisting(string strQuery, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                bool validate;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                SqlDataReader sdr = myCommand.ExecuteReader();
                if (sdr.HasRows)
                {
                    validate = true;
                }
                else
                {
                    validate = false;
                }
                myConnection.Close();
                return validate;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to determine the number of entries in a given table with the use of parameters
        public static int DetermineEntryCount(string strQuery, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                int EntryCount;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                myCommand.Parameters.AddRange(Parameter);
                SqlDataReader sdr = myCommand.ExecuteReader();
                sdr.Read();
                EntryCount = Convert.ToInt32(sdr[0]);
                sdr.Close();
                myConnection.Close();
                return EntryCount;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to determine the number of entries in a given table without the use of parameters
        public static int DetermineEntryCount(string strQuery, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                int EntryCount;
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                SqlDataReader sdr = myCommand.ExecuteReader();
                sdr.Read();
                EntryCount = Convert.ToInt32(sdr[0]);
                sdr.Close();
                myConnection.Close();
                return EntryCount;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to return a DataSet object for the use of relevant ASP.Net tools (ListView, GridView, etc.)
        // with the use of parameters
        public static DataSet DataProcessReturnData(string strQuery, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                DataSet dataset = new DataSet();
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                myCommand.Parameters.AddRange(Parameter);
                SqlDataAdapter dataadapter = new SqlDataAdapter(myCommand);
                dataadapter.Fill(dataset);
                myConnection.Close();
                return dataset;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to return a DataSet object for the use of relevant ASP.Net tools (ListView, GridView, etc.)
        // without the use of parameters
        public static DataSet DataProcessReturnData(string strQuery, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                DataSet dataset = new DataSet();
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(strQuery, myConnection);
                myCommand.CommandType = CommandType.Text;
                SqlDataAdapter dataadapter = new SqlDataAdapter(myCommand);
                dataadapter.Fill(dataset);
                myConnection.Close();
                return dataset;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to execute stored procedures with a DataSet return type with the use of parameters
        public static DataSet StoredProcedureGetData(string storedProc, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                DataSet dataset = new DataSet();
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(storedProc, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddRange(Parameter);
                SqlDataAdapter dataadapter = new SqlDataAdapter(myCommand);
                dataadapter.Fill(dataset);
                myConnection.Close();
                return dataset;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
        // This is to execute stored procedures with a DataSet return type without the use of parameters
        public static DataSet StoredProcedureGetData(string storedProc, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                DataSet dataset = new DataSet();
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(storedProc, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataadapter = new SqlDataAdapter(myCommand);
                dataadapter.Fill(dataset);
                myConnection.Close();
                return dataset;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        // This is to return the new primary key after an insert statement is executed with the use of parameters
        // Works for INSERT statements only
        public static int InsertAndGetIndex(string strQuery, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = strQuery + "; SELECT SCOPE_IDENTITY()";
                myCommand.Parameters.AddRange(Parameter);

                int NewRowID = int.Parse(myCommand.ExecuteScalar().ToString());
                myConnection.Close();
                return NewRowID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        // This is to return the new primary key after an insert statement is executed without the use of parameters
        // Works for INSERT statements only
        public static int InsertAndGetIndex(string strQuery, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = strQuery + "; SELECT SCOPE_IDENTITY()";

                int NewRowID = int.Parse(myCommand.ExecuteScalar().ToString());
                myConnection.Close();
                return NewRowID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        //Returns a datareader object, w/parameters.
        public static SqlDataReader ReturnReader(string strQuery, SqlParameter[] Parameter, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = strQuery;
                myCommand.Parameters.AddRange(Parameter);
                myConnection.Open();
                SqlDataReader dr = myCommand.ExecuteReader();
                return dr;                
            }
            catch (Exception e)
            {     
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                throw e;
            }
            finally 
            {
                
            }
        }
        //Returns a datareader object, w/out parameters.
        public static SqlDataReader ReturnReader(string strQuery, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = strQuery;
                myConnection.Open();
                SqlDataReader dr = myCommand.ExecuteReader();
                return dr;
            }
            catch (Exception e)
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                throw e;
            }
            finally
            {

            }
        }
        //Returns a datareader object, for stored procedures.
        public static SqlDataReader ReturnReaderFromStoredProcedure(string strStoredProceduce, string Connection_String)
        {
            myConnection.ConnectionString = Connection_String;
            try
            {
                SqlCommand myCommand = new SqlCommand(strStoredProceduce, myConnection);
                myCommand.Connection = myConnection;
                myCommand.CommandType = CommandType.StoredProcedure;
                myConnection.Open();
                SqlDataReader dr = myCommand.ExecuteReader();
                return dr;
            }
            catch (Exception e)
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                throw e;
            }
            finally
            {

            }
        }
        //Just in case, this method forces the pre-declared SQL connection to close.
        public static void ForceConnectionToClose()
        {
            try
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
