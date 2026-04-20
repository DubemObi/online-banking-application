# Banking Application - Features Summary

## 📋 Complete Feature List

### 🔐 Authentication & User Management

- **User Registration** - Email-based registration with verification
  - Email verification via link
  - Password hashing
  - User profile creation (First Name, Last Name, Phone, DOB, Address)
- **User Authentication**
  - JWT Token-based login
  - Session logout
  - User account deletion
- **User Administration**
  - View all users (Admin only)
  - View user details
  - Update user information

---

### 💳 Bank Account Management

- **Account Operations**
  - Create new bank accounts
  - View all accounts (Admin) or specific accounts
  - Update account details
  - Delete accounts (Admin)
- **Account Attributes**
  - Account Number
  - Account Name
  - Account Status (Active/Inactive)
  - Account Type (Savings, Checking, etc.)
  - User association

---

### 🎫 Card Management

- **Card Operations**
  - Issue new cards (Admin)
  - View all cards (Admin view with hashed data)
  - View card details
  - Update card information (Admin)
  - Delete cards
- **Card Types Supported**
  - Debit Cards
  - Credit Cards
- **Card Brands**
  - Visa
  - Mastercard (and others supported)
- **Security Features**
  - Card number hashing (PCI compliant)
  - CVV hashing
  - PIN hashing
  - Last 4 digits visibility

---

### 📝 Card Request Management

- **Request Workflow**
  - Users submit card requests
  - Admins review requests
  - Request approval/rejection
  - Automatic card creation on approval
- **Request Statuses**
  - Pending
  - Approved
  - Rejected

---

### 💰 Loan Management

- **Loan Operations**
  - Create loans (Admin)
  - View all loans (Admin)
  - View specific loan details
  - Update loan terms
  - Delete loans (Admin)
- **Loan Parameters**
  - Principal Amount
  - Duration (in months)
  - Interest calculation ready
  - User association

---

### 📋 Loan Request Management

- **Request Workflow**
  - Users submit loan requests
  - Admins review applications
  - Request approval/rejection with terms
  - Automatic loan creation on approval
- **Loan Request Details**
  - Requested Principal Amount
  - Requested Duration
  - Request Status (Pending/Approved/Rejected)

---

### 💸 Transaction Management

- **Transaction Types**
  - Deposits
  - Withdrawals
- **Transaction Features**
  - Timestamp tracking
  - Transaction reference
  - Description/notes
  - Status (Completed/Failed/Pending)
  - Account balance updates
- **Transaction History**
  - View all transactions
  - View specific transaction details
  - Complete audit trail

---

### 👥 Role-Based Access Control (RBAC)

- **Role Management** (Admin only)
  - Create new roles
  - View all roles
  - View specific roles
  - Update role names
  - Delete roles
- **Role Assignment**
  - Assign roles to users
  - Multiple roles per user capability
- **Built-in Roles**
  - Admin (Full system access)
  - User (Standard user access)
  - Manager (Partial admin features)

---

## 🎯 Use Cases & Workflows

### 1. **New User Onboarding**

```
1. User registers → email verification
2. User logs in → receives JWT token
3. Admin can assign roles if needed
```

### 2. **Bank Account Setup**

```
1. User creates account
2. Admin reviews/activates
3. User can manage account
```

### 3. **Card Issuance**

```
1. User requests card
2. Admin reviews request
3. Card is created automatically
4. User receives card details
```

### 4. **Loan Application**

```
1. User submits loan request
2. Admin reviews application
3. Loan is created with terms
4. User can view loan status
```

### 5. **Money Transactions**

```
1. User deposits → balance increases
2. User withdraws → balance decreases
3. Transaction history maintained
4. Email confirmation sent
```

---

## 🔒 Security Features

### Authentication & Authorization

- ✅ JWT Token-based authentication
- ✅ Role-based access control
- ✅ Admin-only operations protected
- ✅ User-specific data isolation

### Data Security

- ✅ Password hashing (ASP.NET Identity)
- ✅ Card number hashing (PCI DSS compliance)
- ✅ CVV hashing
- ✅ PIN hashing
- ✅ Email verification before access

### API Protection

- ✅ Rate limiting (6 requests per minute)
- ✅ Input validation
- ✅ Model state validation
- ✅ Error handling & logging

---

## 📊 Database Entities

### Core Entities

1. **ApplicationUser** - Extended Identity User
2. **BankAccount** - Customer bank accounts
3. **Card** - Issued cards
4. **CardRequest** - Card application requests
5. **Loan** - Active loans
6. **LoanRequest** - Loan applications
7. **Transaction** - Account transactions

### Relationships

```
User → BankAccount (1:Many)
User → Card (1:Many)
User → Loan (1:Many)
BankAccount → Card (1:Many)
BankAccount → Transaction (1:Many)
BankAccount → Loan (1:Many)
```

---

## 🚀 Quick Start for Testing

### Setup

1. Clone/Download the project
2. Configure database connection in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start the API: `dotnet run`

### Import Postman Collection

1. Open Postman
2. Click "Import" → Select "Banking_API.postman_collection.json"
3. Set variables:
   - `base_url` = `http://localhost:5114`
   - `jwt_token` = (set after login)

### Test Flow

```
1. Auth → Register
2. Auth → Login (copy token)
3. Set jwt_token variable
4. User → Get user by ID
5. BankAccount → Create
6. Cards → Request card
7. Cards → Approve request
8. Transactions → Deposit/Withdraw
```

---

## 📚 Additional Features

### Email Service

- Email verification for new registrations
- Email notifications for transactions
- Configurable email settings

### Logging & Auditing

- Comprehensive action logging
- Error tracking
- Request/Response logging

### Error Handling

- Middleware-based global error handling
- User-friendly error messages
- Detailed server-side logging

---

## 🔧 Configuration Files

- `appsettings.json` - Main configuration
- `appsettings.Development.json` - Development settings
- `Program.cs` - Dependency injection & middleware setup
- Database: SQLite (can be changed to SQL Server/PostgreSQL)

---

## 📈 Performance Features

- Async/await patterns for non-blocking operations
- Entity Framework Core with lazy loading
- Database indexing on frequently queried fields
- Rate limiting to prevent abuse

---

## 🎨 API Response Format

### Success Response

```json
{
  "data": {...},
  "message": "Operation successful",
  "statusCode": 200
}
```

### Error Response

```json
{
  "error": "Error message",
  "details": [...],
  "statusCode": 400
}
```

---

## 📞 Support Information

- **Database**: SQLite (default) / SQL Server / PostgreSQL (configurable)
- **Framework**: ASP.NET Core 6.0+
- **Authentication**: JWT Bearer Tokens
- **ORM**: Entity Framework Core
- **Testing**: xUnit framework available

---

**Last Updated**: 2026-04-13
**Version**: 1.0.0
