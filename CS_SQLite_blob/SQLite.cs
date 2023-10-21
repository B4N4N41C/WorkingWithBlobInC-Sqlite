using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Finisar.SQLite;


using System.IO;

namespace CS_SQLite_blob
{
    class SQLite
    {
        //--
        //SQLite
        public static String DBpath = "SQLite_Blob.db";
        public static String StrModifyuid = "";
        public static SQLiteConnection m_icn = new SQLiteConnection();
        public static void initSQLiteDatabase()
        {

            if (!System.IO.File.Exists(DBpath))
            {
                CreateSQLiteDatabase(DBpath);
                string createtablestring = "";

                createtablestring = "CREATE TABLE Blob_Data (uid INTEGER PRIMARY KEY,name TEXT UNIQUE, data BLOB not null);";
                CreateSQLiteTable(DBpath, createtablestring);

            }
        }
        public static SQLiteConnection OpenConn(string Database)
        {
            string cnstr = string.Format("Data Source=" + Database + ";Version=3;New=False;Compress=True;");
            if (m_icn.State == ConnectionState.Closed)//if (m_icn.State != ConnectionState.Open)
            {
                m_icn.ConnectionString = cnstr;
                m_icn.Open();
            }
            return m_icn;
        }
        public static void CloseConn()
        {
            m_icn.Close();
        }
        public static void CreateSQLiteDatabase(string Database)
        {
            string cnstr = string.Format("Data Source=" + Database + ";Version=3;New=True;Compress=True;");
            SQLiteConnection icn = new SQLiteConnection();
            icn.ConnectionString = cnstr;
            icn.Open();
            icn.Close();
        }
        public static void CreateSQLiteTable(string Database, string CreateTableString)
        {
            SQLiteConnection icn = OpenConn(Database);
            SQLiteCommand cmd = new SQLiteCommand(CreateTableString, icn);
            SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                cmd.Transaction = mySqlTransaction;
                cmd.ExecuteNonQuery();
                mySqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                mySqlTransaction.Rollback();
                throw (ex);
            }
            if (icn.State == ConnectionState.Open) icn.Close();
        }
        public static void SQLiteInsertImge(string Database, string SqlSelectString, byte[] buffer)
        {
            SQLiteConnection icn = OpenConn(Database);
            SQLiteCommand cmd = new SQLiteCommand(SqlSelectString, icn);
            //SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                //cmd.Transaction = mySqlTransaction;

                SQLiteParameter para = new SQLiteParameter("@data", DbType.Binary);
                para.Value = buffer;
                cmd.Parameters.Add(para);
                
                cmd.ExecuteNonQuery();
                //mySqlTransaction.Commit();
            }
            catch //(Exception ex)
            {
                //mySqlTransaction.Rollback();
                //throw (ex);
            }
            if (icn.State == ConnectionState.Open) icn.Close();
        }
        public static void SQLiteInsertUpdateDelete(string Database, string SqlSelectString)
        {
            SQLiteConnection icn = OpenConn(Database);
            SQLiteCommand cmd = new SQLiteCommand(SqlSelectString, icn);
            SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                cmd.Transaction = mySqlTransaction;
                cmd.ExecuteNonQuery();
                mySqlTransaction.Commit();
            }
            catch //(Exception ex)
            {
                mySqlTransaction.Rollback();
                //throw (ex);
            }
            if (icn.State == ConnectionState.Open) icn.Close();
        }
        public static SQLiteDataReader GetDataReader(string Database, string SQLiteString)
        {
            SQLiteConnection icn = OpenConn(Database);
            SQLiteDataAdapter da = new SQLiteDataAdapter(SQLiteString, icn);

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = icn.CreateCommand();
            sqlite_cmd.CommandText = SQLiteString;

            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
            return sqlite_datareader;
        }
        public static DataTable GetDataTable(string Database, string SQLiteString)
        {
            DataTable myDataTable = new DataTable();
            SQLiteConnection icn = OpenConn(Database);
            SQLiteDataAdapter da = new SQLiteDataAdapter(SQLiteString, icn);
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);

            myDataTable = ds.Tables[0];
            return myDataTable;
        }
        
        public static void SQLite_clearDB()
        {
            SQLiteInsertUpdateDelete(DBpath, "DELETE FROM users;");
            SQLiteInsertUpdateDelete(DBpath, "DELETE FROM user_ext_group;");
            SQLiteInsertUpdateDelete(DBpath, "DELETE FROM dept;");
            SQLiteInsertUpdateDelete(DBpath, "DELETE FROM door;");
            if (System.IO.File.Exists("AutoSave0.dat"))
            {
                System.IO.File.Delete("AutoSave0.dat");
            }
            if (System.IO.File.Exists("AutoSave1.dat"))
            {
                System.IO.File.Delete("AutoSave1.dat");
            }
        }
       
    }
}
