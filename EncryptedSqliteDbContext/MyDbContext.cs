using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EncryptedSqliteDbContext
{
    public class MyDbContext : DbContext
    {
        // A DbSet for the entity framework.
        public DbSet<TestEntity> TestEntities { get; set; }

        public MyDbContext()
        {
            // The password that will be used to encrypt the SQLite 
            // database. Note that this value should be validated against
            // SQL injection attacks, but since we trust ourselves, this 
            // step can be ommitted. Additionally, if the password contains
            // spaces, then these must be accounted for by inserting '' 
            // around the password. For example:
            // var myDbPassword = "'Hello World'";
            var myDbPassword = "'Hello World'";

            // Open a connection to the database. Note that the connection
            // string has been provided in the method "OnConfiguring".

            var conn = Database.GetDbConnection();
            conn.Open();

            // To enable encryption and decryption the following command
            // must be executed first, which is why it might as well be included 
            // in the DbContext constructor. The command specifies the 
            // encryption key and other encryption related parameters. The 
            // parameters do have default values, but they are now explicitly 
            // set. The meaning of the parameters can be found here: 
            // https://www.zetetic.net/sqlcipher/sqlcipher-api/
            // Please note that the used encryption library is not an official
            // Zetetic build. 

            using (var command = conn.CreateCommand())
            {
                // Give the values for the command. It is important to know these
                // parameter values in case you want to inspect the database in
                // an external database viewer, such as DB Browser for SQLite.

                // IMPORTANT 

                // .NET Core 3
                // The default pragmas for SQLCipher 4 are:
                // PRAGMA kdf_iter = 256000;
                // PRAGMA cipher_page_size = 4096;
                // PRAGMA cipher_kdf_algorithm = PBKDF2_HMAC_SHA512;
                // PRAGMA cipher_hmac_algorithm = HMAC_SHA512;

                // .NET Core 2
                // The default pragmas for SQLCipher 3 are:
                // PRAGMA kdf_iter = 64000;
                // PRAGMA cipher_page_size = 1024;
                // PRAGMA cipher_kdf_algorithm = PBKDF2_HMAC_SHA1;
                // PRAGMA cipher_hmac_algorithm = HMAC_SHA1;

                // The lines above are valid queries and can be used to
                // edit the value. NOTE: Some values could not be edited
                // in testing. In all cases where the database could not 
                // be accessed from an external program (in this case: 
                // DB Browser for SQLCipher) some of the values had stayed
                // as the default values (all values and the password has
                // to be correct to be able to view the data in the database).
                // If you should change these defaults, then you should also 
                // confirm that they work with an external SQLCipher program as
                // the C# code will always be able to access the database. This
                // is because the C# code "knows" which parameters can be
                // changed and which can't. The same does not necessarily apply
                // to your database viewer program.

                // .NET Core 3 values are more secure than .NET Core 2, but both should be fine.

                command.CommandText = "PRAGMA key = " + myDbPassword + ";";

                // Example CommandText where other parameters than the key
                // are changed (works at least in the .NET Core 3 version,
                // probably also in .NET Core 2):

                //command.CommandText = "PRAGMA key = " + myDbPassword + ";" +
                //                      "PRAGMA kdf_iter = 100000;" + 
                //                      "PRAGMA cipher_page_size = 2048;";

                // NOTE: TO ACCESS THE DATABASE YOU MUST PROVIDE THE SAME VALUES
                // THAT HAVE PREVIOUSLY BEEN SET! 

                // Execute the command.
                command.ExecuteNonQuery();
            }


            // Uncomment the code below to change the database password.
            // The password in the code above should after running this match
            // with the new password. THIS IS NOT THE BEST PLACE TO KEEP THIS
            // CODE!

            //var newDbPassword = "GoodbyeWorld";
            //using (var command = conn.CreateCommand())
            //{
            //    // Give the values for the command. It is important to know these
            //    // parameter values in case you want to inspect the database in
            //    // an external database viewer, such as DB Browser for SQLite.
            //    command.CommandText = "PRAGMA rekey = " + newDbPassword + ";";

            //    // Execute the command.
            //    command.ExecuteNonQuery();
            //}

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the SQLite database and choose some path for the database.
            // The path can be anything, but here it is set to default to the same
            // directory in which the source code resides.

            var dbPath = Program.GetProjectRootPath() + "/" + "TestDatabase.db";
            optionsBuilder.UseSqlite("Data Source=" + dbPath);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create the models and map them to tables

            modelBuilder.Entity<TestEntity>().ToTable("TestEntities", "Test");
            modelBuilder.Entity<TestEntity>(entity =>
            {
                entity.HasKey(e => e.Key);
            });
        }
    }
}
