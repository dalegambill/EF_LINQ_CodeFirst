EF_Linq_CodeFirst, by Dale Gambill, 10/6/2019

This is a Windows 10 desktop program written in C# using Visual Studio Community 2019. It does the following:

- Uses Winforms to create the user interface.

- Demonstrates how to use Entity Framework to create a simple relational database in MS SQL Server v2017. This is done by writing C# code to establish a relational data model using C# classes. This means you "code first" to create the data model as C# classes, and Entity Framework will create the corresponding database tables in SQL Server for you. The other choice is 'database first', which this program does not demonstrate.

- Demonstrates using LINQ to query the database.

- In App.Config, demonstrates the correct connection string for accessing the SQL Server database. You could use a 'local database server', which Entity Framework assumes as a default, but I prefer using SQL Server, since that is Microsoft's mainstream database server. Note: A SQL Server database is only accessed through a server, a local database is local to your application only.

- The related YouTube video (link goes here) gives some tips on installing SQL Server, since this project uses that database server.

  
