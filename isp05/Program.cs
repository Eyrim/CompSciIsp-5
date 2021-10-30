using System;
using System.Collections.Generic;
using System.Data.SQLite;

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
             DatabaseManagement DB = new DatabaseManagement(connectionSource:"Data Source=:memory:");

             DB.SendNonQuery(@"
             CREATE TABLE authors
             au_id VARCHAR(20) PRIMARY KEY NOT NULL
             au_lname VARCHAR(20) NOT NULL
             au_fname VARCHAR(20) NOT NULL 
             phone VARCHAR(20) NOT NULL
             address VARCHAR(20) NOT NULL
             city VARCHAR(20) NOT NULL
             au_state VARCHAR(20) NOT NULL
             zip VARCHAR(20) NOT NULL
             contract BOOLEAN NOT NULL;");

             List<string> dataFromFile = Data.ReadFile(filePath: "Data.txt");
             
             

             //Console.WriteLine(reader.GetValues()[0]);
         }

         static void PopulateDB()
         {
             
         }
     }
 }