# dotnet-6-crud-api

.NET 6.0 - LOGIN AND CRUD USERS

CARA MENJALANKAN FILE

1. Set up database di file appsettings.json :
   "ConnectionStrings": {
   "WebApiDatabase": "server=your_local; database=your_database; user=your_username; password=your_password"
   },
   Run the command 'dotnet ef database update' from the project root folder to execute the EF Core migrations and create the database and tables in MySQL.
2. Jalankan dotnet run
3. Lalu jalankan di browser / postman atau yang lainnya :
   - POST https://localhost:5001/users/authenticate
   - POST https://localhost:5001/users/logout
   - GET https://localhost:5001/users/
   - GET https://localhost:5001/users/{id}
   - POST https://localhost:5001/users/
   - PUT https://localhost:5001/users/{id}
   - PUT https://localhost:5001/users/{id}/changepassword
   - DELETE https://localhost:5001/users/{id}
