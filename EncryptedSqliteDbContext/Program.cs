using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EncryptedSqliteDbContext
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lets test the encrypted database with some example code.

            // Insert new data into the database and save
            using (var db = new MyDbContext())
            {
                db.Database.EnsureCreated();

                db.TestEntities.Add(new TestEntity() { Key = db.TestEntities.Count() + 1, X = (int)DateTime.Now.Ticks });

                db.SaveChanges();
            }

            // Read the data from the database
            using (var db = new MyDbContext())
            {
                db.Database.EnsureCreated();
                foreach (var te in db.TestEntities)
                {
                    Console.WriteLine(te.Key + ": " + te.X);
                }
            }

            // Execute a custom query against the database
            using (var db = new MyDbContext())
            {
                var conn = db.Database.GetDbConnection();
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    // TestEntities is the table that is used in the program.
                    command.CommandText = "SELECT * FROM TestEntities;";

                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetInt32(0) + ": " + reader.GetInt32(1));
                        }
                    }

                    reader.Close();
                }
            }


            Console.ReadLine();

        }

        public static string GetProjectRootPath([CallerFilePath] string sourceFilePath = "")
        {
            // Get the path of this file. This allows us to store
            // the SQLite database files in the same directory as the 
            // source code files, rather than in PROJECT/bin/debug or
            // something similar.
            return Path.GetDirectoryName(sourceFilePath);
        }
    }
}
