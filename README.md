# Order Status Integration Hub (OSIH)

## Project Overview

OSIH is a unified order management API and web interface that integrates order data from two legacy systems with different data formats and status codes. The solution provides a consistent view of all orders across both systems through RESTful API endpoints and a Razor Pages web interface.

**Key Features:**
- Aggregates orders from System A (JSON) and System B (CSV) into a single data model
- Unified API with filtering by status and date range
- Responsive web interface for browsing and searching orders
- Real-time data integration (no database required for this assessment)
- OpenAPI/Swagger documentation for API exploration

## Technology Stack

### Backend
- **Framework:** ASP.NET Core (.NET 10)
- **Language:** C# 14
- **API Documentation:** OpenAPI with Scalar API Reference
- **Data Parsing:** System.Text.Json (System A), CsvHelper (System B)

### Frontend
- **Framework:** ASP.NET Core Razor Pages
- **Templating:** Razor with HTML5
- **Styling:** Bootstrap 5
- **Client Validation:** jQuery Validation Unobtrusive
- **HTTP Client:** IHttpClientFactory for API communication

### Why These Choices?
- **.NET 10/C# 14:** Latest LTS release with modern language features, strong typing, and excellent async support
- **Razor Pages over MVC:** Simpler, more maintainable for a single-page web interface; cleaner separation of concerns
- **In-memory Storage:** Meets requirements without database complexity; data reloaded on each application start
- **OpenAPI/Scalar:** Developer-friendly API exploration without external dependencies
- **CsvHelper:** Industry-standard, reliable CSV parsing with minimal configuration

## Setup Instructions

### Prerequisites
- .NET 10 SDK or later
- Visual Studio 2022/2026 (or VS Code with C# Dev Kit)
- Git

### Local Development Setup

1. **Clone the repository:**
git clone https://github.com/StephenElmer/osih.git cd osih


2. **Verify data files exist:**
Ensure both files exist in the root `/data` directory:
- `data/system_a_orders.json` (12 orders from System A)
- `data/system_b_orders.csv` (13 orders from System B)

3. **Backend API Setup:**
cd backend/osih.api dotnet restore dotnet run

- API runs on: `https://localhost:7143`
- OpenAPI docs: `https://localhost:7143/api/openapi/v1.json`
- Scalar Reference: `https://localhost:7143/api/scalar`

4. **Frontend Web Setup (new terminal window):**
cd frontend/osih.web dotnet restore dotnet run

- Web interface runs on: `https://localhost:5001`
- Browse to: `https://localhost:5001/`

5. **Verify both services are running:**
- Test API health: `https://localhost:7143/health`
- Test frontend: `https://localhost:5001/`

### Project Structure
osih/ ├── README.md ├── requirements.md ├── data/ │   ├── system_a_orders.json │   └── system_b_orders.csv ├── backend/ │   └── osih.api/ │       ├── Controllers/ │       │   ├── OrdersController.cs │       │   └── HealthController.cs │       ├── Health.cs │       └── Program.cs ├── frontend/ │   └── osih.web/ │       ├── Controllers/ │       │   └── HomeController.cs │       ├── Models/ │       │   ├── HomeViewModel.cs │       │   └── ErrorViewModel.cs │       ├── Views/ │       │   ├── Home/ │       │   │   └── Index.cshtml │       │   └── Shared/ │       │       ├── _Layout.cshtml │       │       └── _ValidationScriptsPartial.cshtml │       ├── wwwroot/ │       │   ├── css/ │       │   ├── js/ │       │   └── lib/ │       └── Program.cs └── data/ ├── osih.data/ │   ├── DataAccess.cs │   ├── OrderA_DTO.cs │   └── OrderB_DTO.cs └── osih.model/ └── Order.cs


## API Documentation

### Base URL
- Development: `https://localhost:7143/api/`
- Production: `https://<app-service-name>.azurewebsites.net/api/`

### Endpoints

#### 1. **GET /api/orders**
Retrieve all orders from both systems in unified format.

**Response:**
[ { "orderId": "A-2024-1001", "sourceSystem": "SystemA", "customerName": "Acme Laboratory Services", "orderDate": "2024-11-15", "totalAmount": 2450.00, "status": "Processing" }, { "orderId": "241115-5001", "sourceSystem": "SystemB", "customerName": "Acme Laboratory Services", "orderDate": "2024-11-15", "totalAmount": 2680.00, "status": "Completed" } ]


---

#### 2. **GET /api/orders/{orderId}**
Retrieve a specific order by ID from either system.

**Parameters:**
- `orderId` (path, required): Order ID from either SystemA or SystemB

**Example:** `GET /api/orders/A-2024-1001`

**Response:**
{ "orderId": "A-2024-1001", "sourceSystem": "SystemA", "customerName": "Acme Laboratory Services", "orderDate": "2024-11-15", "totalAmount": 2450.00, "status": "Processing" }


**Status Code:** 200 (OK) or 404 (Not Found)

---

#### 3. **GET /api/orders/search**
Filter orders by status, date range, or both (all filter options work in combination).

**Query Parameters:**
- `status` (optional): Human-readable status value
  - Valid values: `Pending`, `Processing`, `Shipped`, `Completed`, `Cancelled`
- `startDate` (optional): Start date in ISO 8601 format (`YYYY-MM-DD`)
- `endDate` (optional): End date in ISO 8601 format (`YYYY-MM-DD`)

**Examples:**
- By status: `GET /api/orders/search?status=Processing`
- By date range: `GET /api/orders/search?startDate=2024-11-15&endDate=2024-11-30`
- Combined: `GET /api/orders/search?status=Completed&startDate=2024-11-15&endDate=2024-11-30`

**Response:** Same format as `/api/orders`, filtered by criteria

---

#### 4. **GET /health**
Health check endpoint for monitoring service availability.

**Response:**
{ "status": "Healthy" }


---

### Interactive API Testing
Use the Scalar API Reference at `https://localhost:7143/api/scalar` to test endpoints interactively without external tools.

## Approach & Decisions

### 1. Data Normalization Strategy

**Challenge:** Two systems use different data formats and status codes.

**Solution:**
- **Status Code Mapping:** Implemented `NormalizeStatus()` method in `DataAccess.cs` that maps both systems to human-readable status values:
  - System A (text codes): `PEND` → `Pending`, `PROC` → `Processing`, `SHIP` → `Shipped`, `COMP` → `Completed`, `CANC` → `Cancelled`
  - System B (numeric codes): `1` → `Pending`, `2` → `Processing`, `3` → `Shipped`, `4` → `Completed`, `5` → `Cancelled`
  
**Rationale:** Human-readable statuses are more maintainable, intuitive for users, and easily extendable if new statuses are added.

### 2. Date Format Standardization

**Challenge:** System A uses ISO 8601 format (`YYYY-MM-DD`), System B uses US format (`MM/DD/YYYY`).

**Solution:** Both dates are parsed into `DateTime` objects in the `Order` model and serialized to ISO 8601 format in API responses.

**Rationale:** 
- ISO 8601 is an international standard, eliminating ambiguity
- Language-agnostic and timezone-aware
- Complies with JSON serialization best practices
- Universally recognized by APIs and databases

### 3. System Identification

**Implementation:** Added `SourceSystem` field to unified `Order` model to track which legacy system each order originated from.

**Benefit:** Frontend can display and filter by source system; useful for debugging integration issues and maintaining audit trails.

### 4. In-Memory Data Model

**Architecture:**
- `DataAccess` class loads both JSON and CSV files on application startup
- Data stored as `List<Order>` in memory
- No database required, simplifying deployment for this assessment

**Trade-offs:**
- ✅ Simple, fast, zero configuration
- ✅ Meets assessment requirements
- ❌ Data lost on application restart
- ❌ Not suitable for production with large datasets

### 5. Combined Filtering Logic

**Implementation:** Single `/api/orders/search` endpoint accepts multiple query parameters:
- Status filter works independently
- Date range filter works independently  
- Both filters work in combination (AND logic)

**Code Pattern:**
var query = db.Orders.AsQueryable();
if (!string.IsNullOrEmpty(status)) query = query.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
if (startDate.HasValue && endDate.HasValue) query = query.Where(o => o.OrderDate >= startDate.Value && o.OrderDate <= endDate.Value);
return Ok(query.ToList());


### 6. Frontend Architecture

**Razor Pages Choice:**
- Simplicity: Page-focused rather than controller-action heavy
- Productivity: Built-in tag helpers for forms and validation
- Maintainability: Single file contains presentation and logic

**Communication Pattern:**
- Frontend makes HTTP calls to backend API via `IHttpClientFactory`
- Uses registered `OrdersApi` named client with base address `https://localhost:7143/api/`
- Supports both GET (display all) and POST (search with filters)

### 7. Error Handling Approach

**Current Implementation:**
- API checks HTTP response success before deserializing
- Frontend gracefully defaults to empty order list on API failure
- Missing data files logged (TODO comment in code)

**Limitation:** No user-facing error messages or retry logic for transient failures.

## Time Management

### Time Investment Summary
**Total Time Spent (excluding write-up):** ~10 hours

This exceeds the 3-hour assessment requirement due to scope refinements and enhancements made during development:

**Phase Breakdown:**
1. **Initial Setup & Planning (0.5 hrs)**
   - Solution structure design
   - NuGet package selection (CsvHelper, OpenAPI, Scalar)

2. **Data Integration Layer (2.5 hrs)**
   - Designed DTO classes for both systems
   - Implemented JSON deserialization (System A)
   - Implemented CSV parsing with CsvHelper (System B)
   - Built status normalization logic
   - Created unified `Order` model
   - Testing data mapping with sample records

3. **Backend API Development (2 hrs)**
   - Built `OrdersController` with three endpoints
   - Implemented combined filtering logic (status + date range)
   - Added `HealthController` endpoint
   - Integrated OpenAPI/Scalar documentation

4. **Frontend Development (3 hrs)**
   - Designed `HomeViewModel` with filter properties
   - Built Razor Pages UI with Bootstrap layout
   - Implemented search form with date inputs
   - Added order detail display in side panel
   - Configured `IHttpClientFactory` for API communication
   - Tested frontend-backend integration

5. **Testing & Bug Fixes (1.5 hrs)**
   - Manual API testing via Scalar
   - Frontend filtering validation
   - Cross-system order retrieval verification
   - Identified defects (noted below)

### Prioritization Rationale

**What Was Prioritized:**
1. ✅ All **must-have** requirements (parsing, endpoints, data normalization, frontend)
2. ✅ **Both optional search endpoints** (not just one)
3. ✅ Combined filtering logic

**What Was Deferred (3-hour constraint):**
1. ❌ Unit tests (would have taken 1-2 hours)
2. ❌ Azure App Services deployment
3. ❌ Production error handling and logging
4. ❌ Advanced filtering UI enhancements (issue: selecting order clears filters)

**Why:** Focused on core functionality and documentation quality, as evaluation prioritizes clarity and problem-solving approach over polish.

## Known Limitations

### 1. Filter State Lost on Order Selection
**Issue:** When you click "view" to see order details, the active search filters are cleared, and the full order list is displayed again.

**Root Cause:** The "view" link performs a GET request with `orderId` parameter, which bypasses the POST search logic.

**Impact:** Minor UX friction; user must re-apply filters after viewing details.

**Resolution:** Would require client-side state management or query parameter passing (not implemented due to time constraints).

---

### 2. Data Not Persisted
**Issue:** All order data is loaded into memory on startup and lost when the application restarts.

**Root Cause:** In-memory `List<Order>` collection with no database backing.

**Impact:** Suitable for assessment/demo only; not production-ready.

**Resolution:** Implement relational database (SQL Server/PostgreSQL) with Entity Framework Core.

---

### 3. No Input Validation on Date Range
**Issue:** Selecting an end date before a start date produces unexpected results.

**Root Cause:** Search endpoint applies both date conditions independently; no validation that `startDate <= endDate`.

**Impact:** Silent failure; query returns no results, but no error message displayed.

**Resolution:** Add validation in `OrdersController.OrdersStatusSearch()` method.

---

### 4. Case-Sensitive System A Status Values
**Issue:** If System A JSON contains unexpected status codes (typos, new statuses), they map to "Unknown" silently.

**Root Cause:** Switch statement in `NormalizeStatus()` uses exact matching.

**Impact:** Data quality issues not surfaced to users or logs.

**Resolution:** Add logging and validation when loading System A data.

---

### 5. CSV File Header Requirement
**Issue:** System B CSV parsing assumes strict column order matching `OrderB_DTO` property names.

**Root Cause:** CsvHelper uses case-insensitive property mapping; if headers are renamed or reordered, parsing fails.

**Impact:** Brittle integration; requires exact CSV format match.

**Resolution:** Add schema validation or column mapping flexibility.

---

### 6. No Pagination
**Issue:** API returns all orders (25 total); no pagination implemented.

**Root Cause:** Simple in-memory list doesn't warrant pagination for assessment scope.

**Impact:** Acceptable for current dataset; wouldn't scale for thousands of orders.

**Resolution:** Implement skip/take parameters in API.

---

### 7. No Sorting Capability
**Issue:** Frontend displays orders in database load order (System A, then System B), not user-selectable sort.

**Root Cause:** Not in assessment requirements; would add 30+ minutes of UI work.

**Impact:** Users can't sort by date, amount, customer name, etc.

**Resolution:** Add sortable column headers in frontend table.

---

### 8. Missing Async Exception Handling
**Issue:** Frontend's `HomeController.Index()` uses `.ConfigureAwait(false)` but doesn't handle `HttpRequestException`.

**Root Cause:** Minimal error handling in scope of assessment.

**Impact:** If API is down, user sees generic 500 error instead of friendly message.

**Resolution:** Wrap API calls in try-catch with user-friendly error display.

---

### 9. Hard-Coded API Base Address
**Issue:** Frontend has hardcoded API URL: `https://localhost:7143/api/`

**Root Cause:** Development-focused; intended for local testing.

**Impact:** Frontend breaks when API moves to different port or deployment URL.

**Resolution:** Move to `appsettings.json` with environment-based configuration.

---

### 10. No CORS Configuration
**Issue:** Frontend and API run on different ports; future deployments may encounter CORS issues.

**Root Cause:** Currently both are localhost; CORS not yet required.

**Impact:** Production deployment would need CORS policy configuration in API.

**Resolution:** Add CORS middleware in `backend/osih.api/Program.cs`.

---

## Future Improvements

### High Priority (Would Implement First)

#### 1. Azure App Services Deployment
- Deploy API to Azure App Service (Linux, B1 tier)
- Deploy frontend to separate Azure App Service
- Configure environment-specific `appsettings.json`
- Set up Application Insights for monitoring
- Estimated time: 2-3 hours

#### 2. Filter State Preservation
- Pass filter parameters in "view" link query string
- Use `[BindProperty(SupportsGet = true)]` in `HomeViewModel`
- Preserve filter UI state when viewing order details
- Estimated time: 1 hour

#### 3. Database Persistence (SQL Server)
- Replace in-memory `List<Order>` with Entity Framework Core
- Create `OsihDbContext` with `Orders` DbSet
- Add migrations and initial seed from JSON/CSV
- Estimated time: 3-4 hours

#### 4. Data Validation & Logging
- Add Serilog for structured logging
- Log when System A statuses are unmapped
- Validate date ranges in search endpoint
- Validate start date ≤ end date
- Estimated time: 2 hours

#### 5. Unit Tests
- Create xUnit test project
- Test `DataAccess.NormalizeStatus()` with all status mappings
- Mock `IHttpClientFactory` for frontend tests
- Achieve 70%+ code coverage
- Estimated time: 3-4 hours

---

### Medium Priority

#### 6. Sorting & Pagination
- Add `orderBy` query parameter to API
- Implement skip/take for pagination
- Add sortable column headers in frontend table
- Estimated time: 2 hours

#### 7. Advanced Search UI
- Multi-select status filter (select multiple statuses)
- Customer name search/filter
- Order amount range filter
- Estimated time: 1.5 hours

#### 8. CORS & Security
- Configure CORS policy in API
- Add request logging middleware
- Implement rate limiting
- Estimated time: 1.5 hours

#### 9. Docker Containerization
- Create `Dockerfile` for API and frontend
- Docker Compose for local development
- Deploy to Azure Container Instances
- Estimated time: 2 hours

#### 10. Performance Optimization
- Add response caching for `/api/orders`
- Implement AsNoTracking() for EF queries
- Add query indexes on `OrderDate` and `Status`
- Estimated time: 1.5 hours

---

### Low Priority (Nice-to-Have)

#### 11. Swagger/OpenAPI Enhancements
- Add XML documentation comments to controllers
- Configure OpenAPI security schemes
- Add request/response examples in annotations
- Estimated time: 1 hour

#### 12. Frontend UI Polish
- Responsive grid layout for mobile
- Status badge styling (color-coded by status)
- Loading spinners during API calls
- Toast notifications for errors
- Estimated time: 2-3 hours

#### 13. System Health Dashboard
- Display integration health for both systems
- Last sync timestamp
- Data freshness metrics
- Estimated time: 1.5 hours

#### 14. Automated Data Refresh
- Implement background job to reload JSON/CSV periodically
- Trigger refresh endpoint with admin authentication
- Estimated time: 1.5 hours

#### 15. Advanced Reporting
- Export orders to CSV/Excel
- Generate order summary reports
- Date range revenue analysis
- Estimated time: 2 hours

---

## Additional Notes

### Development Environment
- **IDE:** Visual Studio 2026
- **SDK:** .NET 10
- **Package Manager:** NuGet
- **Version Control:** Git (GitHub)

### Code Quality Observations
- **Strengths:**
  - Clear separation of concerns (DataAccess, Controllers, Views)
  - Proper use of DTOs for data binding
  - RESTful API design
  - Responsive Bootstrap layout
  
- **Areas for Improvement:**
  - Add XML documentation comments for public methods
  - Extract status normalization to separate service class
  - Add dependency injection for `DataAccess` rather than new instances
  - Remove TODO comments before production release

### Deployment Checklist (When Ready)
- [ ] Configure `appsettings.json` for both projects with Azure settings
- [ ] Enable HTTPS only
- [ ] Set up Application Insights monitoring
- [ ] Configure database connection strings in Azure Key Vault
- [ ] Set up CI/CD pipeline (GitHub Actions or Azure Pipelines)
- [ ] Run security vulnerability scan
- [ ] Load testing
- [ ] Documentation update with live URLs

### Support & Questions
For questions about setup or implementation details, refer to:
- API docs: `https://localhost:7143/api/scalar`
- Code comments in `DataAccess.cs` and `OrdersController.cs`
- `requirements.md` for original assessment criteria
