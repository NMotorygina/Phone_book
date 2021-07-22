using System;
using Microsoft.Data.Sqlite;

namespace ConsoleApp1
{
    public static class CLI
    {
        public static void Start(SqliteConnection db)
        {
            db.Open();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t Телефонный справочник");
            Console.ResetColor();
            var sql = "select * from table_phonebook;";
            var command = new SqliteCommand {Connection = db, CommandText = sql};
            var reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetValue(0);
                    var first_name = reader.GetValue(1);
                    var last_name = reader.GetValue(2);
                    var phone_number = reader.GetValue(3);
                    Console.WriteLine($"{id}. {first_name} {last_name} - {phone_number}");
                }
            }
            else
            {
                Console.WriteLine("Нет данных!!!");
            }
            db.Close();
        }
        public static string SelectAction(SqliteConnection db)
        {
            db.Open();
            var sql = "select * from table_phonebook;";
            var command = new SqliteCommand {Connection = db, CommandText = sql};
            var reader = command.ExecuteReader();
            string select;
            
            if (reader.HasRows)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Выберите дальнейшее действие:");
                Console.WriteLine("1 - Добавить контакт");
                Console.WriteLine("2 - Удалить контакт");
                Console.WriteLine("3 - Редактировать контакт");
                Console.WriteLine("Любая другая клавиша - Выход из программы");
                Console.ResetColor();
                var temp = Console.ReadLine();
                select = "has_rows" + temp;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Выберите дальнейшее действие:");
                Console.WriteLine("1 - Добавить контакт");
                Console.WriteLine("Любая другая клавиша - Выход из программы");
                Console.ResetColor();
                var temp = Console.ReadLine();
                select = "has_no_rows" + temp;
            }
            db.Close();
            return select;
        }
        public static string AddContact()
        {
            string first_name, last_name, phone_number;
            
            Console.Write("Введите имя - ");
            first_name = Console.ReadLine();
            Console.Write("Введите фамилию - ");
            last_name = Console.ReadLine();
            Console.Write("Введите телефон - ");
            phone_number = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Контакт добавлен в справочник!");
            Console.ResetColor();
            return $"insert into table_phonebook (first_name, last_name, phone_number) values ('{first_name}', '{last_name}', '{phone_number}')";
        }
        public static int CheckId(SqliteConnection db)
        {
            int id;
            string input;
            
            Console.Write("Введите ID контакта - ");
            input = Console.ReadLine();
            
            db.Open();
            if (!Int32.TryParse(input, out id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка! Введите число!");
                Console.ResetColor();
                id = CheckId(db);
            }
            else
            {
                var sql = $"select * from table_phonebook where id = {id};";
                var command = new SqliteCommand {Connection = db, CommandText = sql};
                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка! Контакт с таким ID не существует!");
                    Console.ResetColor();
                    id = CheckId(db);
                }
            }
            db.Close();
            
            return id;
        }
        public static string DeleteContact(int id)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Контакт удален из справочника!");
            Console.ResetColor();
            return $"delete from table_phonebook where id = {id};";
        }
        public static string UpdateContact(int id)
        {
            string temp, sql;
            
            Console.WriteLine("Какие данные изменить:");
            Console.WriteLine("1 - Имя");
            Console.WriteLine("2 - Фамилия");
            Console.WriteLine("3 - Номер телефона");
            var select = Console.ReadLine();
            
            switch (select)
            {
                case "1":
                    Console.Write("Введите имя - ");
                    temp = Console.ReadLine();
                    sql = $"update table_phonebook set first_name = '{temp}' where id = {id};";
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Контакт изменен!");
                    Console.ResetColor();
                    break;
                case "2":
                    Console.Write("Введите фамилию - ");
                    temp = Console.ReadLine();
                    sql = $"update table_phonebook set last_name = '{temp}' where id = {id};";
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Контакт изменен!");
                    Console.ResetColor();
                    break;
                case "3":
                    Console.Write("Введите номер телефона - ");
                    temp = Console.ReadLine();
                    sql = $"update table_phonebook set phone_number = '{temp}' where id = {id};";
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Контакт изменен!");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка! Вы ввели неверные данные!");
                    Console.ResetColor();
                    sql = UpdateContact(id);
                    break;
            }
            
            return sql;
        }
    }
}