# MusalaSoft_Project_Job_Searching
A .NET Core application for school purposes, namely - project. The application is used to help people find a job using job ads.

Used working environment and tools: 
DOT.NET Core 2.0,
Entity Frameowork Core (EF Core),
Visual Studio 2017/2019,
Microsoft SQL Server Management Studio 18

Set up: 
The app connects to Microsoft SQL Server (.\express) by default. To change it, change the connection string file.
To establish connections to your database go to Visual Studio > Package Manager Console and write the command update-database (make sure that the target project is JobSearching).  
When starting the app, make sure that the target project is JobSearching.

Idea of the app:
Company employees register to the app. They can create Job ads. On the other hand, there are volunteers who are seeking for a job. They reigtser to the site, too. They can view Job ads and sign to some of them.
