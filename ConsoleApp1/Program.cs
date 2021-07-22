using System;
using Microsoft.Data.Sqlite;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            var connection = "Data Source=phonebook.sqlite;Mode=ReadWrite;";
            var db = new SqliteConnection(connection);
            
            string select, sql;
            int id;
            
            CLI.Start(db);
            do
            {
                select = CLI.SelectAction(db);
                
                switch (select)
                {
                    case "has_rows1":
                    case "has_no_rows1":
                        db.Open();
                        sql = CLI.AddContact();
                        var command1 = new SqliteCommand {Connection = db, CommandText = sql};
                        command1.ExecuteNonQuery();
                        db.Close();
                        CLI.Start(db);
                        break;
                    case "has_rows2":
                        id = CLI.CheckId(db);
                        db.Open();
                        sql = CLI.DeleteContact(id);
                        var command2 = new SqliteCommand {Connection = db, CommandText = sql};
                        command2.ExecuteNonQuery();
                        db.Close();
                        CLI.Start(db);
                        break;
                    case "has_rows3":
                        id = CLI.CheckId(db);
                        db.Open();
                        sql = CLI.UpdateContact(id);
                        var command3 = new SqliteCommand {Connection = db, CommandText = sql};
                        command3.ExecuteNonQuery();
                        db.Close();
                        CLI.Start(db);
                        break;
                    default:
                        Console.WriteLine("До свидания...");
                        select = null;
                        break;
                }
            } while (select != null);
        }
    }
}