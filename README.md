# 📚 ReadSynch

ReadSynch is a full-stack web application that allows users to track books they've read, view detailed book information, log progress, leave ratings, and manage personal reading lists. The app integrates with the Google Books API to search and add books to a user’s library.

---

## 🔧 Tech Stack

### Backend
- **ASP.NET Core Web API**
- **PostgreSQL** (via Entity Framework Core)
- **JWT Authentication**
- **Google Books API Integration**
- **User Secrets for secure local development**

### Frontend
- **HTML/JS, Bootstrap**
- Hosted separately in `/frontend` directory

---

## 🚀 Features

- 🔐 Secure user authentication with JWT
- 📖 Search books via Google Books API
- ⭐ Add, favorite, rate, and update reading status for books
- 🗂 View personal library and reading progress
- 📝 Leave reviews or notes (future feature)
- 📄 Book details view for both added and unadded books

---

## 🛠 Local Development Setup

### 📦 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)
- Optional: [Node.js](https://nodejs.org/)

---

### 🔐 Set up User Secrets

Configure sensitive data using [ASP.NET Core User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

```bash
dotnet user-secrets set "ConnectionStrings:PostgreSQL" "Host=localhost;Database=ReadSynch;Username=postgres;Password=yourpassword"
dotnet user-secrets set "ApiKeys:GoogleBooks" "your_google_books_api_key"
dotnet user-secrets set "Jwt:Key" "your_secure_jwt_key"
dotnet user-secrets set "Jwt:Issuer" "https://localhost:5001"
dotnet user-secrets set "Jwt:Audience" "https://localhost:5001"
```

### 🧪 Run the Backend
bash
Copy
Edit
cd ReadSynch.Backend
dotnet ef database update     # Applies migrations
dotnet run
The API will start at https://localhost:5001

### 💻 Run the Frontend
bash
Copy
Edit
cd frontend
npm install
npm start
Frontend served at http://localhost:3000

---

## 📂 Project Structure
```bash
/ReadSynch
├── ReadSynch/     # ASP.NET Core Web API
│   ├── Controllers/
│   ├── Data/
│   ├── Dtos/
│   ├── Interfaces/
│   ├── Migrations/
│   ├── Models/
│   ├── Properties/
│   ├── Services/
│   └── Program.cs
├── ReadSynchTests/     # Unit Tests
│   ├── Controllers/
│   └── Services/
├── frontend/              # Frontend
│   └── ...
└── README.md
```
## 📌 Notes
Secrets are managed via User Secrets and never committed to source control.

This project is not deployed, but designed for full local development.

## 📚 License
This project is open-source and available under the MIT License.

## 🧠 Future Improvements
Social features (friend lists, shared libraries)

Book reviews and comments

Book recommendations

Mobile-friendly design

## 👨‍💻 Author
Liam O'Gara - [LinkedIn](www.linkedin.com/in/liam-o-gara-85a4502a2)