# .NET-Core-Encrypted-SQLite-DbContext
.NET Core encrypted SQLite DbContext

This repository contains an example of how a SQLite DbContext can be encrypted with SQLCipher. 
SQLCipher is provided by Zetetic (https://www.zetetic.net) and licensed under a BSD style license.
However, Zetetic does not provided pre-built libraries. The built libraries in this project are 
provided by Eric Sink in the repository SQLitePCL.raw (https://github.com/ericsink/SQLitePCL.raw).
This repository is built targeting .NET Core 3, but the same code works for .NET Core 2. However,
the dependencies are slightly different and should be inspected below. 

The code in this project is simple so that the implementation details will be as clear as possible.
For example, it may not be good practice to store the password for the database hardcoded in the
DbContext class. 

## How it Works

| File		     | Description																			                                   |
|----------------|-------------------------------------------------------------------------------------------------------------------------|
| Program.cs     | Contains examples on how data can be read and added. Additionally custom SQL queries.								   |
| MyDbContext.cs | Contains the SQLCipher implementation of a DbContext. Heavily commented and the most important part of this repository. |
| TestEntity.cs  | Contains the class definition of an entity that will be stored in the database.										   |


## Dependencies

### .NET Core 2

| Dependency                                | Version | License    |
|-------------------------------------------|---------|------------|
| Microsoft.EntityFrameworkCore.Sqlite.Core | 2.2.6   | Apache-2.0 |
| SQLitePCLRaw.bundle_sqlcipher             | 1.1.14  | Apache-2.0 |

### .NET Core 3

| Dependency                                | Version					| License    |
|-------------------------------------------|---------------------------|------------|
| Microsoft.EntityFrameworkCore.Sqlite.Core | 3.0.0-preview8.19405.11   | Apache-2.0 |
| SQLitePCLRaw.bundle_e_sqlcipher           | 2.0.1  					| Apache-2.0 |

