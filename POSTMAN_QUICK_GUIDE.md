# 🚀 Quick Testing Guide - Postman Setup

## Step-by-Step Setup for Fast Testing

### 1. Import Postman Collection

```
1. Open Postman
2. Click "Import" button (top left)
3. Select file: "Banking_API.postman_collection.json"
4. Click "Import"
```

### 2. Configure Environment Variables

Set these variables in Postman:

```
base_url: http://localhost:5114
jwt_token: (leave empty, will populate after login)
```

---

## 🔄 Complete Testing Workflow

### **Phase 1: Authentication**

#### Step 1.1: Register a New User

```
Method: POST
URL: {{base_url}}/api/auth/register
Body (raw JSON):
{
  "email": "testuser@example.com",
  "password": "Test@12345",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "1234567890",
  "dateOfBirth": "1990-01-15",
  "address": "123 Main Street, City"
}
```

✅ Expected: 200 OK - User registered with verification email

#### Step 1.2: Login User

```
Method: POST
URL: {{base_url}}/api/auth/login
Body (raw JSON):
{
  "email": "testuser@example.com",
  "password": "Test@12345"
}
```

✅ Expected: 200 OK - Returns JWT token
📌 **ACTION**: Copy the token from response and paste into `jwt_token` variable

#### Step 1.3: Verify Email (Optional)

```
Method: GET
URL: {{base_url}}/api/auth/verify-email?userId=USER_ID&token=TOKEN
```

(Get userId and token from registration email/response)

---

### **Phase 2: User Management**

#### Step 2.1: Get Current User

```
Method: GET
URL: {{base_url}}/api/user/USER_ID
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - User details returned

#### Step 2.2: Update User Profile

```
Method: PUT
URL: {{base_url}}/api/user/USER_ID
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "id": "USER_ID",
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "testuser@example.com"
}
```

✅ Expected: 200 OK - Profile updated

#### Step 2.3: Get All Users (Admin Only)

```
Method: GET
URL: {{base_url}}/api/user
Headers: Authorization: Bearer {{jwt_token}}
```

⚠️ Requires Admin role

---

### **Phase 3: Bank Account Management**

#### Step 3.1: Create Bank Account

```
Method: POST
URL: {{base_url}}/api/bankaccount
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "accountNumber": "ACC001234567",
  "accountName": "My Savings Account",
  "accountStatus": "Active",
  "accountType": "Savings",
  "userId": "USER_ID"
}
```

✅ Expected: 201 Created
📌 **SAVE**: Account ID from response (e.g., "accountId": 1)

#### Step 3.2: Get Bank Account

```
Method: GET
URL: {{base_url}}/api/bankaccount/1
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - Account details

#### Step 3.3: Update Bank Account

```
Method: PUT
URL: {{base_url}}/api/bankaccount/1
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "accountId": 1,
  "accountNumber": "ACC001234567",
  "accountName": "Updated Savings Account",
  "accountStatus": "Active",
  "accountType": "Savings",
  "userId": "USER_ID"
}
```

✅ Expected: 204 No Content

---

### **Phase 4: Card Management**

#### Step 4.1: Request New Card (User)

```
Method: POST
URL: {{base_url}}/api/cardrequest
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "userId": "USER_ID",
  "bankAccountId": 1,
  "cardType": "Debit",
  "status": "Pending"
}
```

✅ Expected: 201 Created
📌 **SAVE**: Card Request ID (e.g., "id": 1)

#### Step 4.2: View Card Request

```
Method: GET
URL: {{base_url}}/api/cardrequest/1
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - Request details

#### Step 4.3: Approve Card Request (Admin)

```
Method: POST
URL: {{base_url}}/api/cardrequest/approve
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "cardRequestId": 1,
  "approved": true
}
```

✅ Expected: 200 OK - Card created automatically
⚠️ Requires Admin role

#### Step 4.4: Get Card Details

```
Method: GET
URL: {{base_url}}/api/card/1
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - Card info (hashed for security)

---

### **Phase 5: Loan Management**

#### Step 5.1: Request Loan (User)

```
Method: POST
URL: {{base_url}}/api/loanrequest
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "userId": "USER_ID",
  "bankAccountId": 1,
  "principalAmount": 50000,
  "durationInMonths": 24,
  "status": "Pending"
}
```

✅ Expected: 201 Created
📌 **SAVE**: Loan Request ID (e.g., "id": 1)

#### Step 5.2: Get Loan Request

```
Method: GET
URL: {{base_url}}/api/loanrequest/1
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - Request details

#### Step 5.3: Approve Loan Request (Admin)

```
Method: POST
URL: {{base_url}}/api/loanrequest/approve
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "loanRequestId": 1,
  "approved": true
}
```

✅ Expected: 200 OK - Loan created
⚠️ Requires Admin role

---

### **Phase 6: Transactions**

#### Step 6.1: Deposit Money

```
Method: POST
URL: {{base_url}}/api/transaction/Deposit
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "accountId": 1,
  "amount": 5000,
  "transactionType": "Deposit",
  "status": "Completed"
}
```

✅ Expected: 201 Created
📌 Balance increased by 5000

#### Step 6.2: Withdraw Money

```
Method: POST
URL: {{base_url}}/api/transaction/Withdraw
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "accountId": 1,
  "amount": 1000,
  "transactionType": "Withdraw",
  "status": "Completed"
}
```

✅ Expected: 201 Created
📌 Balance decreased by 1000

#### Step 6.3: Get All Transactions

```
Method: GET
URL: {{base_url}}/api/transaction
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - Transaction history

#### Step 6.4: Get Specific Transaction

```
Method: GET
URL: {{base_url}}/api/transaction/1
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - Transaction details

---

### **Phase 7: Role Management (Admin Only)**

#### Step 7.1: Get All Roles

```
Method: GET
URL: {{base_url}}/api/roles
Headers: Authorization: Bearer {{jwt_token}}
```

✅ Expected: 200 OK - List of roles

#### Step 7.2: Create New Role

```
Method: POST
URL: {{base_url}}/api/roles
Headers: Authorization: Bearer {{jwt_token}}
Body (raw text): "Manager"
```

✅ Expected: 200 OK

#### Step 7.3: Assign Role to User

```
Method: POST
URL: {{base_url}}/api/roles/assign-role-to-user
Headers: Authorization: Bearer {{jwt_token}}
Body (raw JSON):
{
  "userId": "USER_ID",
  "roleName": "Manager"
}
```

✅ Expected: 200 OK - Role assigned

---

## 📊 Complete Test Scenario

### Scenario: Banking Customer Journey

```
1. NEW CUSTOMER SIGN-UP
   ├─ Register new user
   ├─ Login to get token
   └─ View profile

2. ACCOUNT SETUP
   ├─ Create bank account
   └─ View account details

3. CARD SERVICES
   ├─ Request debit card
   ├─ (Admin approves request)
   └─ View card details

4. LENDING SERVICES
   ├─ Request loan (50,000 for 24 months)
   ├─ (Admin approves loan)
   └─ View loan details

5. TRANSACTIONS
   ├─ Deposit 10,000
   ├─ Withdraw 2,000
   ├─ View transaction history
   └─ Check final balance

6. ADMIN FUNCTIONS
   ├─ View all users
   ├─ View all accounts
   ├─ View all card requests
   ├─ View all loan requests
   ├─ Manage roles
   └─ Approve/reject requests
```

---

## ⚠️ Common Issues & Solutions

### Issue: Unauthorized (401)

**Solution**: Make sure jwt_token is set in variables after login

### Issue: Forbidden (403)

**Solution**: Some endpoints require Admin role. Login with admin account.

### Issue: Not Found (404)

**Solution**: Verify the ID exists. Check IDs from previous responses.

### Issue: Bad Request (400)

**Solution**: Check JSON format. Required fields:

- Email format valid
- Passwords meet requirements (12+ chars recommended)
- All required fields present

### Issue: Rate Limited (429)

**Solution**: Wait a minute. API allows 6 requests per minute (configurable)

---

## 🎯 Testing Checklist

- [ ] User Registration
- [ ] User Login
- [ ] User Profile Update
- [ ] Bank Account Creation
- [ ] Bank Account Update
- [ ] Card Request Submission
- [ ] Card Request Approval (Admin)
- [ ] Card Viewing
- [ ] Loan Request Submission
- [ ] Loan Request Approval (Admin)
- [ ] Deposit Transaction
- [ ] Withdrawal Transaction
- [ ] Transaction History
- [ ] Role Creation (Admin)
- [ ] Role Assignment (Admin)
- [ ] View All Users (Admin)
- [ ] View All Accounts (Admin)

---

## 💡 Pro Tips

1. **Save IDs after creation** - Copy IDs from response for next requests
2. **Use Postman Runner** - Test multiple requests sequentially
3. **Check Response Status** - First indicator of success/failure
4. **Read Error Messages** - Often tells you exactly what's wrong
5. **Use Variables** - Reduces manual copying
6. **Test as Different Users** - Register multiple users to test relationships

---

**Happy Testing! 🎉**
