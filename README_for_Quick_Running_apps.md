### Prerequisites

- .NET 8.0 SDK (Make sure you have this SDK, if we have only latest SDK it wont work)
- Node.js 16+ (for frontend)

### Backend Setup

```bash
cd api
dotnet restore AvalphaTechnologies.CommissionCalculator.csproj
dotnet run --project AvalphaTechnologies.CommissionCalculator.csproj
```

Backend runs on: `http://localhost:5111`

### Frontend Setup

```bash
cd ui
npm install
npm start
```

Frontend runs on: `http://localhost:3000`

---

## 📝 Notes

- All currency values are in **British Pounds (£)**
- Ensure both backend and frontend are running simultaneously
- Check browser console (F12) for any frontend errors
- Check terminal for backend logs

---

## 📧 Contact

For issues or questions, contact the development team (official.akashk@gmail.com).
