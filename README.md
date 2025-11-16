# JWT Authentication with .NET

This repository contains a complete implementation of **JWT Authentication** in **.NET 9**, covering basic login, secure endpoints, role-based authorization, and refresh token flows. The project uses **.NET Web API**, **Entity Framework Core**, and **code-first migrations**.

## Introduction
This project demonstrates how to implement JWT-based authentication in a .NET Web API. It covers user registration, login, secure endpoints, role-based access control, and refresh token handling. The repository follows **best practices**, including **password hashing**, **DTOs**, **service layer refactoring**, and **code-first database migrations**.

---

## Features
- User registration and login
- Password hashing for secure storage
- JWT token generation and validation
- Secure API endpoints using `[Authorize]` attribute
- Role-based authorization
- Refresh token implementation
- Entity Framework Core integration with code-first migrations
- Service layer architecture for clean code separation

---

## Project Structure
- **Entities:** Define the `User` model and other database entities.
- **DTOs:** Data Transfer Objects for requests and responses.
- **Controllers:** Handles API endpoints like authentication and user management.
- **Services:** Contains business logic for authentication, token generation, and user management.
- **Data:** Database context and migrations using Entity Framework Core.
- **Helpers:** Utilities for password hashing, JWT generation, and token validation.

---

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- SQL Server or any compatible relational database
- IDE: Visual Studio / VS Code

### Installation
1. Clone the repository:
```bash
git clone https://github.com/MuhammadAbdulMoiz/JWT-Authentication-with-.NET.git
```

2. Navigate to the project folder:
```bash
cd JWT-Authentication-with-.NET
```

3. Configure the database connection in appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLExpress;Database=JwtAuthDb;Trusted_Connection=True;TrustServerCertificate=True;"
},
"JwtSettings": {
  "SecretKey": "YourSecretKeyHere",
  "Issuer": "YourIssuer",
  "Audience": "YourAudience"
}
```

4. Apply EF Core migrations:

```bash
dotnet ef database update
```

5. Run the API:
```bash
dotnet run
```

## Usage

You can test the API using **Postman** or **Swagger UI**:

1. **Register a new user**  
   Endpoint: `/api/auth/register`  
   Method: `POST`  

2. **Login**  
   Endpoint: `/api/auth/login`  
   Method: `POST`  
   - Returns a JWT token  

3. **Access protected endpoints**  
   - Include the JWT in the request header:  
     ```
     Authorization: Bearer <token>
     ```

4. **Refresh expired tokens**  
   Endpoint: `/api/auth/refresh`  
   Method: `POST`  

---

## Endpoints Overview

| Endpoint             | Method | Description                              |
|---------------------|--------|------------------------------------------|
| `/api/auth/register` | POST   | Register a new user                       |
| `/api/auth/login`    | POST   | Login and receive JWT token               |
| `/api/auth/refresh`  | POST   | Refresh access token using refresh token |
| `/api/users`         | GET    | Protected endpoint, requires `[Authorize]` |
| `/api/users/{id}`    | GET    | Get user details (role-based access)     |

---

## Technologies

- **.NET Web API**  
- **Entity Framework Core**  
- **SQL Server** (or compatible database)  
- **JWT (JSON Web Tokens)**  
- **Scalar** (API testing) 

