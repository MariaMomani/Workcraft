# WorkCraft - Employee Management System

A full-stack web application for managing employees and tasks within an organization, 
built with ASP.NET MVC 5 and SQL Server.

---

## Features

### Admin
- Live dashboard with KPI cards and productivity charts
- Employee management (add, edit, deactivate)
- Task assignment with full status lifecycle tracking
- Real-time employee availability status monitoring
- Message inbox and notification center
- Company settings management

### Employee
- Personal dashboard with assigned tasks overview
- Task workflow management (Accept / Reject / Complete)
- Real-time availability status visible to admin
- Notification center (new and archived)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET MVC 5, C# |
| ORM | Entity Framework 6 |
| Database | SQL Server |
| Frontend | HTML5, CSS3, Bootstrap 5, Razor Views |
| Scripting | JavaScript, jQuery, AJAX |

---

## Architecture

Built following the **MVC (Model-View-Controller)** architectural pattern with:
- Role-based access control (Admin / Employee)
- AJAX-based real-time notifications without page reloads
- Relational database schema designed from scratch

---

## Getting Started

### Prerequisites
- Visual Studio 2019 or later
- SQL Server
- .NET Framework 4.x

### Setup
1. Clone the repository
   git clone https://github.com/MariaMomani/WorkCraft.git

2. Open the solution in Visual Studio

3. Update the connection string in `Web.config` to point to your SQL Server instance

4. Run the database migrations using Package Manager Console
   Update-Database

5. Build and run the project


---

## Author

**Maria Momani**  
[LinkedIn](https://www.linkedin.com/in/maria-momani) • [GitHub](https://github.com/MariaMomani)
