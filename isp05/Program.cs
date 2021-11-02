using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace isp05
 {
     static class Program
     {
         static void Main(string[] args)
         {
             InitDB();
         }
 
         static void InitDB()
         {
            // The Database object to operate on
            DatabaseManagement DB = new DatabaseManagement(connectionSource:"Data Source=:memory:"); // @"URI=file:C:\Users\gamin\RiderProjects\a2\isps\isp05\isp05\data.db" 

            // Creates the table of authors, ready to be populated from the file
            DB.SendNonQuery(@"
            CREATE TABLE authors (
                au_id VARCHAR(20) PRIMARY KEY NOT NULL,
                au_lname VARCHAR(20) NOT NULL,
                au_fname VARCHAR(20) NOT NULL,
                au_phone VARCHAR(20) NOT NULL,
                au_address VARCHAR(20) NOT NULL,
                au_city VARCHAR(20) NOT NULL,
                au_state VARCHAR(20) NOT NULL,
                au_zip VARCHAR(20) NOT NULL,
                au_contract BOOLEAN NOT NULL
            );");


            //DB.SendNonQuery("INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract)VALUES(172 - 32 - 1176, White, Johnson, 408 - 496 - 7223, 10932 Bigge Rd., Menlo Park, CA, 94025, y); ");

            //Environment.Exit(0);

            PopulateDB("C:\\Users\\gamin\\RiderProjects\\a2\\isps\\isp05\\isp05\\Data.txt", DB);

            //Console.WriteLine(reader.GetValues()[0]);
        }

        /// <summary>
        /// Populates the Database with data from a given file
        /// </summary>
        /// <param name="file">The file containing the data</param>
        static void PopulateDB(string file, DatabaseManagement DB)
        {
            List<string> dataFromFile = Data.ReadFile(filePath: file);


            // Assign each value to the correct entry in the database

            // Build query

            string query = ""; // "INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract) VALUES(";
            List<string> lineToInsert = new List<string>();
            List<List<string>> newList = new List<List<string>>();

            query = "INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract) VALUES(";
            
            for (int i = 0; i < dataFromFile.Count - 1; ++i)
            {
                newList.Add(dataFromFile[i].Split("|").ToList());
                newList[i] = Data.AddQuotes(newList[i]);
            }

            // query = "INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract) VALUES(";
            
            // Adds each line in the list to the query
            for (int i = 0; i < newList.Count; i++)
            {
                for (int j = 0; j < newList[i].Count; j++)
                {
                    query += newList[i][j];

                    if (j != 8)
                        query += ",";
                }

                query += ");";
            
                DB.SendNonQuery(query);
                
                query = "INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract) VALUES(";
            }

            DB.SendQuery(toExecute: "SELECT au_id FROM authors");

            // Profit question mark
        }
     }
 }