using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EncryptedSqliteDbContext
{
    // This class is the example entity that we will store in our
    // encrypted SQLite database for testing.
    public class TestEntity
    {
        [Key]
        public int Key { get; set; }

        public int X { get; set; }
    }
}
