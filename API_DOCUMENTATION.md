# Banking API - Complete Documentation

**Base URL:** `http://localhost:5114`

---

## Table of Contents

1. [Authentication](#authentication)
2. [Users](#users)
3. [Bank Accounts](#bank-accounts)
4. [Cards](#cards)
5. [Card Requests](#card-requests)
6. [Loans](#loans)
7. [Loan Requests](#loan-requests)
8. [Transactions](#transactions)
9. [Roles](#roles)

---

## Authentication

### Register User

- **Endpoint:** `POST /api/auth/register`
- **Authentication:** None
- **Description:** Register a new user account
- **Request Body:**

```json
{
  "email": "user@example.com",
  "password": "Password123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "1234567890",
  "dateOfBirth": "1990-01-01",
  "address": "123 Main St"
}
```

- **Response:** User registered successfully + Verification email sent

### Login

- **Endpoint:** `POST /api/auth/login`
- **Authentication:** None
- **Description:** Login user and receive JWT token
- **Request Body:**

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

- **Response:**

```json
{
  "token": "jwt_token_here"
}
```

### Verify Email

- **Endpoint:** `GET /api/auth/verify-email`
- **Authentication:** None
- **Description:** Verify user email address
- **Query Parameters:**
  - `userId` (string): User ID
  - `token` (string): Verification token from email
- **Response:** Email verification successful

### Logout

- **Endpoint:** `POST /api/auth/logout`
- **Authentication:** Required (Bearer Token)
- **Description:** Logout current user
- **Response:** Logged out

### Delete User

- **Endpoint:** `DELETE /api/auth/delete-user/{email}`
- **Authentication:** Required (Bearer Token)
- **Description:** Delete user account
- **Response:** User deleted successfully

---

## Users

### Get All Users

- **Endpoint:** `GET /api/user`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all users (Admin only)
- **Response:** List of all users

### Get User by ID

- **Endpoint:** `GET /api/user/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific user details
- **Path Parameters:**
  - `id` (string): User ID
- **Response:** User details

### Update User

- **Endpoint:** `PUT /api/user/{id}`
- **Authentication:** Required
- **Description:** Update user information
- **Path Parameters:**
  - `id` (string): User ID
- **Request Body:**

```json
{
  "id": "USER_ID",
  "firstName": "Jane",
  "lastName": "Doe",
  "email": "user@example.com"
}
```

- **Response:** User updated successfully

---

## Bank Accounts

### Get All Bank Accounts

- **Endpoint:** `GET /api/bankaccount`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all bank accounts (Admin only)
- **Response:** List of all bank accounts

### Get Bank Account by ID

- **Endpoint:** `GET /api/bankaccount/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific bank account
- **Path Parameters:**
  - `id` (integer): Account ID
- **Response:** Bank account details

### Create Bank Account

- **Endpoint:** `POST /api/bankaccount`
- **Authentication:** Required
- **Description:** Create new bank account
- **Request Body:**

```json
{
  "accountNumber": "ACC123456",
  "accountName": "Savings Account",
  "accountStatus": "Active",
  "accountType": "Savings",
  "userId": "USER_ID"
}
```

- **Response:** Bank account created

### Update Bank Account

- **Endpoint:** `PUT /api/bankaccount/{id}`
- **Authentication:** Required
- **Description:** Update bank account information
- **Path Parameters:**
  - `id` (integer): Account ID
- **Request Body:**

```json
{
  "accountId": 1,
  "accountNumber": "ACC123456",
  "accountName": "Updated Savings Account",
  "accountStatus": "Active",
  "accountType": "Savings",
  "userId": "USER_ID"
}
```

- **Response:** No Content (204)

### Delete Bank Account

- **Endpoint:** `DELETE /api/bankaccount/{id}`
- **Authentication:** Required (Admin)
- **Description:** Delete bank account (Admin only)
- **Path Parameters:**
  - `id` (integer): Account ID
- **Response:** No Content (204)

---

## Cards

### Get All Cards

- **Endpoint:** `GET /api/card`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all cards (Admin only, returns hashed data)
- **Response:** List of all cards

### Get Card by ID

- **Endpoint:** `GET /api/card/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific card details
- **Path Parameters:**
  - `id` (integer): Card ID
- **Response:** Card details

### Create Card

- **Endpoint:** `POST /api/card`
- **Authentication:** Required (Admin)
- **Description:** Create new card (Admin only)
- **Request Body:**

```json
{
  "cardNumber": "4532123456789010",
  "last4Digits": "9010",
  "cardType": "Debit",
  "cardBrand": "Visa",
  "expiryDate": "12/25",
  "cvv": "123",
  "pin": "1234",
  "userId": "USER_ID",
  "bankAccountId": 1
}
```

- **Response:** Card created

### Update Card

- **Endpoint:** `PUT /api/card/{id}`
- **Authentication:** Required (Admin)
- **Description:** Update card information (Admin only)
- **Path Parameters:**
  - `id` (integer): Card ID
- **Request Body:**

```json
{
  "id": 1,
  "cardNumber": "4532123456789010",
  "last4Digits": "9010",
  "cardType": "Debit",
  "cardBrand": "Visa",
  "expiryDate": "12/26",
  "cvv": "123",
  "pin": "1234",
  "userId": "USER_ID",
  "bankAccountId": 1
}
```

- **Response:** No Content (204)

### Delete Card

- **Endpoint:** `DELETE /api/card/{id}`
- **Authentication:** Required
- **Description:** Delete card
- **Path Parameters:**
  - `id` (integer): Card ID
- **Response:** No Content (204)

---

## Card Requests

### Get All Card Requests

- **Endpoint:** `GET /api/cardrequest`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all card requests (Admin only)
- **Response:** List of all card requests

### Get Card Request by ID

- **Endpoint:** `GET /api/cardrequest/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific card request
- **Path Parameters:**
  - `id` (integer): Card Request ID
- **Response:** Card request details

### Create Card Request

- **Endpoint:** `POST /api/cardrequest`
- **Authentication:** Required
- **Description:** Submit new card request
- **Request Body:**

```json
{
  "userId": "USER_ID",
  "bankAccountId": 1,
  "cardType": "Debit",
  "status": "Pending"
}
```

- **Response:** Card request created

### Update Card Request

- **Endpoint:** `PUT /api/cardrequest/{id}`
- **Authentication:** Required
- **Description:** Update card request
- **Path Parameters:**
  - `id` (integer): Card Request ID
- **Request Body:**

```json
{
  "id": 1,
  "userId": "USER_ID",
  "bankAccountId": 1,
  "cardType": "Credit",
  "status": "Pending"
}
```

- **Response:** No Content (204)

### Approve Card Request

- **Endpoint:** `POST /api/cardrequest/approve`
- **Authentication:** Required (Admin)
- **Description:** Approve/Reject card request (Admin only)
- **Request Body:**

```json
{
  "cardRequestId": 1,
  "approved": true
}
```

- **Response:** Card approved/rejected

### Delete Card Request

- **Endpoint:** `DELETE /api/cardrequest/{id}`
- **Authentication:** Required
- **Description:** Delete card request
- **Path Parameters:**
  - `id` (integer): Card Request ID
- **Response:** No Content (204)

---

## Loans

### Get All Loans

- **Endpoint:** `GET /api/loan`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all loans (Admin only)
- **Response:** List of all loans

### Get Loan by ID

- **Endpoint:** `GET /api/loan/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific loan details
- **Path Parameters:**
  - `id` (integer): Loan ID
- **Response:** Loan details

### Create Loan

- **Endpoint:** `POST /api/loan`
- **Authentication:** Required (Admin)
- **Description:** Create new loan (Admin only)
- **Request Body:**

```json
{
  "principalAmount": 50000,
  "durationInMonths": 24,
  "bankAccountId": 1,
  "userId": "USER_ID"
}
```

- **Response:** Loan created

### Update Loan

- **Endpoint:** `PUT /api/loan/{id}`
- **Authentication:** Required
- **Description:** Update loan information
- **Path Parameters:**
  - `id` (integer): Loan ID
- **Request Body:**

```json
{
  "id": 1,
  "principalAmount": 50000,
  "durationInMonths": 24,
  "bankAccountId": 1,
  "userId": "USER_ID"
}
```

- **Response:** No Content (204)

### Delete Loan

- **Endpoint:** `DELETE /api/loan/{id}`
- **Authentication:** Required (Admin)
- **Description:** Delete loan (Admin only)
- **Path Parameters:**
  - `id` (integer): Loan ID
- **Response:** No Content (204)

---

## Loan Requests

### Get All Loan Requests

- **Endpoint:** `GET /api/loanrequest`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all loan requests (Admin only)
- **Response:** List of all loan requests

### Get Loan Request by ID

- **Endpoint:** `GET /api/loanrequest/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific loan request
- **Path Parameters:**
  - `id` (integer): Loan Request ID
- **Response:** Loan request details

### Create Loan Request

- **Endpoint:** `POST /api/loanrequest`
- **Authentication:** Required
- **Description:** Submit new loan request
- **Request Body:**

```json
{
  "userId": "USER_ID",
  "bankAccountId": 1,
  "principalAmount": 50000,
  "durationInMonths": 24,
  "status": "Pending"
}
```

- **Response:** Loan request created

### Update Loan Request

- **Endpoint:** `PUT /api/loanrequest/{id}`
- **Authentication:** Required
- **Description:** Update loan request
- **Path Parameters:**
  - `id` (integer): Loan Request ID
- **Request Body:**

```json
{
  "id": 1,
  "userId": "USER_ID",
  "bankAccountId": 1,
  "principalAmount": 50000,
  "durationInMonths": 24,
  "status": "Pending"
}
```

- **Response:** No Content (204)

### Approve Loan Request

- **Endpoint:** `POST /api/loanrequest/approve`
- **Authentication:** Required (Admin)
- **Description:** Approve/Reject loan request (Admin only)
- **Request Body:**

```json
{
  "loanRequestId": 1,
  "approved": true
}
```

- **Response:** Loan approved/rejected

### Delete Loan Request

- **Endpoint:** `DELETE /api/loanrequest/{id}`
- **Authentication:** Required
- **Description:** Delete loan request
- **Path Parameters:**
  - `id` (integer): Loan Request ID
- **Response:** No Content (204)

---

## Transactions

### Get All Transactions

- **Endpoint:** `GET /api/transaction`
- **Authentication:** Required
- **Description:** Retrieve all transactions
- **Response:** List of all transactions

### Get Transaction by ID

- **Endpoint:** `GET /api/transaction/{id}`
- **Authentication:** Required
- **Description:** Retrieve specific transaction
- **Path Parameters:**
  - `id` (integer): Transaction ID
- **Response:** Transaction details

### Deposit

- **Endpoint:** `POST /api/transaction/Deposit`
- **Authentication:** Required
- **Description:** Make a deposit transaction
- **Request Body:**

```json
{
  "accountId": 1,
  "amount": 1000,
  "transactionType": "Deposit",
  "status": "Completed"
}
```

- **Response:** Deposit transaction created

### Withdraw

- **Endpoint:** `POST /api/transaction/Withdraw`
- **Authentication:** Required
- **Description:** Make a withdrawal transaction
- **Request Body:**

```json
{
  "accountId": 1,
  "amount": 500,
  "transactionType": "Withdraw",
  "status": "Completed"
}
```

- **Response:** Withdrawal transaction created

---

## Roles

**Note:** All role endpoints require Admin authentication

### Get All Roles

- **Endpoint:** `GET /api/roles`
- **Authentication:** Required (Admin)
- **Description:** Retrieve all roles
- **Response:** List of all roles

### Get Role by ID

- **Endpoint:** `GET /api/roles/{roleId}`
- **Authentication:** Required (Admin)
- **Description:** Retrieve specific role
- **Path Parameters:**
  - `roleId` (string): Role ID
- **Response:** Role details

### Create Role

- **Endpoint:** `POST /api/roles`
- **Authentication:** Required (Admin)
- **Description:** Create new role
- **Request Body:** `"Manager"` (string)
- **Response:** Role created successfully

### Update Role

- **Endpoint:** `PUT /api/roles`
- **Authentication:** Required (Admin)
- **Description:** Update existing role
- **Request Body:**

```json
{
  "roleId": "ROLE_ID",
  "newRoleName": "UpdatedManager"
}
```

- **Response:** Role updated successfully

### Delete Role

- **Endpoint:** `DELETE /api/roles`
- **Authentication:** Required (Admin)
- **Description:** Delete role
- **Query Parameters:**
  - `roleId` (string): Role ID to delete
- **Response:** Role deleted successfully

### Assign Role to User

- **Endpoint:** `POST /api/roles/assign-role-to-user`
- **Authentication:** Required (Admin)
- **Description:** Assign role to user
- **Request Body:**

```json
{
  "userId": "USER_ID",
  "roleName": "Manager"
}
```

- **Response:** Role assigned to user successfully

---

## Quick Testing Guide

### 1. Register & Login

```
1. Register user: POST /api/auth/register
2. Login: POST /api/auth/login (Get JWT Token)
3. Copy token to variable: jwt_token
```

### 2. User Management

```
1. Get all users: GET /api/user (Admin only)
2. Get specific user: GET /api/user/{id}
3. Update user: PUT /api/user/{id}
```

### 3. Bank Accounts

```
1. Create account: POST /api/bankaccount
2. Get account: GET /api/bankaccount/{id}
3. Update account: PUT /api/bankaccount/{id}
```

### 4. Cards

```
1. Create card: POST /api/card (Admin)
2. Get card: GET /api/card/{id}
3. Request card: POST /api/cardrequest
4. Approve request: POST /api/cardrequest/approve (Admin)
```

### 5. Loans

```
1. Request loan: POST /api/loanrequest
2. Get request: GET /api/loanrequest/{id}
3. Approve loan: POST /api/loanrequest/approve (Admin)
```

### 6. Transactions

```
1. Make deposit: POST /api/transaction/Deposit
2. Make withdrawal: POST /api/transaction/Withdraw
3. View transactions: GET /api/transaction
```

### 7. Roles (Admin)

```
1. Create role: POST /api/roles
2. Assign to user: POST /api/roles/assign-role-to-user
3. View all roles: GET /api/roles
```

---

## Authentication Notes

- JWT token obtained from login is valid for a configurable duration (default: set in appsettings)
- Include token in header: `Authorization: Bearer {token}`
- Refresh token if expired by logging in again
- Some endpoints require specific roles (indicated with "Admin")

---

## Security Features

- ✅ JWT Bearer Token Authentication
- ✅ Role-based Access Control (RBAC)
- ✅ Email Verification
- ✅ Password Hashing
- ✅ Card/PIN/CVV Hashing (PCI Compliance)
- ✅ Rate Limiting (6 requests per minute)

---

## Status Codes

- `200 OK` - Successful request
- `201 Created` - Resource successfully created
- `204 No Content` - Successful but no content to return
- `400 Bad Request` - Invalid request
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Access denied
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error
- `429 Too Many Requests` - Rate limit exceeded
