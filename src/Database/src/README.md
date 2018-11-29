![alt text](https://fablecode.visualstudio.com/_apis/public/build/definitions/9e9640ec-37b8-4d8b-8cb2-19c074a1fa41/7/badge?maxAge=0 "Visual studio team services build status")  

# Sql Database
For storing and retrieving yugioh deck and card related data.

## Installing
```
 $ git clone https://github.com/fablecode/dueltank.git
```
1. Build the database project 'dueltank.database.sln'
2. Publish database to sql server

## Built With
* [SSDT VS2017 Database Project](https://visualstudio.microsoft.com/vs/features/ssdt/)
* [SSDT DACPAC](https://docs.microsoft.com/en-us/sql/relational-databases/data-tier-applications/data-tier-applications?view=sql-server-2017)

## DACPAC
A data-tier application (DAC) is a logical database management entity that defines all SQL Server objects - such as tables, views, and instance objects - associated with a user's database. It is a self-contained unit of SQL Server database deployment that enables data-tier developers and DBAs to package SQL Server objects into a portable artifact called a DAC package, or .dacpac file.

## License
This project is licensed under the MIT License - see the [LICENSE.md](/LICENSE) file for details.
