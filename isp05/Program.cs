using System;
using System.Collections.Generic;

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

            DB.SendNonQuery("CREATE TABLE TestTable(TestField1 VARCHAR(20), TestField2 VARCHAR(20));");
            
            
            /*DB.InitField(FieldName:"TestField1", TableName:"TestTable",
                FieldType:"VARCHAR(20)", IsPrimaryKey:false,
                IsNullable:true, IsAutoIncrement:false)*/;

            Dictionary<string, string> values = new Dictionary<string, string>();
            
            values.Add(key:"TestField1", "thing");
            values.Add(key:"TestField2", "thing2");

            DB.InitListOfRecords(values, "TestTable");
            
            Console.WriteLine(DB.SendQuery("SELECT * FROM TestTable;"));
        }
    }
}