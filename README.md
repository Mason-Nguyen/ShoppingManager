# ShoppingManager

A web application built with .NET 8 backend and ReactJS frontend for managing shopping requests with role-based access control.

## Features

- User authentication with JWT claims
- Role-based authorization (Admin, Requester, Approver, Receiver, Purchase, User)
- User management (CRUD operations for Admin)
- Password reset/change functionality
- Login/logout tracking with IP and timestamp
- Responsive UI with Tailwind CSS

## Tech Stack

**Backend:**
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication

**Frontend:**
- ReactJS
- React Router
- Tailwind CSS
- Axios for API calls

## Getting Started

### Backend Setup
1. Navigate to `backend` folder
2. Run `dotnet restore`
3. Update connection string in `appsettings.json`
4. Run `dotnet ef database update`
5. Run `dotnet run`

### Frontend Setup
1. Navigate to `frontend` folder
2. Run `npm install`
3. Run `npm start`

## Default Admin Account
- Email: admin@shoppingmanager.com
- Password: Admin123!