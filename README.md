# Table of contents
<details>
   <summary>Click here to expand content list.</summary>
   
1. [General information](#1-general-information)
2. [License](#2-license)
3. [System description](#3-system-description)
4. [System requirements](#4-system-requirements)
5. [Supported features](#5-supported-features)
6. [Diagrams](#6-diagrams)
    * [6.1 Sitemap](#61-sitemap)
    * [6.2 ER diagram](#62-er-diagram)
    * [6.3 Sequence diagram](#63-sequence-diagram)
7. [User interface](#7-user-interface)
8. [Setup guide](#8-setup-guide)
    * [8.1 Prerequisites](#81-prerequisites)
    * [8.2 Prepare the database and its tables](#82-prepare-the-database-and-its-tables)
    * [8.3 Install necessary Visual Studio Code extensions](#83-install-necessary-visual-studio-code-extensions)
    * [8.4 Import the application to Visual Studio Code](#84-import-the-application-to-visual-studio-code)
    * [8.5 Configure the application](#85-configure-the-application)
    * [8.6 Run the application](#86-run-the-application)
    * [8.7 Create the admin user](#87-create-the-admin-user)
9. [Contact details](#9-contact-details)
</details>

---

# 1 General information
”News 1.0” was created in Visual Studio Code by Annice Strömberg, 2019, with [Annice.se](https://annice.se) as the primary download location. The script is basically a simple web content management application (WCM) that allows an administrator user to create and edit news entries, as well as edit the administrator credentials. Furthermore, visitors can view published news.

---

# 2 License
Released under the MIT license.

MIT: [http://rem.mit-license.org](http://rem.mit-license.org/), see [LICENSE](LICENSE).

---

# 3 System description
“News 1.0” is built in CSS3, razor HTML5, JavaScript, C# with ASP.NET Core, and Transact SQL using SQL Server as a database management system (DBMS).

Furthermore, the server code (C#) is based on the design pattern model-view-controller (MVC) along with a relational database to handle the different application entities.

---

# 4 System requirements
The script can be run on a server that supports C# ASP.NET Core 2.2, e.g. on Azure. However, I will not go into any details of how you can deploy this application on an Azure server or suchlike as this script is implemented locally. Yet, I will go through the necessary steps to run this web application on your local computer first and foremost.
As mentioned above, this application requires to be built and ran on a .NET Core 2.2 platform. Furthermore, if you are using Visual Studio Code as an IDE, you must have at least the following extensions installed:

  * C#
  * Razor+ (optional, but recommended for more eye-friendly razor code in view).
  
Nevertheless, you can read more about the necessary setup steps under section “8 Setup guide”.

---

# 5 Supported features
The following functions and features are supported by this script:
  * Login system based on sessions.
  * User password encryption (SHA256) with hash and salt.
  * Protection against SQL injections.
  * Protection against cross-site forgery.
  * Full create/read/update/delete (CRUD) functionality for an admin.
  * Responsive design.
  * Client and server side validation.
  * Pagination.
  * Filter function to search entries

---
  
# 6 Diagrams
This section describes and illustrates the different application flows to give you a quick overview of how the application context.

## 6.1 Sitemap
Anonymous visitors have access to the following site pages and actions:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/public-sitemap.png" alt="" width="500">

The administrator user has access to the following site pages and actions:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/admin-sitemap.png" alt="" width="600">

## 6.2 ER diagram
The application is based on the following entity and database table attributes and relationships:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/er-diagram.png" alt="" width="400">

## 6.3 Sequence diagram
The following diagram illustrates an example of how the MVC pattern works for an entry creation request:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/seq-diagram.png" alt="" width="800">
  
---

# 7 User interface
Screenshot of the public start page in desktop vs. responsive view:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-start-desktop.png" alt="" width="450"> <img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-start-responsive.png" alt="" width="200">

Screenshot of the public entry detail page in desktop vs. responsive view:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-entry-details-desktop.png" alt="" width="430"> <img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-entry-details-responsive.png" alt="" width="180">

Screenshot of the admin page to create a new entry in desktop vs. responsive view:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-admin-desktop.png" alt="" width="500"> <img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-admin-responsive.png" alt="" width="180">

Screenshot of the admin page to edit news in desktop vs. responsive view:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-admin-edit-desktop.png" alt="" width="430"> <img src="https://diagrams.annice.se/c-sharp-news-1.0/gui-admin-edit-responsive.png" alt="" width="160">

---

# 8 Setup guide
As this script was created in Visual Studio Code with SQL Server, I will go through the necessary installation steps accordingly.

## 8.1 Prerequisites
  * [Install SQL Server Express](https://www.microsoft.com/sv-se/sql-server/sql-server-downloads)
  * [Install SQL Server Management Studio (SSMS)](https://docs.microsoft.com/sv-se/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
  * [Install .NET Core 3.1 (SDK)](https://dotnet.microsoft.com/download)
  * [Install Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
  
## 8.2 Prepare the database and its tables
1. [Create a database in SQL Server](https://docs.microsoft.com/en-us/sql/relational-databases/databases/create-a-database?view=sql-server-2017#SSMSProcedure).
2. [Create a user login in SQL Server](https://docs.microsoft.com/en-us/sql/relational-databases/databases/create-a-database?view=sql-server-2017#SSMSProcedure).
3. Launch SQL Server.
4. In the unzipped script folder, open the file “DBTables.sql” in SQL Server and change the following code line to match your own created database:

```sql
USE YourDatabaseName
GO
```

5. Execute the SQL file in SQL Server to create the application database tables.

## 8.3 Install necessary Visual Studio Code extensions
6. Launch Visual Studio Code.
7. In the main menu, navigate to View > Extensions (Ctrl+Shift+X).
8. Type “C#” in the extension search field and hit enter.
9. Click on the extension named “C#” and choose to install it.

## 8.4 Import the application to Visual Studio Code'
10. In the main menu bar, navigate to File > Open Folder… (Ctrl+K Ctrl+O).
11. Browse to the unzipped script folder and select to open the subfolder named “MvcNews”. The imported folder will then appear – including the application source code – in the explorer section as below:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/proj-hierarchy.png" alt="" width="300">

## 8.5 Configure the application
12. Once the application is imported into VSC, open the file “appSettings.json” in the explorer section.
13. In the file “appSettings.json”, change the database connection string to suit your own database credentials:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/appsettings1.png" alt="" width="600">

14. In the same file, you can also choose to change the number of entries to be displayed per page before pagination links are displayed:

<img src="https://diagrams.annice.se/c-sharp-news-1.0/appsettings2.png" alt="" width="400">

15. Save the project via main menu option File > Save (Ctrl+S).

## 8.6 Run the application
16. In Visual Studio Code, open a terminal window by navigating to *View > Terminal* via the main menu (Swedish shortcut: Ctrl+Ö).
17. In the open terminal window, type *dotnet run* and hit enter.
18. Once you see a confirmation message in the terminal saying that the application has started, you can navigate to the listening URL. In my case it was displayed in the terminal as: https://localhost:5001

## 8.7 Create the admin user
19. When the application is up and running, you also have to create an admin user as a final step to be able to login and manage the news entries. This is a one time action, which can be done by navigating to the following application URL: https://localhost:<YourListeningPort>/Admin/Create

---

# 9 Contact details
For general feedback related to this script, such as any discovered bugs etc., you can contact me via the following email address: [info@annice.se](mailto:info@annice.se)
