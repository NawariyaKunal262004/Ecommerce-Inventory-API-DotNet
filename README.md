
# 🏥 Medicine Billing &amp; Inventory Management Backend - COMPLETE BACKEND ENGINEERING HANDBOOK

## Table of Contents
1. [Introduction](#1-introduction)
2. [Understanding the Entire Project](#2-understanding-the-entire-project)
3. [Complete Architecture Breakdown](#3-complete-architecture-breakdown)
4. [Backend Request Lifecycle](#4-backend-request-lifecycle)
5. [OOPS Concepts Used in This Project](#5-oops-concepts-used-in-this-project)
6. [ASP.NET Core Fundamentals](#6-aspnet-core-fundamentals)
7. [CQRS Deep Dive](#7-cqrs-deep-dive)
8. [Repository Pattern Deep Dive](#8-repository-pattern-deep-dive)
9. [Entity Framework Core Deep Dive](#9-entity-framework-core-deep-dive)
10. [Database Design Thinking](#10-database-design-thinking)
11. [Authentication &amp; Authorization](#11-authentication--authorization)
12. [Validation System](#12-validation-system)
13. [Global Exception Handling](#13-global-exception-handling)
14. [Dependency Injection Deep Dive](#14-dependency-injection-deep-dive)
15. [Docker &amp; Deployment Basics](#15-docker--deployment-basics)
16. [API Design Best Practices](#16-api-design-best-practices)
17. [Reusable Code &amp; Scalability](#17-reusable-code--scalability)
18. [How to Build Large Projects](#18-how-to-build-large-projects)
19. [Module-by-Module Breakdown](#19-module-by-module-breakdown)
20. [Complete End-to-End Example](#20-complete-end-to-end-example)
21. [Interview Preparation Section](#21-interview-preparation-section)
22. [Beginner to Advanced Backend Roadmap](#22-beginner-to-advanced-backend-roadmap)
23. [Common Mistakes Beginners Make](#23-common-mistakes-beginners-make)
24. [Senior Developer Mindset](#24-senior-developer-mindset)
25. [Final Summary](#25-final-summary)

---

## 1. Introduction

### What is Backend Development?
Backend development is the "behind-the-scenes" work that powers web applications. It's the part of the software that users don't see directly, but it's responsible for:
- Storing and retrieving data from databases
- Processing business logic
- Handling user authentication and authorization
- Communicating with external services
- Sending responses to the frontend

### What Happens Inside Backend?
When you use a frontend (like a website or mobile app), here's what happens in the backend:
1. Frontend sends an HTTP request to the backend
2. Backend receives the request
3. Backend validates the request data
4. Backend processes the request (business logic)
5. Backend interacts with the database
6. Backend sends a response back to the frontend

### Real-world Backend Examples
- When you log into Instagram: backend checks your credentials and returns your feed
- When you order food on Swiggy: backend creates an order, updates inventory, and notifies the restaurant
- When you check your bank balance: backend retrieves your account details from the database

### What Problems Backend Solves
1. **Data Storage**: Where to save user data, orders, etc.
2. **Security**: How to protect data from unauthorized access
3. **Scalability**: How to handle thousands of users at once
4. **Consistency**: How to ensure data is always correct
5. **Integration**: How to connect with other services (payment gateways, SMS services, etc.)

### Why Backend is Difficult
- You have to think about edge cases
- You have to handle failures (database down, network issues)
- You have to optimize for performance
- You have to ensure security
- You have to make it scalable

### Why Architecture Matters
Architecture is the blueprint of your system. A good architecture:
- Makes your code maintainable
- Makes your code testable
- Makes your code scalable
- Makes onboarding new developers easier
- Reduces technical debt

---

## 2. Understanding the Entire Project

### What This Medicine Management System Does
This is a complete backend system for managing a medical store's:
- Inventory (medicines, stock, batches)
- Patients
- Billing (creating bills, generating invoices)
- Users (admin, billers)
- Suppliers
- Authentication &amp; Authorization

### Real-world Use Case
Imagine a local pharmacy with:
- An **Admin** who manages medicines, users, and suppliers
- A **Biller** who creates bills for patients and manages inventory

This system automates all their daily operations.

### Who Uses It
| Role | Permissions |
|------|-------------|
| Admin | Full access - manage medicines, users, suppliers, view all bills, inventory |
| Biller | Create bills, view medicines, view patients, manage inventory |

### Why Modules Exist
Modules help organize code by functionality. Instead of having all code in one place, we split it into:
- Auth module
- Medicines module
- Patients module
- Billing module
- Inventory module

This makes the codebase easier to understand and maintain.

### How Businesses Think
Businesses care about:
1. **Efficiency**: Automate repetitive tasks
2. **Accuracy**: No manual errors in billing or inventory
3. **Security**: Only authorized people can access sensitive data
4. **Reporting**: Get insights into sales, inventory, etc.

---

## 3. Complete Architecture Breakdown

### Clean Architecture
Clean Architecture is a software design pattern that separates concerns into layers. The key principle is the **Dependency Rule**:
> Dependencies should point inwards. Inner layers should not know anything about outer layers.

### Layers
```
┌─────────────────────────────────────────┐
│         Presentation Layer (API)        │ ← Controllers, Middleware
├─────────────────────────────────────────┤
│       Application Layer (Use Cases)     │ ← Commands, Queries, Handlers
├─────────────────────────────────────────┤
│          Domain Layer (Core)            │ ← Entities, Interfaces
├─────────────────────────────────────────┤
│      Infrastructure Layer (Infra)       │ ← Database, Repositories, External Services
└─────────────────────────────────────────┘
```

### Why Separation Matters
1. **Testability**: You can test each layer independently
2. **Maintainability**: Changes in one layer don't affect other layers
3. **Flexibility**: You can swap out databases or frameworks easily
4. **Reusability**: Core logic can be used in different applications

### Why Large Companies Use This Architecture
- It scales well for large teams
- It reduces technical debt
- It makes onboarding easier
- It supports long-term maintenance

---

### Folder Structure &amp; Explanation
```
Medical/
├── Medical.API/                    # Presentation Layer
│   ├── Controllers/                # API endpoints
│   │   ├── MedicinesController.cs
│   │   ├── AuthController.cs
│   │   ├── BillsController.cs
│   │   └── ...
│   ├── Middlewares/                # Custom middleware
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Extensions/                 # Extension methods for Program.cs
│   │   └── AuthExtensions.cs
│   ├── Program.cs                  # Application entry point
│   └── appsettings.json            # Configuration
├── Medical.Application/            # Application Layer
│   ├── Commands/                   # CQRS Commands (write operations)
│   │   ├── CreateMedicineCommand.cs
│   │   ├── CreateBillCommand.cs
│   │   └── ...
│   ├── Queries/                    # CQRS Queries (read operations)
│   │   ├── GetMedicineById.cs
│   │   ├── GetAllBillsQuery.cs
│   │   └── ...
│   ├── Handlers/                   # Command &amp; Query Handlers
│   │   ├── CreateMedicineCommandHandler.cs
│   │   ├── GetMedicineByIdQueryHandler.cs
│   │   └── ...
│   ├── Validators/                 # FluentValidation validators
│   │   ├── CreateMedicineValidator.cs
│   │   └── ...
│   ├── Behaviors/                  # MediatR pipeline behaviors
│   │   ├── ValidationBehavior.cs
│   │   ├── LoggingBehavior.cs
│   │   └── PerformanceBehavior.cs
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Responses/                  # API response models
│   ├── Extensions/                 # Extension methods
│   │   └── ApplicationServices.cs
│   └── Services/                   # Application services
├── Medical.Core/                   # Domain Layer
│   ├── Entity/                     # Domain entities
│   │   ├── MedicineEntity.cs
│   │   ├── BillEntity.cs
│   │   ├── PatientEntity.cs
│   │   └── ...
│   ├── IRepositories/              # Repository interfaces
│   │   ├── IAsyncRepository.cs
│   │   ├── IMedicineRepository.cs
│   │   └── ...
│   ├── Common/                     # Common constants, base classes
│   │   ├── BaseEntity.cs
│   │   ├── AuthConstants.cs
│   │   └── ...
│   └── Settings/                   # Configuration settings
│       └── JwtSettings.cs
└── Medical.Infra/                  # Infrastructure Layer
    ├── Data/                       # Database context
    │   └── ApplicationDbContext.cs
    ├── Repositories/               # Repository implementations
    │   ├── RepositoryBase.cs
    │   ├── MedicineRepository.cs
    │   └── ...
    ├── Extensions/                 # Infrastructure setup
    │   └── InfraService.cs
    └── Migrations/                 # EF Core migrations
```

---

## 4. Backend Request Lifecycle

Let's take a **Create Medicine** request to see the complete flow:

### Complete Flow Diagram
```
Frontend (Postman / Web App)
    ↓ [HTTP POST /api/medicines]
Controller (MedicinesController.CreateMedicine)
    ↓ [_mediator.Send(command)]
MediatR
    ↓ [Routes to CreateMedicineCommandHandler]
Command (CreateMedicineCommand)
    ↓ [Validation via ValidationBehavior]
Handler (CreateMedicineCommandHandler)
    ↓ [Calls repository]
Repository (MedicineRepository.AddAsync)
    ↓ [Uses DbContext]
DbContext (ApplicationDbContext)
    ↓ [SQL INSERT]
Database (SQL Server / PostgreSQL)
    ↓ [Returns created record]
DbContext
    ↓ [Returns entity]
Repository
    ↓ [Returns to handler]
Handler
    ↓ [Maps to response]
MediatR
    ↓ [Returns to controller]
Controller
    ↓ [HTTP 200 OK with response]
Frontend
```

---

### What Each Layer Does

#### 1. Frontend
- **What it is**: The user interface (Postman, web app, mobile app)
- **What it does**: Sends HTTP requests to the backend
- **Why it exists**: To interact with users

#### 2. Controller
**File**: `Medical.API/Controllers/MedicinesController.cs:15-22`

```csharp
[HttpPost]
[Authorize(Roles = AuthConstants.Admin)]
public async Task&lt;IActionResult&gt; CreateMedicine([FromBody] CreateMedicineCommand command)
{
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

**Why it exists**:
- To receive HTTP requests
- To validate authorization (who can access this endpoint)
- To send commands/queries to MediatR
- To return HTTP responses

**What problem it solves**:
- Decouples API endpoint logic from business logic
- Provides a consistent way to handle HTTP requests

**Best Practices**:
- Keep controllers thin (no business logic)
- Use attribute routing
- Use `[ApiController]` for automatic model validation
- Use `[Authorize]` for protected endpoints

**Common Mistakes**:
- Putting business logic in controllers
- Not validating authorization
- Returning raw entities instead of DTOs

---

#### 3. MediatR
- **What it is**: A library that implements the Mediator pattern
- **What it does**: Routes commands/queries to their respective handlers
- **Why it exists**: Decouples the sender (controller) from the receiver (handler)

**How it works internally**:
1. You send a command/query using `_mediator.Send()`
2. MediatR looks for a handler that implements `IRequestHandler&lt;TCommand, TResponse&gt;`
3. MediatR executes the handler
4. MediatR returns the response back to the sender

---

#### 4. Command (CQRS)
**File**: `Medical.Application/Commands/CreateMedicineCommand.cs`

```csharp
public class CreateMedicineCommand : IRequest&lt;ResponseModel&gt;
{
    public string? MedicineName { get; set; }
    public string? MedicineCategory { get; set; }
    public decimal MedicinePrice { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public Guid SupplierId { get; set; }
    public DateOnly ManufacturingDate { get; set; }
    public string? Manufacturer { get; set; } = null;
}
```

**Why it exists**:
- Represents a write operation (create, update, delete)
- Carries all the data needed for the operation
- Implements `IRequest&lt;TResponse&gt;` so MediatR can route it

---

#### 5. Handler
**File**: `Medical.Application/Handlers/CreateMedicineCommandHandler.cs`

```csharp
public class CreateMedicineCommandHandler : IRequestHandler&lt;CreateMedicineCommand, ResponseModel&gt;
{
    private readonly IMedicineRepository _repository;

    public CreateMedicineCommandHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task&lt;ResponseModel&gt; Handle(CreateMedicineCommand request, CancellationToken cancellationToken)
    {
        var medicine = new MedicineEntity
        {
            MedicineId = Guid.NewGuid(),
            MedicineName = request.MedicineName,
            MedicineCategory = request.MedicineCategory,
            MedicinePrice = request.MedicinePrice,
            Stock = 0,
            ExpirationDate = request.ExpirationDate,
            SupplierId = request.SupplierId,
            ManufacturingDate = request.ManufacturingDate,
            Manufacturer = request.Manufacturer,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        var createdMedicine = await _repository.AddAsync(medicine);

        var response = new MedicineResponse
        {
            MedicineId = createdMedicine.MedicineId,
            MedicineName = createdMedicine.MedicineName,
            // ... other properties
        };

        return ResponseModel.SuccessResponse(response, "Medicine created successfully");
    }
}
```

**Why it exists**:
- Contains the business logic for the operation
- Uses repositories to interact with the database
- Maps commands to entities and entities to responses

---

#### 6. Repository
**File**: `Medical.Infra/Repositories/MedicineRepository.cs`

```csharp
public class MedicineRepository : RepositoryBase&lt;MedicineEntity&gt;, IMedicineRepository
{
    public MedicineRepository(ApplicationDbContext context, ILogger&lt;RepositoryBase&lt;MedicineEntity&gt;&gt; logger) : base(context, logger)
    {
    }

    public async Task&lt;IReadOnlyList&lt;MedicineEntity&gt;&gt; GetLowStockAsync()
    {
        return await _context.Medicines
            .Where(m =&gt; m.Stock &lt; 10)
            .ToListAsync();
    }

    public async Task&lt;IReadOnlyList&lt;MedicineEntity&gt;&gt; GetExpiringAsync()
    {
        var expiryThreshold = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
        
        return await _context.Medicines
            .Where(m =&gt; m.ExpirationDate &lt;= expiryThreshold &amp;&amp; m.ExpirationDate &gt;= DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync();
    }
}
```

**Why it exists**:
- Abstracts database operations
- Provides a consistent interface for data access
- Contains query logic specific to the entity

---

#### 7. DbContext
**File**: `Medical.Infra/Data/ApplicationDbContext.cs:13`

```csharp
public DbSet&lt;MedicineEntity&gt; Medicines { get; set; }
```

**Why it exists**:
- Represents a session with the database
- Manages database connections
- Tracks entity changes
- Executes SQL queries

---

#### 8. Database
- **What it is**: Persistent storage for your data
- **What it does**: Stores tables, relationships, and data
- **Why it exists**: To keep data even when the application is restarted

---

## 5. OOPS Concepts Used in This Project

### Class
**Definition**: A blueprint for creating objects. It defines properties (data) and methods (behavior).

**Real-world Analogy**: A class is like a recipe for making cookies. The recipe (class) tells you what ingredients (properties) you need and how to mix them (methods).

**Where Used in Project**: `MedicineEntity`, `CreateMedicineCommand`, `MedicineRepository`, etc.

**Example Code**:
```csharp
public class MedicineEntity
{
    public Guid MedicineId { get; set; }
    public string MedicineName { get; set; }
    public decimal MedicinePrice { get; set; }
}
```

---

### Object
**Definition**: An instance of a class. It's a concrete thing created from the class blueprint.

**Real-world Analogy**: An object is like a batch of cookies made from the recipe (class).

**Where Used in Project**: When you create a `new MedicineEntity()`, that's an object.

**Example Code**:
```csharp
var medicine = new MedicineEntity
{
    MedicineId = Guid.NewGuid(),
    MedicineName = "Paracetamol",
    MedicinePrice = 10.50m
};
```

---

### Encapsulation
**Definition**: Hiding internal data and exposing only what's necessary.

**Real-world Analogy**: A car - you don't need to know how the engine works to drive it. You only need the steering wheel, pedals, and gear shift.

**Where Used in Project**: `RepositoryBase` - it has a protected `_context` field that's only accessible to derived classes.

**Why Used**:
- Prevents accidental modification of internal state
- Makes code more maintainable
- Provides a clean interface

---

### Abstraction
**Definition**: Hiding complex implementation details and showing only the essential features.

**Real-world Analogy**: A remote control - you press buttons without knowing how it communicates with the TV.

**Where Used in Project**: `IMedicineRepository` interface - it defines what methods are available, but not how they're implemented.

**Example Code**:
```csharp
public interface IMedicineRepository : IAsyncRepository&lt;MedicineEntity&gt;
{
    Task&lt;IReadOnlyList&lt;MedicineEntity&gt;&gt; GetLowStockAsync();
    Task&lt;IReadOnlyList&lt;MedicineEntity&gt;&gt; GetExpiringAsync();
}
```

---

### Inheritance
**Definition**: A class (derived class) inherits properties and methods from another class (base class).

**Real-world Analogy**: A child inheriting traits from their parents.

**Where Used in Project**: `MedicineRepository` inherits from `RepositoryBase&lt;MedicineEntity&gt;`.

**Example Code**:
```csharp
public class MedicineRepository : RepositoryBase&lt;MedicineEntity&gt;, IMedicineRepository
{
    public MedicineRepository(ApplicationDbContext context, ILogger&lt;RepositoryBase&lt;MedicineEntity&gt;&gt; logger) 
        : base(context, logger)
    {
    }
}
```

**Why Used**:
- Promotes code reuse
- Establishes a hierarchy
- Makes code more maintainable

---

### Polymorphism
**Definition**: The ability of objects to be treated as instances of their base class.

**Real-world Analogy**: A USB port - you can plug in a mouse, keyboard, or printer, and they all work.

**Where Used in Project**: MediatR handlers - you can have multiple handlers for different requests, but they all implement `IRequestHandler`.

---

### Interface
**Definition**: A contract that defines methods and properties a class must implement.

**Real-world Analogy**: A job description - it defines what tasks you need to do, but not how to do them.

**Where Used in Project**: `IAsyncRepository&lt;T&gt;`, `IMedicineRepository`, `IBillRepository`, etc.

**Example Code**:
```csharp
public interface IAsyncRepository&lt;T&gt; where T : class
{
    Task&lt;T?&gt; GetByIdAsync(Guid id);
    Task&lt;IReadOnlyList&lt;T&gt;&gt; GetAllAsync();
    Task&lt;T&gt; AddAsync(T entity);
    Task&lt;T&gt; UpdateAsync(T entity);
    Task&lt;bool&gt; DeleteAsync(Guid id);
}
```

---

### Dependency Injection (DI)
**Definition**: A technique where an object receives its dependencies instead of creating them itself.

**Real-world Analogy**: Instead of you making your own coffee every morning, someone brings it to you.

**Where Used in Project**: Controllers, Handlers, Repositories - they all receive dependencies via constructor.

**Example Code**:
```csharp
public class MedicinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicinesController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
```

---

### Composition
**Definition**: Building complex objects by combining simpler objects.

**Real-world Analogy**: A computer is composed of a CPU, RAM, hard drive, etc.

**Where Used in Project**: `BillEntity` has a collection of `BillItemEntity`.

**Example Code**:
```csharp
public class BillEntity
{
    public Guid BillId { get; set; }
    public ICollection&lt;BillItemEntity&gt; BillItems { get; set; } = new List&lt;BillItemEntity&gt;();
}
```

---

### Constructor Injection
**Definition**: A type of dependency injection where dependencies are passed through the constructor.

**Where Used in Project**: Almost every class in the project!

**Example Code**: See the MedicinesController example above.

---

### SOLID Principles

#### S - Single Responsibility Principle (SRP)
**Definition**: A class should have only one reason to change.

**Where Used in Project**: 
- Controllers only handle HTTP requests
- Handlers only handle business logic
- Repositories only handle data access

#### O - Open/Closed Principle (OCP)
**Definition**: Software entities should be open for extension but closed for modification.

**Where Used in Project**: Repository pattern - you can add new repository implementations without changing existing code.

#### L - Liskov Substitution Principle (LSP)
**Definition**: Derived classes should be substitutable for their base classes.

**Where Used in Project**: `MedicineRepository` can be used wherever `IAsyncRepository&lt;MedicineEntity&gt;` is expected.

#### I - Interface Segregation Principle (ISP)
**Definition**: Clients should not be forced to depend on interfaces they don't use.

**Where Used in Project**: `IMedicineRepository` extends `IAsyncRepository&lt;MedicineEntity&gt;` and adds only the methods it needs.

#### D - Dependency Inversion Principle (DIP)
**Definition**: Depend on abstractions, not concretions.

**Where Used in Project**: Handlers depend on `IMedicineRepository` (interface), not `MedicineRepository` (concrete class).

---

## 6. ASP.NET Core Fundamentals

### Controllers
Controllers are the entry point for HTTP requests. They handle routing, model binding, and return responses.

**File**: `Medical.API/Controllers/MedicinesController.cs`

### APIs
APIs (Application Programming Interfaces) allow different software systems to communicate. Our project uses REST APIs.

### Routing
Routing determines which controller action should handle a request.

**Example**:
```csharp
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    // This handles GET /api/medicines
    [HttpGet]
    public async Task&lt;IActionResult&gt; GetAllMedicines() { ... }
}
```

### HTTP Methods
| Method | Purpose | Idempotent? |
|--------|---------|-------------|
| GET | Retrieve data | Yes |
| POST | Create data | No |
| PUT | Update data | Yes |
| DELETE | Delete data | Yes |

### Middleware
Middleware is software that's assembled into an application pipeline to handle requests and responses.

**File**: `Medical.API/Middlewares/GlobalExceptionMiddleware.cs`

**How it works**:
1. Request comes in
2. Middleware 1 processes it
3. Middleware 2 processes it
4. ...
5. Controller handles it
6. Response goes back through the middleware pipeline

### Dependency Injection
ASP.NET Core has built-in dependency injection. We register services in `Program.cs`.

### Services
Services are reusable components that provide functionality.

### Configuration
Configuration is stored in `appsettings.json` and `appsettings.Development.json`.

### appsettings.json
**File**: `Medical.API/appsettings.json`

### Program.cs
**File**: `Medical.API/Program.cs`

This is the entry point of the application. It sets up:
- Controllers
- OpenAPI/Swagger
- Application services
- Authentication
- Middleware pipeline

### Extension Methods
Extension methods allow you to add methods to existing types without modifying them.

**File**: `Medical.Application/Extensions/ApplicationServices.cs`

### Swagger
Swagger is a tool that documents APIs and provides a UI to test them.

### DTOs (Data Transfer Objects)
DTOs are objects that carry data between layers. They prevent exposing internal entities directly.

**File**: `Medical.Application/DTOs/AddBillItemRequest.cs`

### Model Binding
Model binding is the process of converting HTTP request data into .NET objects.

---

## 7. CQRS Deep Dive

### What is CQRS?
CQRS stands for **Command Query Responsibility Segregation**. It's a pattern that separates read operations (queries) from write operations (commands).

### Why Use CQRS?
1. **Different Scalability Needs**: Reads are usually more frequent than writes - you can scale them independently
2. **Different Optimization**: You can optimize read models for queries and write models for commands
3. **Clearer Code**: Separating concerns makes code easier to understand

### Commands
Commands represent **write operations** (create, update, delete). They change the state of the system.

**Example**: `CreateMedicineCommand`

### Queries
Queries represent **read operations** (get, list). They don't change the state of the system.

**Example**: `GetMedicineById`

### Handlers
Handlers contain the logic for executing commands and queries.

### IRequest
`IRequest&lt;T&gt;` is an interface from MediatR that marks a class as a request (command or query).

### IRequestHandler
`IRequestHandler&lt;TRequest, TResponse&gt;` is an interface that handles a specific request.

### MediatR
MediatR is a library that implements the Mediator pattern. It routes requests to their handlers.

### Validation Flow
Validation happens in a MediatR pipeline behavior before the handler is executed.

**File**: `Medical.Application/Behaviors/ValidationBehavior.cs`

---

## 8. Repository Pattern Deep Dive

### What Are Repositories?
Repositories are classes that encapsulate data access logic. They act as a middle layer between the business logic and the database.

### Why IRepository Exists?
The interface defines the contract for data access. This allows us to:
- Have multiple implementations (e.g., in-memory for testing, EF Core for production)
- Depend on abstractions instead of concretions (Dependency Inversion Principle)

### Why Implementation is Separated?
Separation allows us to change the data access technology without changing the business logic.

### Generic Repository
A generic repository works with any entity type.

**File**: `Medical.Core/IRepositories/IAsyncRepository.cs`

### Custom Repository
A custom repository extends the generic repository with entity-specific methods.

**File**: `Medical.Core/IRepositories/IMedicineRepository.cs`

### Async Methods
All repository methods are async to avoid blocking the main thread.

### Reusability
The generic repository is reusable across all entities.

---

## 9. Entity Framework Core Deep Dive

### DbContext
`DbContext` is the main class that interacts with the database. It manages database connections and tracks entity changes.

**File**: `Medical.Infra/Data/ApplicationDbContext.cs`

### DbSet
`DbSet&lt;T&gt;` represents a collection of entities in the database.

### Migrations
Migrations are a way to evolve the database schema over time.

### Relationships
EF Core supports different types of relationships:

#### One-to-Many
**Example**: One Supplier has many Medicines

**File**: `Medical.Infra/Data/ApplicationDbContext.cs:51-54`
```csharp
entity.HasMany(s =&gt; s.Medicines)
    .WithOne(m =&gt; m.Supplier)
    .HasForeignKey(m =&gt; m.SupplierId)
    .OnDelete(DeleteBehavior.Restrict);
```

#### Many-to-Many
Not used in this project, but EF Core supports it.

### Tracking
EF Core tracks changes to entities so it can generate the appropriate SQL commands.

### LINQ
You can use LINQ (Language Integrated Query) to query the database.

### Async Database Operations
All EF Core operations are async for better performance.

---

## 10. Database Design Thinking

### How to Design Tables
1. **Identify Entities**: What are the main things you need to store? (Medicine, Patient, Bill, etc.)
2. **Define Properties**: What information do you need about each entity?
3. **Establish Relationships**: How are entities related to each other?
4. **Normalize**: Reduce data redundancy

### How Relationships Are Planned
- **One-to-Many**: One supplier has many medicines
- **One-to-Many**: One patient has many bills
- **One-to-Many**: One bill has many bill items

### Why Normalization Matters
Normalization reduces data redundancy and improves data integrity.

### Why Foreign Keys Matter
Foreign keys ensure referential integrity - you can't have a bill for a non-existent patient.

### Billing Relationships
- `BillEntity` has a foreign key to `PatientEntity`
- `BillItemEntity` has a foreign key to `BillEntity`
- `BillItemEntity` has a foreign key to `MedicineEntity`

### Inventory Relationships
- `MedicineBatchEntity` has a foreign key to `MedicineEntity`
- `InventoryTransactionEntity` has a foreign key to `MedicineBatchEntity`

### Stock Management Logic
- When a bill is created, stock is deducted
- When a batch is added, stock is increased
- Low stock alerts when stock &lt; 10

---

## 11. Authentication &amp; Authorization

### Complete Auth System
Let's break down how authentication and authorization work in this project.

### Login
**File**: `Medical.API/Controllers/AuthController.cs:11-17`

### Register
**File**: `Medical.API/Controllers/AuthController.cs:19-25`

### JWT
JWT stands for **JSON Web Token**. It's a compact, URL-safe way to represent claims between two parties.

### Claims
Claims are statements about an entity (user) and additional data. Examples:
- User ID
- Username
- Role

### Roles
Roles define what a user can do. Our project has two roles:
- Admin
- Biller

### Access Tokens
Access tokens are JWTs that are sent in the HTTP header to authenticate requests.

### Authentication Middleware
Authentication middleware validates the JWT token.

### Authorization
Authorization determines what an authenticated user can do.

### Policies
Policies are a way to define complex authorization rules.

### Role-Based Access
**File**: `Medical.API/Controllers/MedicinesController.cs:16`
```csharp
[Authorize(Roles = AuthConstants.Admin)]
public async Task&lt;IActionResult&gt; CreateMedicine([FromBody] CreateMedicineCommand command) { ... }
```

### Admin Permissions
- Manage medicines
- Manage users
- Manage suppliers
- View all bills
- View inventory

### Biller Permissions
- Create bills
- View medicines
- View patients
- Manage inventory

### Complete JWT Flow
1. User sends login request with username and password
2. Backend validates credentials
3. Backend generates a JWT token
4. Backend returns the token to the frontend
5. Frontend sends the token in the `Authorization` header for subsequent requests
6. Authentication middleware validates the token
7. Authorization middleware checks if the user has the right role/permissions

---

## 12. Validation System

### Validators
Validators ensure that input data is correct before it's processed.

### FluentValidation
FluentValidation is a library for building strongly-typed validation rules.

**File**: `Medical.Application/Validators/CreateMedicineValidator.cs`

### Why Validation Matters
- Prevents invalid data from entering the system
- Provides clear error messages
- Improves security

### Request Validation Flow
1. Request comes in
2. MediatR's `ValidationBehavior` runs
3. Validator checks the request
4. If validation fails, returns error response
5. If validation passes, proceeds to handler

### Business Rule Validation
Business rule validation ensures that the operation complies with business rules. Example:
- Can't deduct more stock than available
- Can't use expired medicine

---

## 13. Global Exception Handling

### Why Exceptions Happen
Exceptions happen when something goes wrong:
- Invalid input
- Database connection failed
- User not authorized
- etc.

### Middleware
Global exception handling is implemented as middleware.

**File**: `Medical.API/Middlewares/GlobalExceptionMiddleware.cs`

### Custom Exceptions
Custom exceptions make error handling more specific.

**File**: `Medical.Application/Exceptions/InsufficientStockException.cs`

### Error Responses
Error responses provide a consistent format for errors.

### Logging Thinking
Logging helps you debug issues in production. You should log:
- Errors
- Warnings
- Important events

---

## 14. Dependency Injection Deep Dive

### What DI Solves
1. **Tight Coupling**: Reduces dependencies between classes
2. **Testability**: Makes it easy to mock dependencies for testing
3. **Maintainability**: Makes code easier to change

### Tight Coupling vs Loose Coupling
- **Tight Coupling**: A class creates its own dependencies
- **Loose Coupling**: A class receives its dependencies (DI)

### Service Lifetimes
| Lifetime | Description | Use Case |
|----------|-------------|----------|
| Transient | Created every time they're requested | Lightweight, stateless services |
| Scoped | Created once per request | Database contexts, repositories |
| Singleton | Created once and reused | Configuration, caches |

**File**: `Medical.Application/Extensions/ApplicationServices.cs:18-31`
```csharp
services.AddScoped&lt;IMedicineRepository, MedicineRepository&gt;();
services.AddScoped&lt;IBillRepository, BillRepository&gt;();
services.AddScoped&lt;IPatientRepository, PatientRepository&gt;();
// ...
```

---

## 15. Docker &amp; Deployment Basics

### Docker
Docker is a platform for developing, shipping, and running applications in containers.

### Containers
Containers are lightweight, standalone, executable packages that include everything needed to run an application.

### Dockerfile
A Dockerfile is a text file that contains instructions to build a Docker image.

### docker-compose
Docker Compose is a tool for defining and running multi-container Docker applications.

### Why Containers Matter
- **Consistency**: Runs the same everywhere
- **Isolation**: Applications don't interfere with each other
- **Portability**: Easy to move between environments
- **Scalability**: Easy to scale horizontally

### Backend Deployment Thinking
- Use CI/CD pipelines for automated deployments
- Use environment variables for configuration
- Monitor the application in production
- Have a backup strategy

---

## 16. API Design Best Practices

### REST APIs
REST (Representational State Transfer) is an architectural style for designing APIs.

### Naming Conventions
- Use nouns for resource names: `/api/medicines`, `/api/patients`
- Use plural nouns
- Use kebab-case or snake_case for multi-word names

### Response Structures
Use a consistent response structure:
```json
{
  "success": true,
  "message": "Medicine created successfully",
  "data": { ... }
}
```

### Status Codes
| Status Code | Meaning |
|-------------|---------|
| 200 OK | Success |
| 201 Created | Resource created |
| 400 Bad Request | Invalid input |
| 401 Unauthorized | Not authenticated |
| 403 Forbidden | Authenticated but not authorized |
| 404 Not Found | Resource not found |
| 500 Internal Server Error | Server error |

### Pagination
For large datasets, use pagination:
- `page`: Page number
- `pageSize`: Number of items per page

### Filtering
Allow filtering results:
- `/api/medicines?category=painkiller`

### Versioning
Version your APIs to avoid breaking changes:
- `/api/v1/medicines`
- `/api/v2/medicines`

---

## 17. Reusable Code &amp; Scalability

### What Code Should Be Reusable
- Generic repositories
- Base classes
- Extension methods
- Common responses
- Validation rules

### Generic Patterns
Generic patterns allow you to write code that works with multiple types.

### Shared Utilities
Shared utilities are helper classes that provide common functionality.

### Shared Responses
Shared responses provide a consistent format for API responses.

### Extension Methods
Extension methods allow you to add methods to existing types.

### Scalable Architecture
- Use Clean Architecture
- Separate read and write models (CQRS)
- Use caching
- Use async/await
- Design for horizontal scaling

---

## 18. How to Build Large Projects

### How Seniors Start Projects
1. **Requirement Analysis**: Understand what the project needs to do
2. **Module Planning**: Break the project into modules
3. **Folder Planning**: Design the folder structure
4. **Entity Planning**: Design the database entities
5. **API Planning**: Design the API endpoints
6. **Database Planning**: Design the database schema
7. **Scaling Planning**: Plan for scalability

### Requirement Analysis
Talk to stakeholders to understand:
- What features are needed
- Who will use the system
- What are the constraints

### Module Planning
Break the project into modules based on functionality:
- Auth module
- Medicines module
- Patients module
- Billing module
- Inventory module

### Folder Planning
Use Clean Architecture to organize the folders:
- Core (Domain)
- Application
- Infrastructure
- API (Presentation)

### Entity Planning
Identify the main entities:
- Medicine
- Patient
- Bill
- BillItem
- Supplier
- User

### API Planning
Design the API endpoints:
- `POST /api/auth/login`
- `POST /api/medicines`
- `GET /api/medicines`
- `POST /api/bills`
- etc.

### Database Planning
Design the database schema:
- Tables
- Columns
- Relationships
- Indexes

### Scaling Planning
- Plan for horizontal scaling
- Use caching
- Use async/await
- Design for failure

---

## 19. Module-by-Module Breakdown

### Medicines Module
**Entities**: `MedicineEntity`
**APIs**:
- `POST /api/medicines` - Create medicine
- `PUT /api/medicines/{id}` - Update medicine
- `DELETE /api/medicines/{id}` - Delete medicine
- `GET /api/medicines` - Get all medicines
- `GET /api/medicines/{id}` - Get medicine by ID
- `GET /api/medicines/low-stock` - Get low stock medicines
- `GET /api/medicines/expiring` - Get expiring medicines

**Handlers**:
- `CreateMedicineCommandHandler`
- `UpdateMedicineCommandHandler`
- `DeleteMedicineCommandHandler`
- `GetAllMedicineListQueryHandler`
- `GetMedicineByIdQueryHandler`
- `GetLowStockQueryHandler`
- `GetExpiringQueryHandler`

**Repositories**: `IMedicineRepository`, `MedicineRepository`

**Validators**: `CreateMedicineValidator`, `UpdateMedicineValidator`

### Patients Module
**Entities**: `PatientEntity`
**APIs**:
- `POST /api/patients` - Create patient
- `PUT /api/patients/{id}` - Update patient
- `DELETE /api/patients/{id}` - Delete patient
- `GET /api/patients` - Get all patients
- `GET /api/patients/{id}` - Get patient by ID
- `GET /api/patients/{id}/history` - Get patient history

### Billing Module
**Entities**: `BillEntity`, `BillItemEntity`
**APIs**:
- `POST /api/bills` - Create bill
- `PUT /api/bills/{id}` - Update bill
- `DELETE /api/bills/{id}` - Delete bill
- `GET /api/bills` - Get all bills
- `GET /api/bills/{id}` - Get bill by ID

### Inventory Module
**Entities**: `MedicineBatchEntity`, `InventoryTransactionEntity`, `StockAdjustmentEntity`
**APIs**:
- `POST /api/inventory/batches` - Add medicine batch
- `PUT /api/inventory/batches/{id}` - Update medicine batch
- `DELETE /api/inventory/batches/{id}` - Delete medicine batch
- `GET /api/inventory/batches` - Get all batches
- `GET /api/inventory/summary` - Get inventory summary
- `GET /api/inventory/transactions` - Get inventory transactions

### Auth Module
**Entities**: `ApplicationUser`, `ApplicationRole`
**APIs**:
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Register (Admin only)
- `POST /api/auth/change-password` - Change password
- `POST /api/auth/reset-password` - Reset password (Admin only)
- `GET /api/auth/current-user` - Get current user

### Users Module
**Entities**: `ApplicationUser`
**APIs**:
- `GET /api/users` - Get all users (Admin only)
- `GET /api/users/{id}` - Get user by ID (Admin only)
- `PUT /api/users/{id}` - Update user (Admin only)
- `DELETE /api/users/{id}` - Delete user (Admin only)

---

## 20. Complete End-to-End Example

Let's walk through the **Generate Bill** flow to see how everything connects together.

### Step 1: Frontend Request
Frontend sends a POST request to `/api/bills` with the bill data:
```json
{
  "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "billItems": [
    {
      "medicineId": "3fa85f64-5717-4562-b3fc-2c963f66afb1",
      "quantity": 2
    }
  ],
  "discount": 0,
  "tax": 0
}
```

### Step 2: Controller
`BillsController.CreateBill` receives the request:
```csharp
[HttpPost]
[Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
public async Task&lt;IActionResult&gt; CreateBill([FromBody] CreateBillCommand command)
{
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

### Step 3: MediatR
MediatR routes the command to `CreateBillCommandHandler`.

### Step 4: Validation
`ValidationBehavior` runs the validator for `CreateBillCommand`.

### Step 5: Handler
`CreateBillCommandHandler` executes the business logic:
1. Validates that all medicines exist
2. Validates that there's enough stock
3. Creates a new `BillEntity`
4. Creates `BillItemEntity` for each item
5. Deducts stock for each medicine
6. Saves everything to the database
7. Maps the bill to a response DTO
8. Returns the response

### Step 6: Repository
The handler uses `IBillRepository`, `IMedicineRepository`, and `IBillInventoryService` to interact with the database.

### Step 7: DbContext
`ApplicationDbContext` tracks the changes and executes the SQL commands:
- INSERT INTO Bills
- INSERT INTO BillItems
- UPDATE Medicines SET Stock = Stock - quantity
- INSERT INTO InventoryTransactions

### Step 8: Database
The database executes the SQL commands and commits the transaction.

### Step 9: Response
The handler maps the created bill to a response DTO and returns it.

### Step 10: Frontend
The frontend receives the response and displays the bill to the user.

---

## 21. Interview Preparation Section

### Most Asked Backend Interview Questions

#### 1. What is Clean Architecture?
**Answer**: Clean Architecture is a software design pattern that separates concerns into layers. The key principle is the Dependency Rule: dependencies should point inwards. Inner layers should not know anything about outer layers.

#### 2. What is CQRS?
**Answer**: CQRS stands for Command Query Responsibility Segregation. It's a pattern that separates read operations (queries) from write operations (commands). This allows you to optimize read and write models independently.

#### 3. What is the Repository Pattern?
**Answer**: The Repository Pattern is a design pattern that encapsulates data access logic. It acts as a middle layer between the business logic and the database, providing a consistent interface for data access.

#### 4. What is Dependency Injection?
**Answer**: Dependency Injection is a technique where an object receives its dependencies instead of creating them itself. This promotes loose coupling, testability, and maintainability.

#### 5. What are the service lifetimes in ASP.NET Core?
**Answer**:
- **Transient**: Created every time they're requested
- **Scoped**: Created once per request
- **Singleton**: Created once and reused

#### 6. What is JWT?
**Answer**: JWT stands for JSON Web Token. It's a compact, URL-safe way to represent claims between two parties. JWTs are commonly used for authentication.

#### 7. What is the difference between Authentication and Authorization?
**Answer**:
- **Authentication**: Verifies who you are
- **Authorization**: Verifies what you can do

#### 8. What is Entity Framework Core?
**Answer**: Entity Framework Core is an ORM (Object-Relational Mapper) for .NET. It allows you to work with databases using .NET objects, eliminating the need for most of the data-access code.

#### 9. What is Middleware?
**Answer**: Middleware is software that's assembled into an application pipeline to handle requests and responses. Each middleware component can either pass the request to the next middleware or short-circuit the pipeline.

#### 10. What are SOLID Principles?
**Answer**:
- **S** - Single Responsibility Principle: A class should have only one reason to change
- **O** - Open/Closed Principle: Software entities should be open for extension but closed for modification
- **L** - Liskov Substitution Principle: Derived classes should be substitutable for their base classes
- **I** - Interface Segregation Principle: Clients should not be forced to depend on interfaces they don't use
- **D** - Dependency Inversion Principle: Depend on abstractions, not concretions

---

## 22. Beginner to Advanced Backend Roadmap

### 3 Month Roadmap
**Month 1: Fundamentals**
- Learn C# basics
- Learn OOP concepts
- Learn ASP.NET Core basics
- Build a simple CRUD API

**Month 2: Intermediate**
- Learn Entity Framework Core
- Learn SQL
- Learn Repository Pattern
- Learn Dependency Injection
- Learn Authentication &amp; Authorization

**Month 3: Advanced**
- Learn Clean Architecture
- Learn CQRS &amp; MediatR
- Learn Validation
- Learn Exception Handling
- Learn Docker basics
- Build a complete project (like this medicine management system)

### 6 Month Roadmap
**Months 1-3**: Follow the 3-month roadmap above

**Month 4: Scalability &amp; Performance**
- Learn caching
- Learn async/await best practices
- Learn database optimization
- Learn load balancing

**Month 5: DevOps**
- Learn CI/CD pipelines
- Learn cloud basics (AWS/Azure/GCP)
- Learn monitoring &amp; logging
- Learn container orchestration (Kubernetes basics)

**Month 6: System Design**
- Learn system design basics
- Learn how to design scalable systems
- Learn about microservices
- Build a scalable project

### 1 Year Roadmap
**Months 1-6**: Follow the 6-month roadmap above

**Months 7-9: Specialization**
- Choose a specialization (e.g., security, performance, DevOps)
- Learn advanced topics in your specialization
- Contribute to open-source projects

**Months 10-12: Leadership**
- Learn how to mentor junior developers
- Learn how to lead projects
- Learn how to make architectural decisions
- Start building your personal brand

---

## 23. Common Mistakes Beginners Make

### 1. Tightly Coupled Code
**Mistake**: Classes create their own dependencies instead of receiving them via DI.
**Solution**: Use Dependency Injection.

### 2. Fat Controllers
**Mistake**: Putting business logic in controllers.
**Solution**: Keep controllers thin - use MediatR and handlers for business logic.

### 3. Bad Architecture
**Mistake**: Not using any architecture pattern.
**Solution**: Use Clean Architecture or Onion Architecture.

### 4. Poor Naming
**Mistake**: Using unclear or inconsistent names.
**Solution**: Follow naming conventions - use descriptive names.

### 5. Duplicate Code
**Mistake**: Copy-pasting code instead of reusing it.
**Solution**: Create reusable components - generic repositories, base classes, extension methods.

### 6. Missing Validations
**Mistake**: Not validating input data.
**Solution**: Use FluentValidation to validate all requests.

### 7. Bad Database Design
**Mistake**: Not normalizing the database, not using foreign keys.
**Solution**: Learn database normalization, use foreign keys for referential integrity.

### 8. Not Using Async/Await
**Mistake**: Using synchronous methods for I/O operations.
**Solution**: Use async/await for all I/O operations (database calls, HTTP requests, etc.).

### 9. Not Logging
**Mistake**: Not logging errors or important events.
**Solution**: Use a logging framework (like Serilog) and log errors, warnings, and important events.

### 10. Not Testing
**Mistake**: Not writing tests.
**Solution**: Write unit tests and integration tests.

---

## 24. Senior Developer Mindset

### How Seniors Think
1. **Maintainability**: Code should be easy to maintain - even by someone else
2. **Scalability**: System should handle growth
3. **Testability**: Code should be easy to test
4. **Reusability**: Code should be reusable
5. **Failure**: Assume things will fail - plan for it
6. **Security**: Security is a priority, not an afterthought
7. **Documentation**: Document why, not what
8. **Collaboration**: Work with the team, not alone
9. **Continuous Learning**: Always be learning
10. **Simplicity**: Keep it simple - avoid over-engineering

### Scalability Mindset
- Design for horizontal scaling
- Use caching
- Use async/await
- Optimize database queries
- Use message queues for long-running tasks

### Maintainability Mindset
- Use Clean Architecture
- Follow SOLID principles
- Write clean code
- Use meaningful names
- Keep functions small and focused

### Debugging Mindset
- Reproduce the issue first
- Use logs to understand what's happening
- Don't guess - use a debugger
- Test your fix thoroughly
- Learn from the issue - prevent it from happening again

### Production Mindset
- Monitor the application in production
- Have an alerting system
- Have a backup strategy
- Have a rollback plan
- Test in production (carefully!)

---

## 25. Final Summary

### How Everything Connects Together
Let's recap the complete flow:
1. Frontend sends an HTTP request
2. Controller receives the request
3. Controller sends a command/query to MediatR
4. MediatR routes it to the appropriate handler
5. Handler uses repositories to interact with the database
6. Repositories use DbContext to execute SQL commands
7. Database returns the results
8. Handler maps the results to a response DTO
9. Controller returns the response to the frontend

### How Backend Systems Are Built
1. Start with requirement analysis
2. Design the architecture (Clean Architecture)
3. Design the database schema
4. Implement the domain layer (entities, interfaces)
5. Implement the application layer (commands, queries, handlers)
6. Implement the infrastructure layer (repositories, DbContext)
7. Implement the presentation layer (controllers, middleware)
8. Test everything
9. Deploy to production

### What to Learn Next
- System design
- Microservices
- Message queues (RabbitMQ, Kafka)
- Caching (Redis)
- Search engines (Elasticsearch)
- Advanced security
- Performance optimization
- Cloud computing (AWS, Azure, GCP)
- CI/CD pipelines
- Monitoring and observability

---

**Congratulations!** You've completed the backend engineering handbook. Now go build amazing things! 🚀

# Medicine-Learning
