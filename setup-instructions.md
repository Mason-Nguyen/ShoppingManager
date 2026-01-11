# ShoppingManager Setup Instructions

## Prerequisites

### Backend Requirements
- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Frontend Requirements
- Node.js (v16 or higher)
- npm or yarn
- TypeScript support (included in dependencies)

## Backend Setup

1. **Navigate to backend directory:**
   ```bash
   cd ShoppingManager/backend/ShoppingManager.API
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Update database connection string (if needed):**
   - Edit `appsettings.json` and `appsettings.Development.json`
   - Update the `DefaultConnection` string to match your SQL Server setup

4. **Create and seed the database:**
   ```bash
   dotnet ef database update
   ```
   
   If Entity Framework tools are not installed:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

5. **Run the backend:**
   ```bash
   dotnet run
   ```

   The API will be available at:
   - HTTP: http://localhost:5000
   - HTTPS: https://localhost:7000
   - Swagger UI: https://localhost:7000/swagger

## Frontend Setup

1. **Navigate to frontend directory:**
   ```bash
   cd ShoppingManager/frontend
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm start
   ```

   The React app will be available at: http://localhost:3000

## TypeScript Configuration

The frontend is built with TypeScript for better type safety and developer experience:

- **Type Definitions:** All interfaces and types are defined in `src/types/index.ts`
- **Strict Mode:** TypeScript strict mode is enabled for better code quality
- **Component Types:** All React components are properly typed with TypeScript
- **API Types:** API calls are fully typed with request/response interfaces

## Admin User Creation Features

The admin panel now includes comprehensive user management capabilities:

### Enhanced User Creation
- **Role Selection:** Admin can assign any role (Admin, Requester, Approver, Receiver, Purchase, User)
- **Password Validation:** Strong password requirements with uppercase, lowercase, numbers, and special characters
- **Account Status:** Can create users as active or inactive
- **Role Descriptions:** Each role shows its capabilities and permissions
- **Form Validation:** Real-time validation with helpful error messages

### User Management Features
- **User Statistics Dashboard:** Shows total users, active users, administrators, and inactive users
- **Role-based Color Coding:** Visual indicators for different user roles
- **Account Status Toggle:** Quick activate/deactivate user accounts
- **Login History Tracking:** View detailed login/logout history for each user
- **Bulk Operations:** Edit, delete, and manage multiple users efficiently

### API Endpoints for Admin

#### User Creation
- `POST /api/auth/create-user` - Create user with any role (Admin only)
- `GET /api/auth/roles` - Get available roles with descriptions (Admin only)

#### Enhanced Validation
- Email format validation
- Password strength requirements (6+ chars, uppercase, lowercase, number, special char)
- Role validation against available roles
- Duplicate email prevention

## Default Admin Account

- **Email:** admin@shoppingmanager.com
- **Password:** Admin123!

## Testing the Application

1. **Login as Admin:**
   - Use the default admin credentials
   - Navigate to Admin Panel to manage users

2. **Create Test Users:**
   - Create users with different roles (Requester, Approver, etc.)
   - Test role-based access control

3. **Test Features:**
   - User login/logout
   - Password change
   - User management (Admin only)
   - Login history tracking

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `POST /api/auth/change-password` - Change password
- `GET /api/auth/me` - Get current user info

### User Management (Admin only)
- `GET /api/users` - Get all users
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `PATCH /api/users/{id}/toggle-status` - Toggle user status
- `GET /api/users/{id}/login-history` - Get user login history

## User Roles

1. **Admin** - Full system access, user management
2. **Requester** - Can create shopping requests
3. **Approver** - Can approve/reject requests
4. **Receiver** - Can receive and confirm deliveries
5. **Purchase** - Can process approved requests
6. **User** - Basic user access

## Security Features

- JWT token-based authentication
- Role-based authorization with TypeScript enums
- Password hashing with BCrypt
- Login/logout tracking with IP addresses
- Failed login attempt logging
- CORS protection
- Type-safe API calls with TypeScript

## Database Schema

### Users Table
- Id, Email, PasswordHash, FirstName, LastName
- Role, IsActive, CreatedAt, LastLoginAt, LastLoginIP
- ResetToken, ResetTokenExpiry

### LoginHistories Table
- Id, UserId, IPAddress, LoginTime, LogoutTime
- UserAgent, IsSuccessful

## Troubleshooting

### Backend Issues
- Ensure SQL Server is running
- Check connection string in appsettings.json
- Verify .NET 8 SDK is installed
- Run `dotnet ef database update` if database issues occur

### Frontend Issues
- Ensure Node.js v16+ is installed
- Clear npm cache: `npm cache clean --force`
- Delete node_modules and reinstall: `rm -rf node_modules && npm install`
- Check if backend is running on correct port (7000 for HTTPS)

### CORS Issues
- Ensure backend CORS policy allows frontend origin (localhost:3000)
- Check browser console for CORS errors
- Verify API base URL in frontend matches backend URL