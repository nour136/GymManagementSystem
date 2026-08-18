# 🏋️ Gym Management System

A complete **Gym Management System** built using **ASP.NET Core MVC** to automate and manage gym operations including members, trainers, sessions, memberships, plans, and bookings.

The project follows a clean layered architecture to achieve **scalability, maintainability, and separation of concerns**.

---

# 🚀 Features

## 👤 Authentication & Authorization

* User authentication using **ASP.NET Core Identity**
* Role-based authorization
* Secure password hashing
* User and role management

---

## 👥 Member Management

* Add, update, delete, and view members
* Manage member information
* Track memberships and subscriptions
* Handle member attachments

---

## 🏋️ Trainer Management

* Manage trainers
* Assign trainers to sessions
* Maintain trainer information

---

## 📅 Session Management

* Create and manage gym sessions
* Define session capacity
* Assign trainers
* Track session schedules

---

## 💳 Membership Management

* Create membership plans
* Manage subscriptions
* Track member memberships

---

## 📌 Booking System

* Book sessions
* Manage booking operations
* Prevent invalid bookings based on business rules

---

# 🏗️ Architecture

The project is designed using a layered architecture:

```text
GymManagementSystem

│
├── Presentation Layer
│   └── ASP.NET Core MVC
│       ├── Controllers
│       ├── Views
│       └── ViewModels
│
├── Business Logic Layer
│   └── Services
│       ├── Business Rules
│       ├── Application Logic
│       └── DTOs
│
└── Data Access Layer
    └── Entity Framework Core
        ├── DbContext
        ├── Entities
        ├── Configurations
        └── Migrations
```

---

# 🛠️ Technologies Used

## Backend

* C#
* ASP.NET Core MVC
* .NET 9
* Entity Framework Core
* LINQ

## Database

* SQL Server
* Entity Framework Core Code First
* Fluent API
* Migrations

## Frontend

* Razor Views
* HTML5
* CSS3
* Bootstrap
* JavaScript
* jQuery

## Security

* ASP.NET Core Identity
* Role-Based Authorization

## Tools

* Visual Studio 2022
* Git & GitHub
* Swagger
* Postman

---

# 🧩 Design Patterns & Principles

The project applies:

* Repository Pattern
* Unit of Work Pattern
* Specification Pattern
* Dependency Injection
* SOLID Principles
* Object-Oriented Programming (OOP)

---

# 📂 Project Structure

```text
GymManagementSystem

│
├── GymManagement.Presentation
│
├── GymManagement.BLL
│
├── GymManagement.DAL
│
└── GymManagement.sln
```

---

# ⚙️ Installation & Setup

## 1. Clone Repository

```bash
git clone https://github.com/sief-elmenshawi/GymManagementSystem.git
```

---

## 2. Configure Database Connection

Update the connection string inside:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## 3. Apply Database Migration

Using Package Manager Console:

```powershell
Update-Database
```

or using CLI:

```bash
dotnet ef database update
```

---

## 4. Run Application

```bash
dotnet run
```

---

# 🔐 Default Roles

| Role    | Permissions              |
| ------- | ------------------------ |
| Admin   | Full system management   |
| Trainer | Manage assigned sessions |
| Member  | View and book sessions   |

---

# 🎯 Future Improvements

* Build RESTful Web API version
* Add payment integration
* Add notification system using SignalR
* Add dashboard analytics
* Add automated testing

---

#  Author

**Nour Yasser**

.NET Developer

GitHub:


LinkedIn:

