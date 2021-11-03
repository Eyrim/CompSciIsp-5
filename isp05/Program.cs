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
             DatabaseManagement DB = InitDB();

             List<Author> authors = GetAllRecords(DB);

             string idToSearch = Console.ReadLine();
                 
                 
             for (int i = 0; i < authors.Count; i++)
             {
                 if (authors[i].au_id == idToSearch)
                 {
                     Console.WriteLine("Author_ID: {authors[i].au_id}");
                     Console.WriteLine("Author_LastName: {authors[i].au_lname}");
                     Console.WriteLine("Author_FirstName: {authors[i].au_fname}");
                     Console.WriteLine("Author_Phone: {authors[i].phone}");
                     Console.WriteLine("Author_Address: {authors[i].address}");
                     Console.WriteLine("Author_State: {authors[i].state}");
                     Console.WriteLine("Author_Zip: {authors[i].zip}");
                     Console.WriteLine("Author_Contract: {authors[i].contract}");
                 }
             }
         }

         static List<Author> GetAllRecords(DatabaseManagement DB)
         {
             // Get all data and put into 2d list
             SQLiteDataReader reader = DB.SendQuery("SELECT * FROM authors;");
             List<Author> authors = new List<Author>();
             List<string> newList = new List<string>();

             int c = 0;
             
             for (int i = 0; i < reader.VisibleFieldCount; i++)
             {
                 if (reader.HasRows && reader.Read())
                 {
                     while (true)
                     {
                         // We can just wait for this to break, readers are really weird
                         try
                         {
                             newList.Add(reader.GetString(c));
                             c++;
                         }
                         
                         catch
                         {
                             break;
                         }
                     }

                     c = 0;
                     authors.Add(new Author(newList));
                     newList.Clear();
                 }
             }

             return authors;
         }
 
         static DatabaseManagement InitDB()
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


            DB = PopulateDB(@"C:\Users\marfx\Desktop\CompSciIsp-5\isp05\Data.txt", DB);

            return DB;
         }

        /// <summary>
        /// Populates the Database with data from a given file
        /// </summary>
        /// <param name="file">The file containing the data</param>
        static DatabaseManagement PopulateDB(string file, DatabaseManagement DB)
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

            return DB;
        }
     }
 }