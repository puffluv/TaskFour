using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace TaskFour
{
    public class DatabaseManager
    {
        private string connectionString;

        // Конструктор с использованием значения по умолчанию
        public DatabaseManager(string connectionString = "Data Source=C:\\Users\\puffluv\\source\\repos\\TaskFour\\TaskFour\\StudentsDatabase.db;Version=3;")
        {
            this.connectionString = connectionString;
        }
        public void InsertStudent(Student student)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Удаление из последовательности для обеспечения корректного инкремента ID
                        using (var deleteSequenceCommand = new SQLiteCommand("DELETE FROM sqlite_sequence WHERE name='Students'", connection))
                        {
                            deleteSequenceCommand.ExecuteNonQuery();
                        }

                        // Вставка новой записи
                        using (var command = new SQLiteCommand("INSERT INTO Students (Name, PhysicsGrade, MathGrade) VALUES (@Name, @PhysicsGrade, @MathGrade)", connection))
                        {
                            command.Parameters.AddWithValue("@Name", student.Name);
                            command.Parameters.AddWithValue("@PhysicsGrade", student.PhysicsGrade);
                            command.Parameters.AddWithValue("@MathGrade", student.MathGrade);

                            command.ExecuteNonQuery();
                        }

                        // Если все успешно, фиксируем транзакцию
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Если произошла ошибка, откатываем транзакцию
                        transaction.Rollback();
                        Console.WriteLine($"Ошибка при вставке данных: {ex.Message}");
                    }
                }
            }
        }

        public void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Students (Id INTEGER PRIMARY KEY, Name TEXT, PhysicsGrade INTEGER, MathGrade INTEGER)", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Students";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student();

                            // Проверка наличия поля "Id" в результирующем наборе
                            if (!reader.IsDBNull(reader.GetOrdinal("Id")))
                            {
                                student.Id = Convert.ToInt32(reader["Id"]);
                            }

                            // Проверка наличия поля "Name" в результирующем наборе
                            if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                            {
                                student.Name = reader["Name"].ToString();
                            }

                            // Проверка наличия поля "PhysicsGrade" в результирующем наборе
                            if (!reader.IsDBNull(reader.GetOrdinal("PhysicsGrade")))
                            {
                                student.PhysicsGrade = Convert.ToInt32(reader["PhysicsGrade"]);
                            }

                            // Проверка наличия поля "MathGrade" в результирующем наборе
                            if (!reader.IsDBNull(reader.GetOrdinal("MathGrade")))
                            {
                                student.MathGrade = Convert.ToInt32(reader["MathGrade"]);
                            }

                            students.Add(student);
                        }

                    }
                }
            }

            return students;
        }

        public void AddStudent(string name, int physicsGrade, int mathGrade)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("INSERT INTO Students (Name, PhysicsGrade, MathGrade) VALUES (@Name, @PhysicsGrade, @MathGrade)", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PhysicsGrade", physicsGrade);
                    command.Parameters.AddWithValue("@MathGrade", mathGrade);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Student> ReadStudents()
        {
            List<Student> students = new List<Student>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Students", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = Convert.ToString(reader["Name"]);
                            int physicsGrade = Convert.ToInt32(reader["PhysicsGrade"]);
                            int mathGrade = Convert.ToInt32(reader["MathGrade"]);

                            students.Add(new Student { Id = id, Name = name, PhysicsGrade = physicsGrade, MathGrade = mathGrade });
                        }
                    }
                }
            }

            return students;
        }
        public void UpdateStudent(int id, string name, int physicsGrade, int mathGrade)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("UPDATE Students SET Name = @Name, PhysicsGrade = @PhysicsGrade, MathGrade = @MathGrade WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PhysicsGrade", physicsGrade);
                    command.Parameters.AddWithValue("@MathGrade", mathGrade);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteStudent(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("DELETE FROM Students WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
