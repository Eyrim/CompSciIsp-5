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
            DatabaseManagement DB = new DatabaseManagement(connectionSource:"Data Source=:memory:");

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


            PopulateDB("C:\\Users\\h50004271\\Source\\Repos\\CompSciIsp-5\\isp05\\Data.txt", DB);

            //Console.WriteLine(reader.GetValues()[0]);
        }

        /// <summary>
        /// Populates the Database with data from a given file
        /// </summary>
        /// <param name="file">The file containing the data</param>
        static void PopulateDB(string file, DatabaseManagement DB)
         {
            List<string> dataFromFile = Data.ReadFile(filePath: file);
            Console.WriteLine("Data Grabed");

            // Assign each value to the correct entry in the database

            // Build query

            string query = ""; // "INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract) VALUES(";
            List<string> lineToInsert = new List<string>();

            for (int i = 0; i < dataFromFile.Count - 1; ++i)
            {
                query = "INSERT INTO authors(au_id, au_lname, au_fname, au_phone, au_address, au_city, au_state, au_zip, au_contract) VALUES(";
                lineToInsert = dataFromFile[i].Split("|").ToList();
                for (int j = 0; j < lineToInsert.Count; j++)
                {
                    query += lineToInsert[j];
                    query += ",";
                }

                query = query.Remove(query.Length - 1);

                query += ");";

                DB.SendNonQuery(query);
            }

            

            // Profit question mark
         }
     }
 }