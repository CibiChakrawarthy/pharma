# 🧮 SoftwareEngineer-L3: Gross-Profit API Challenge

You’ll build a **minimal-API** endpoint in .NET that reports each product’s **gross profit**.  
The repo already contains:

* a **deterministic** SQLite seed (`seed.sql`) with
  * 50 supplement products
  * 100 orders (`Status` = *Complete* or *Cancelled*)
  * 1-to-5 items per order
* a compile-ready stub endpoint you must finish (`/Routes/GrossProfitEndpoints.cs`)
* public unit tests in `Api.Tests/public`

> **Time box:** **1 h 30 m**  
> **Plan:** ~30 m setup + ≤ 60 m core coding  
> **Stretch:** any time left for the **date-filter bonus**

---

## 📝 What to build

|                  | Requirement                                                                                                                                                                                                                                   |
|------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Endpoint**     | `GET /api/gross-profit`                                                                                                                                                                                                                       |
| **Query params** | `sort` = **asc/desc** (default `desc`) <br> `limit` = positive int (optional)                                                                                                                                                                 |
| **Core logic**   | 1. Only use **Complete** orders. <br>2. For each product<br> `GrossProfit = Σ((UnitPrice − UnitCost) × Quantity)`<br>3. Return JSON array sorted by `GrossProfit` (fetch `limit` rows). <br>4. On bad `sort` or `limit` ⇒ **400 Bad Request** |
| **Bonus**        | Support <br>`minDate` and/or `maxDate` (ISO-8601 date part only). <br>Filter on `Orders.OrderDate`.                                                                                                                                           |
| **Tech**         | **Dapper + raw SQL** (no EF).                                                                                                                                                                                                                 |

### Response shape

```jsonc
[
  {
    "productId": 1,
    "name": "Ashwagandha KSM-66®",
    "unitCost": 10.0,
    "unitPrice": 20.0,
    "grossProfit": 200.0
  },
  …
]
```

---

## 🏁 Getting started

1. Click the assignment link sent you and **accept** the exercise.
2. GitHub Classroom will create **your own private repository** for this task.
3. Clone that repository:

```bash
git clone <your-assignment-repo-url>   # the URL shown after acceptance
cd software-engineer-l3-<your-github-handle>
```

> **Tip**: If you misplace the URL, it's also on the repo's green **Code** button.

4. Run the Api project

```bash
dotnet run --project Api
```

> *First launch writes* `grossprofit.db` (from `seed.sql`) and starts listening on **http://localhost:5226**.

5. Verify the stub:

```bash
curl http://localhost:5226/api/gross-profit
# → []
```

6. Start editing the `/api/gross-profit` endpoint in `/Routes/GrossProfitEndpoints.cs`

---

## 🗂️ Repo layout

```
Api/
├─ seed.sql               ← deterministic data
├─ Program.cs             ← runs seed & wires DI
├─ Models/
│  ├─ Order*.cs
│  ├─ OrderItem.cs
│  ├─ Product.cs
│  └─ ProductProfitDto.cs
└─ Routes/
   └─ GrossProfitEndpoints.cs  ← **👉 implement here**
Api.Tests/
 └─ public/   ← unit tests
```

---

## ✅ Running tests

Feel free to write your own tests inside the `Api.Tests` project.

```bash
cd Api.Tests
dotnet test
```

---

## 🎯 Tips

* Keep SQL simple
* Validate query parameters early; surface clear error messages
* For the bonus, add conditional `WHERE` clauses only when the parameter is supplied
* Don’t rename files or namespaces - the tests rely on them

Good luck! 🚀
