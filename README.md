# ESY-MVC

This is an ASP.NET MVC project for product management. It consists of authentication, database migrations, user roles, CRUD functionality, audit functionality and WEB API for product details.

## Requirements:
- Microsoft SQL Server.


## Test data
Project comes with seed data for testing which is available in the "Data" folder, in the "DataContext.cs" file.

### The seed data includes the following user accounts and example products:
#### Admin user: 
- username: Admin
- password: Admin

#### Regular user:
- username: User
- password: User

### To get the seed test data:
 - Through Package Manager Console open the project directory (cd .\ESY-MVC);
 - Run 'dotnet build';
 - Run 'dotnet ef database update'. (if dotnet ef is not installed, run: 'dotnet tool install --global dotnet-ef')


## Usage
Set the desired project as a startup project (ESY-MVC or ESY-API).

### ESY-MVC
Use the authentication form to log in with your username and password. If you are an Administrator, you can perform CRUD operations on products. If you are a Regular User, you can only view the product records.
### ESY-API 
Use the get request to see 10 newest product modifications. To set certain date, use this format: mm/dd/yyyy h:mm:ss tt".
