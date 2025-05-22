# 💊 PharmaCare - Pharmacy Management System

**PharmaCare** is a robust pharmacy management system developed using **.NET MVC**. It is designed to streamline pharmacy operations by offering powerful tools for inventory control, order management, customer interaction, and pharmacist supervision. The system ensures secure transactions, real-time tracking, and role-based access for administrators and pharmacists.

---

## 📌 Features

### 🔐 Admin Panel
- Full access to inventory management.
- Add sales (retail & bulk), and manage all orders.
- Monitor stock and request orders from suppliers.
- Add/update medicine data (quantity, expiry date).
- Categorize medicines for organized tracking.
- Monitor payment transactions for fraud.
- Generate sales and revenue reports.
- Add or remove pharmacists and manage permissions.
- View statistics (total sales, completed & active orders).

### 👨‍⚕️ Pharmacist Panel
- View assigned inventory and orders.
- Confirm prescriptions from customers.
- Serve multiple customers and manage consultations.
- Access limited features based on permissions.

### 🧑‍💻 Customer Features
- Register and manage profile.
- Browse medicines and products by category.
- Upload prescriptions for review.
- Get advice from pharmacists.
- Place orders and leave reviews or complaints.

### 🛒 Orders & Payments
- Smooth order flow: from cart to delivery.
- Multiple payment options (credit, wallets, etc.).
- Secure and fast payment processing.
- Automated invoicing and order confirmations.
- Stock updates and availability alerts.

### 🔔 Notification System
- Low stock and near-expiry alerts.
- Order confirmation notifications.
- Expiration reminders.
- Promotions and medicine availability updates.

---

## 🛠️ Technologies Used

- **Backend:**
- ASP.NET MVC
- ASP.NET Core Web API
- Entity Framework Core
- LINQ
- SQL Server
- Manual Mapping
- JWT for authentication with Identity
- Swagger (for API documentation)
- **Frontend:** Razor Views  
- **Database:** SQL Server  
- **Authentication:** JWT Identity Authentication
- **Notifications:** System-based alerts

---

## 🧱 Database Overview

- Real-time inventory tracking (stock level, expiry).
- Entity Relationship Diagram (ERD) showing relations:
  - Admin ↔ Inventory, Pharmacist, Orders
  - Customer ↔ Prescriptions, Orders, Reviews
  - Pharmacist ↔ Customers, Inventory
  - Orders ↔ Payment Methods
  - Medicines ↔ Categories

---

## 🧭 System Flow

1. **User Registration & Validation**  
   Includes validations for password strength, phone, and email.

2. **Login & Access Control**  
   Users are redirected to dashboards based on roles.

3. **Inventory Operations**  
   Admins manage stock, track batches, and categorize medicines.

4. **Prescription Handling**  
   Customers upload prescriptions, reviewed by pharmacists.

5. **Order Processing**  
   Orders confirmed → Payment → Invoice generated → Notification sent.

---
