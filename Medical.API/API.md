# API Reference

Base URL (local): `http://localhost:5099` or `https://localhost:7171`

All controllers use `[ApiController]` and return JSON. Errors from unhandled exceptions are returned by `GlobalExceptionMiddleware` as:

```json
{
  "requestId": "...",
  "message": "...",
  "detailedMessage": "...",
  "stackTrace": "..."
}
```

**Swagger:** `GET /swagger` (Development only)

---

## Bills — `/api/bills`

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/bills` | Create empty bill |
| POST | `/api/bills/{id}/items` | Add line item (deducts stock FIFO) |
| GET | `/api/bills/{id}` | Get bill with items |
| GET | `/api/bills` | List all bills |
| GET | `/api/bills/daily-sales` | Daily sales total |
| PUT | `/api/bills/{id}` | Update bill (e.g. discount) |
| DELETE | `/api/bills/{id}` | Delete bill (restores stock) |
| PUT | `/api/bills/items/{id}` | Update bill line quantity |
| DELETE | `/api/bills/items/{id}` | Delete bill line (restores stock) |

### Add bill item (example)

```http
POST /api/bills/{billId}/items
Content-Type: application/json

{
  "medicineId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantity": 2
}
```

---

## Patients — `/api/patients`

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/patients` | Create patient |
| GET | `/api/patients/{id}` | Get by id |
| GET | `/api/patients/{id}/history` | Purchase history |
| GET | `/api/patients` | List all |
| PUT | `/api/patients/{id}` | Update |
| DELETE | `/api/patients/{id}` | Delete |

---

## Medicines — `/api/medicines`

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/medicines` | Create |
| PUT | `/api/medicines/{id}` | Update |
| DELETE | `/api/medicines/{id}` | Delete |
| GET | `/api/medicines` | List all |
| GET | `/api/medicines/{id}` | Get by id |
| GET | `/api/medicines/low-stock` | Low stock (threshold on entity) |
| GET | `/api/medicines/expiring` | Expiring within window |

### Create medicine (example)

```http
POST /api/medicines
Content-Type: application/json

{
  "medicineName": "Paracetamol 500mg",
  "medicineCategory": "Analgesic",
  "medicinePrice": 25.00,
  "stock": 100,
  "expirationDate": "2027-12-31",
  "supplierId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

## Inventory — `/api/inventory`

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/inventory/batches` | Add batch |
| PUT | `/api/inventory/batches/{id}` | Update batch |
| DELETE | `/api/inventory/batches/{id}` | Delete batch |
| POST | `/api/inventory/adjust-stock` | Set batch quantity |
| POST | `/api/inventory/deduct-stock` | FIFO deduct |
| POST | `/api/inventory/restore-stock` | Restore to batch |
| POST | `/api/inventory/remove-expired` | Remove expired stock |
| GET | `/api/inventory/summary` | Inventory summary |
| GET | `/api/inventory/low-stock` | Low-stock batches |
| GET | `/api/inventory/expiring` | Expiring batches |
| GET | `/api/inventory/audit` | Stock audit |
| GET | `/api/inventory/batches/{medicineId}` | Batches for medicine |
| GET | `/api/inventory/batches` | All batches |
| GET | `/api/inventory/batches/{id}/details` | Batch details |
| GET | `/api/inventory/transactions` | Transaction history |

---

## Suppliers — `/api/supplier`

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/supplier` | Create |
| GET | `/api/supplier` | List all |
| GET | `/api/supplier/{id}` | Get by id |
| PUT | `/api/supplier/{id}` | Update |
| DELETE | `/api/supplier/{id}` | Delete |

---

## AI / analytics — `/api/ai`

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/ai/suggest-medicines?patientId={guid}` | Suggest medicines for patient |
| GET | `/api/ai/low-stock-prediction` | Predict low-stock risk |
| GET | `/api/ai/sales-trends?days=30` | Sales trend analysis |
| POST | `/api/ai/calculate-bill-total` | Calculate bill totals with warnings |

---

## Testing with Swagger

1. Run the API: `dotnet run --project Medical.API`
2. Open `http://localhost:5099/swagger`
3. Typical flow:
   - Create **supplier** → **medicine** → **inventory batch**
   - Create **patient** → **bill** → **add bill items**
   - Use **GET** endpoints to verify stock and totals

## HTTP status notes

| Situation | Typical status |
|-----------|----------------|
| Success | 200 OK |
| Validation failure | 400 (FluentValidation via pipeline) |
| Entity tracking / DB conflict | 400 / 409 (middleware) |
| Unhandled error | 500 |

## MediatR command/query map

Handlers live under `Medical.Application/Handlers/`. Naming convention:

- `CreateXCommand` / `UpdateXCommand` / `DeleteXCommand` → write operations  
- `GetXQuery` → read operations  

Validators are in `Medical.Application/Validators/` and run automatically before handlers.

For full handler list, search the `Handlers` folder or use Swagger operation IDs.
