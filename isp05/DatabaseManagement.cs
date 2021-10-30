//TODO: Flesh out the error handling
//TODO: Document

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace isp05
{
    public class DatabaseManagement
    {
        /// <summary>
        /// The Connection Source for the DB
        /// </summary>
        private string ConnectionSource;

        /// <summary>
        /// The Connection itself to the DB
        /// </summary>
        private SQLiteConnection Connection;

        /// <summary>
        /// Is the connection to the DB alive
        /// </summary>
        private bool IsConnected = false;
        
        /// <summary>
        /// Constructor for the DatabaseManagement class
        /// Auto Connects to DB
        /// </summary>
        /// <param name="connectionSource">The source for the DB connection, default is :memory:</param>
        public DatabaseManagement(string connectionSource)
        {
            this.ConnectionSource = connectionSource;
            
            // Creates a new SQL Connection
            this.Connection = new SQLiteConnection(this.ConnectionSource);
            
            // Connects to DB
            this.Connection.Open();
            
            this.IsConnected = true;
        }
        
        /// <summary>
        /// Closes the DB connection held in this.Connection
        /// </summary>
        /// <returns>void</returns>
        public void CloseDBConnection()
        {
            if (this.IsConnected)
            {
                this.Connection.Close();
            }

            else
            {
                // If this.Connection is already closed then raise new Exception exception
                throw new DatabaseNotConnectedException(message:"Database Must Be Connected In Order For Connection To Be Closed");
            }
        }

        /// <summary>
        /// Sends a NonQuery to the DB
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="isp05.DatabaseNotConnectedException">If operations on a closed database are attempted</exception>
        public void SendNonQuery(string toExecute)
        {
            if (this.IsConnected)
            {
                using (SQLiteCommand command = new SQLiteCommand(this.Connection))
                {
                    command.CommandText = toExecute;
                    command.ExecuteNonQuery();
                }
            }

            else
            {
                throw new DatabaseNotConnectedException(message: "Database Must Be Connected In Order For Connection To Be Closed");
            }
        }

        /// <summary>
        /// Sends a DB query, such as a SELECT statement
        /// </summary>
        /// <param name="toExecute">The query to send to the DB</param>
        /// <returns>System.Data.SQLite.SQLiteDataReader</returns>
        public SQLiteDataReader SendQuery(string toExecute)
        {
            using (SQLiteCommand command = new SQLiteCommand(this.Connection))
            {
                command.CommandText = toExecute;
                command.CommandType = CommandType.Text;

                SQLiteDataReader reader = command.ExecuteReader();

                return reader;
            }
        }

        /*
        using (SQLiteConnection connection = new SQLiteConnection(myDatabase.mySQLiteConnection))
        {
            connection.Open();

            using (SQLiteCommand selectCMD = connection.CreateCommand())
            {
                selectCMD.CommandText = "SELECT * FROM Food";
                selectCMD.CommandType = CommandType.Text;
                SQLiteDataReader myReader = selectCMD.ExecuteReader();
                while (myReader.Read())
                {
                    Console.WriteLine(myReader["FoodName"] + " " + myReader["FoodType"]);
                }
            }
    
        }
        
        */
        
        /// <summary>
        /// Initialises a field in the DB
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="ConnectionSource"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldType"></param>
        /// <param name="IsPrimaryKey"></param>
        /// <param name="IsNullable"></param>
        /// <param name="IsAutoIncrement"></param>
        /// <returns>void</returns>
        public void InitField(string FieldName, string TableName, 
            string FieldType, bool IsPrimaryKey = false, 
            bool IsNullable = true, bool IsAutoIncrement = false)
        {
            using (SQLiteCommand command = new SQLiteCommand(this.Connection))
            {
                /*ALTER TABLE table_name
                  ADD column_name datatype;*/
                string toExecute = @$"ALTER TABLE ";

                toExecute += @$"{TableName} ";
                toExecute += "ADD ";
                toExecute += $"{FieldName} ";
                toExecute += $"{FieldType} ";
                toExecute += (IsPrimaryKey) ? "PRIMARY KEY " : "";
                toExecute += (!IsNullable) ? "NOT NULL " : "";
                toExecute += (IsAutoIncrement) ? "AUTO INCREMENT" : "";
                toExecute += ";";

                command.CommandText = toExecute;
                command.ExecuteNonQuery();
            }
        }

        //TODO: REFACTOR
        /// <summary>
        /// A method to initialise a group of records
        /// </summary>
        /// <param name="Values"></param>
        /// <param name="TableName"></param>
        /// <returns>void</returns>
        public void InitListOfRecords(Dictionary<string, string> Values, string TableName)
        {
            // INSERT INTO <table>(Field1, Field2)
            // VALUES("thing", 10);
            using (SQLiteCommand command = new SQLiteCommand(this.Connection))
            {
                // The command text to execute
                string toExecute = $@"INSERT INTO {TableName}(";

                // The keys in the Dictionary passed in
                Dictionary<string, string>.KeyCollection valueKeys = Values.Keys;

                Console.WriteLine(valueKeys.Count);

                for (int i = 0; i < valueKeys.Count; i++)
                {
                    // Adds the current key to the command
                    toExecute += @$"{valueKeys.ToList()[i]}";
                    Console.WriteLine("DEBUG: keyEnumerator.Current: " + valueKeys.ToList()[i]);
                    Console.WriteLine("i: " + i);

                    if (i != valueKeys.Count - 1)
                        toExecute += ",";
                }

                toExecute += ")VALUES(";

                
                // ADDING THE DATA ITSELF
                
                //INSERT INTO table_name (column1, column2, column3, ...)
                //VALUES (value1, value2, value3, ...);

                for (int i = 0; i < Values.Count; i++)
                {
                    toExecute += $"\"{Values[valueKeys.ToList()[i]]}\"";
                    
                    if (i != valueKeys.Count - 1)
                        toExecute += ",";
                }

                toExecute += ");";
                Console.WriteLine("DEBUG: toExecute: " + toExecute);

                command.CommandText = toExecute;
                command.ExecuteNonQuery();
            }
        }
    }
}