# 🧾 Assignment 3 — Equipment Rental Management System (Google OAuth)

## 👨‍💻 Course
**Enterprise Application Development (ASP.NET Core Web API)**  

## 👥 Team Members
- **Jenil Gohel**  
- **Nirali Sathvara**  
- **Samson Ikilama**

---

## 🎯 Objective

Enhance the **Equipment Rental Management System** by replacing JWT authentication with **Google OAuth (OpenID Connect)** while maintaining **role-based authorization** using claims.

This ensures:
- 👑 **Admins** can manage all equipment, rentals, and customers.  
- 👤 **Users** can only browse and view their own data.

---

## ⚙️ Tech Stack

| Layer | Technology |
|-------|-------------|
| **Frontend** | ASP.NET Core MVC (Razor Views) |
| **Backend** | ASP.NET Core Web API |
| **Database** | Entity Framework Core (In-Memory) |
| **Authentication** | Google OAuth 2.0 with OpenID Connect |
| **Authorization** | Role-based using Claims |
| **Language** | C# (.NET 8.0) |
| **Styling** | Bootstrap 5 |

---

## 🔐 Authentication & Claims Mapping

Google OAuth is implemented via **OpenID Connect** in `Program.cs` (UI project):

```csharp
options.ClientId = "<your-client-id>";
options.ClientSecret = "<your-client-secret>";
options.CallbackPath = "/signin-oidc";
options.Scope.Add("openid");
options.Scope.Add("profile");
options.Scope.Add("email");
```
After successful login, the system extracts Google claims such as:

email
name
sub (unique Google ID)

Then, the app checks the email in the local AppUser table.

public class AppUser {
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string Role { get; set; } = "User"; // Default role
    public string? ExternalProvider { get; set; } = "Google";
    public string? ExternalId { get; set; }
}

If the user’s email matches a seeded Admin (e.g., jgohel9902@gmail.com),
the app assigns the Admin role.

Otherwise, it defaults to User.

Roles are attached as claims during the OnTokenValidated event.

🔒 Authorization Logic
Attribute	Access Level	Example
[Authorize]	Any logged-in user	/Home/Secure
[Authorize(Roles = "Admin")]	Admin only	/Equipment/Add
[Authorize(Roles = "User")]	Regular user	/Home/UserDashboard

Role-based claims are automatically validated by the middleware:

✅ Role assigned to jgohel9902@gmail.com: Admin  
✅ Role assigned to jenilgohel2005@gmail.com: User

🧭 Feature Overview
👑 Admin Role

Admins have full control over the system.

Feature	Description
➕ Add Equipment	Create new rental items
🧰 Manage Equipment	View, edit, delete equipment
⚠️ View Overdue Rentals	Check items not returned on time
📋 Completed Rentals	Track all returned items
👥 Manage Customers	Admin-only access
🏠 Dashboard Stats	See rented, overdue, and total counts
👤 User Role

Users can only view and interact with their data.

Feature	Description
🔍 Browse Equipment	View available items for rent
📦 My Active Rentals	Check ongoing rentals
✅ Completed Rentals	See rental history
👤 My Info	View user details from Google claims
🚫 Restricted	Cannot add or edit equipment
🧩 Local User Store

Seeded in AppDbContext:

ID	Email	Role	Provider
1	jgohel9902@gmail.com
	Admin	Google
2	jenilgohel2005@gmail.com
	User	Google
🧠 How Role Redirection Works

After a successful Google login:

System reads user’s email and checks local AppUser table.

Assigns appropriate Role claim (Admin/User).

Redirects user based on role:

Admin → /Home/AdminDashboard

User → /Home/UserDashboard

Unauthorized access to other role pages → Access Denied.

💻 Setup Instructions
1️⃣ Configure Google OAuth

Go to Google Cloud Console
.

Create a New OAuth 2.0 Client ID.

Add the following as Authorized Redirect URI:

https://localhost:7145/signin-oidc


Copy the Client ID and Client Secret.

Paste them into your EquipmentRentalUI/appsettings.json:

"Authentication": {
  "Google": {
    "ClientId": "<your-client-id>",
    "ClientSecret": "<your-client-secret>"
  }
}

2️⃣ Run Both Projects

Open the solution in Visual Studio.
Set both projects (API + UI) as startup.
Press F5 or Run All.

The URLs should be:
API → https://localhost:7030
UI → https://localhost:7145

3️⃣ Test Logins
Role	Email	Description
Admin	jgohel9902@gmail.com	Full Access
User	jenilgohel2005@gmail.com	View-Only Access



🚀 Result
Seamless Google OAuth authentication
Role-based authorization with claims
Modern bootstrap-styled dashboards
Clean and professional UI for both Admin and User
Fully functional and ready for submission 🎯

🧑‍💻 Developed By
Group 4 — Equipment Rental Management System
Jenil Gohel
Nirali Sathvara
Samson Ikilama
