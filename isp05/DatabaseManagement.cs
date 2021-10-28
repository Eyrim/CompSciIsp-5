//TODO: Flesh out the error handling

using System;
using System.Collections.Generic;
using System.Collections;
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
        /// <param name="toExecute"></param>
        /// <returns></returns>
        public string SendQuery(string toExecute)
        {
            using (SQLiteCommand command = new SQLiteCommand(this.Connection))
            {
                // The dictionary to hold the data returned from the DB
                Dictionary<string, string> dataFromDB = new Dictionary<string, string>();
                
                command.CommandText = toExecute;
                
                // The reader for the data from the DB
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    IEnumerator readerEnum = reader.GetEnumerator();
                    
                    //dataFromDB.Add(key:reader.GetString(), value:);
                    reader.Read();
                    return reader.GetString(0);
                }
            }
        }
        
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
                string toExecute = @$"ALTER TABLE";

                toExecute += @$"{TableName}(";
                toExecute += FieldType;
                toExecute += (IsPrimaryKey) ? "PRIMARY KEY" : "";
                toExecute += (!IsNullable) ? "NOT NULL" : "";
                toExecute += (IsAutoIncrement) ? "AUTO INCREMENT" : "";
                toExecute += ");";

                command.CommandText = toExecute;
                command.ExecuteNonQuery();
            }
        }

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

                // Enumerator to traverse key collection
                using (Dictionary<string, string>.KeyCollection.Enumerator keyEnumerator = valueKeys.GetEnumerator())
                {
                    // Enumerators start one before the first element, so moving to first here
                    keyEnumerator.MoveNext();
                    
                    // Whether the end of the Dict has been reached
                    bool canMove = false;

                    for (int i = 0; i < Values.Count - 1; i++)
                    {
                        // Adds the current key to the command
                        toExecute += @$"{keyEnumerator.Current},";
                        Console.WriteLine("DEBUG: keyEnumerator.Current: " + keyEnumerator.Current);
                        
                        // Moves the enumerator and checks if end of dict reached
                        canMove = (keyEnumerator.MoveNext()) ? true : false;

                        // If end of dict reached then break
                        if (canMove == false)
                        {
                            // Remove the last comma, which should be the last character
                            toExecute = toExecute.Remove(startIndex: toExecute.Length - 1, count: 1);
                            Console.WriteLine("DEBUG: toExecute: " + toExecute);
                            toExecute += ")VALUES(";
                            break;
                        }
                    }

                    
                    // ADDING THE DATA ITSELF
                    
                    //INSERT INTO table_name (column1, column2, column3, ...)
                    //VALUES (value1, value2, value3, ...);

                    for (int i = 0; i < Values.Count - 1; i++)
                    {
                        toExecute += Values[valueKeys.ToList()[i]];
                        toExecute += ",";
                    }
                    
                    toExecute = toExecute.Remove(startIndex: toExecute.Length - 1, count: 1);
                    Console.WriteLine("DEBUG: toExecute: " + toExecute);
                    toExecute += ");";
                    
                    command.CommandText = toExecute;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}