# 🧮 Commission Calculator — Technical Assessment

> Finish and ship a production-quality Commission Calculator for **Avalpha Technologies**.  
> The API and React app are scaffolded; the controller exists but the logic & wiring are incomplete **by design**.

---

## 🚀 What you’ll build

- Connect **frontend (React)** ↔ **backend (C#/.NET)**  
- Implement commission calculations in the C# controller  
- Deliver clean, production-ready code (tests, structure, readability)

⏱ **Timebox:** up to **4 hours max**. Be pragmatic. Prioritize correctness, clarity, and the essentials.

---

## 🔀 Before you start (Fork + Setup)

1. **Fork** this repository into your own GitHub account.  
2. **Clone** your fork locally.  
3. Work in a feature branch, e.g. `feat/commission-impl`.  
4. When done, push to your fork and open a **Pull Request** back to your fork’s `main`.  
   - Add a short **README-notes.md** describing decisions, trade-offs, and anything unfinished.  
5. Share your fork/PR link with us.

> ✅ We want to see how you think, structure, and ****. Small, meaningful commits > one giant commit.

---

## 🧠 Business Rules

At **Avalpha Technologies**:
- **Local Sales Commission:** **20%**
- **Foreign Sales Commission:** **35%**

Competitors pay:
- **Local:** **2%**
- **Foreign:** **7.55%**

**Inputs:**
- `localSalesCount` (number)  
- `foreignSalesCount` (number)  
- `averageSaleAmount` (currency/number)  

**Output (example):**

Local Sales count: 10
Foreign Sales count: 10
Average Sales Amount: £100

Avalpha Commission:

Local = 20% * 10 * 100 = £200

Foreign = 35% * 10 * 100 = £350

Total = £550

Competitor Commission:

Local = 2% * 10 * 100 = £20

Foreign = 7.55% * 10 * 100 = £75.5

Total = £95.5

## 🧩 Your Tasks (Checklist)

- [✅] Wire up the **React frontend** to call the backend API
- [✅] Implement calculation logic
- [✅] Validate inputs (numbers ≥ 0, sensible upper bounds)
- [✅] Return a typed, well-structured response (DTO)
- [✅] Display results in the UI with clear labels and currency formatting
- [✅] Handle errors gracefully (backend & UI)
- [✅ ] Provide basic **docs**: how to run, how to test, decisions
- [] Keep commits small and messages clear 

---

## 🧱 Tech Stack

- **Frontend:** React (Vite/CRA), TypeScript preferred (if scaffolded), Fetch/Axios OK  
- **Backend:** .NET (C#), minimal API or MVC controller  
- **Tests:** xUnit/NUnit + React Testing Library / Vitest/Jest  