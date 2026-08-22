# Gym Management System

A layered **ASP.NET Core Web API** for managing gym operations, including members, trainers, sessions, membership plans, subscriptions, bookings, attendance, and payments.

The project follows a clean layered architecture to achieve scalability, maintainability, and separation of concerns. It is built on .NET 9 with JWT-based authentication and role-based authorization.

---

## Architecture

```text
GymManagementSystem
│
├── GymManagement            (Presentation / API Layer)
│   ├── Controllers
│   ├── Middleware
│   └── Program.cs
│
├── GymManagement.BLL         (Business Logic Layer)
│   ├── Services
│   ├── DTOs
│   └── Exceptions
│
└── GymManagement.DAL         (Data Access Layer)
    ├── Entities
    ├── Configurations
    ├── Repositories
    └── Migrations
```

Data flows: **API → BLL → DAL → EF Core → SQL Server**

---

## Features

### Authentication & Authorization
- ASP.NET Core Identity for user management
- JWT Bearer authentication
- Role-based authorization (Admin, Trainer, Member)
- Roles and a default admin account seeded on startup

### Domain Entities
- **Member / Trainer** — linked 1:1 with an Identity user account
- **Plan** — membership plans (name, price, duration)
- **Subscription** — a member's active or past subscription to a plan, with server-computed start/end dates
- **Session** — a bookable class run by a trainer
- **Booking** — a member's booking of a session, with capacity and double-booking checks
- **Attendance** — check-in tracking tied to a booking
- **Payment** — payments linked to a member, optionally linked to a subscription

### Business Rules
- Plans cannot be deleted while active subscriptions reference them
- Sessions cannot be created in the past
- Bookings are blocked once a session is full, already booked, or in the past
- Members can only view or manage their own bookings; requesting another member's booking returns 404
- Subscriptions block overlapping active periods for the same member
- Payments allow flexible amounts (discounts/partial payments) and optional subscription linkage

### API Infrastructure
- Centralized exception handling via a global exception handler
- Consistent pagination (`pageNumber`, `pageSize`) across list endpoints
- Structured logging for business rule rejections, authentication events, and entity creation
- Interactive API documentation and testing via Swagger

---

## Technology Stack

**Backend**
- C#, .NET 9
- ASP.NET Core Web API
- Entity Framework Core 9

**Database**
- SQL Server
- EF Core Code First with Fluent API configurations and migrations

**Security**
- ASP.NET Core Identity
- JWT Bearer authentication
- Role-based authorization

**Tools**
- Visual Studio 2022
- Git & GitHub
- Swagger / Swashbuckle

---

## Design Patterns & Principles

- Repository Pattern (generic, not per-entity)
- Unit of Work Pattern
- Dependency Injection
- SOLID principles

---

## Project Structure

```text
GymManagementSystem
│
├── GymManagement          (API / Presentation)
├── GymManagement.BLL      (Business Logic Layer)
├── GymManagement.DAL      (Data Access Layer)
└── GymManagementSolution.sln
```

---

## Installation & Setup

### 1. Clone the repository

```bash
git clone https://github.com/nour136/GymManagementSystem.git
```

### 2. Configure the database connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Apply database migrations

Using Package Manager Console:

```powershell
Update-Database
```

Or using the .NET CLI:

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

Swagger UI will be available at `/swagger` in development mode.

---

## Default Roles

| Role    | Permissions                                            |
|---------|----------------------------------------------------------|
| Admin   | Full system management                                   |
| Trainer | Manage assigned sessions and mark attendance              |
| Member  | Browse plans/sessions, manage own bookings and subscriptions |

A default admin account is seeded on first run:

```
Email:    admin@gym.com
Password: Admin@123456
```

---

## Author

**Nour Yasser Mansour**
.NET Developer

**LinkedIn:**
https://www.linkedin.com/in/nour-yasser-a68474356

**GitHub:**
https://github.com/nour136
