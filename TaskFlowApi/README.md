# TaskFlow — Backend API

TaskFlow is a project and task management application inspired by tools like Trello and Jira. It helps users organize projects, manage tasks, and track progress through a Kanban-style workflow.

This directory contains the backend REST API, built with **ASP.NET Core 10**, **Entity Framework Core 10**, and **PostgreSQL**.

The API handles project and task operations, database persistence, request validation, and task status transitions.

## Features

- **Project Management** — Create and retrieve projects.
- **Task Management** — Create tasks and retrieve tasks associated with projects.
- **Task Status Updates** — Update task statuses using a dedicated API endpoint.
- **Transition Validation** — Enforce valid status changes between Kanban columns.
- **Database Persistence** — Store project and task data in PostgreSQL.
- **Request Validation** — Validate incoming data using request DTOs and data annotations.
- **Centralized Error Handling** — Handle API exceptions through custom middleware.
- **Swagger Documentation** — Explore and test API endpoints during development.
- **Frontend Integration** — Support requests from the React development server through CORS.

## Tech Stack

| Technology | Purpose |
|---|---|
| .NET 10 | Application framework |
| ASP.NET Core Web API | REST API development |
| C# | Backend programming language |
| Entity Framework Core 10 | Database access and ORM |
| PostgreSQL | Relational database |
| Neon | Hosted PostgreSQL provider |
| Npgsql | PostgreSQL provider for Entity Framework Core |
| Swagger / Swashbuckle | Interactive API documentation |
| .NET User Secrets | Local management of sensitive configuration |

## Architecture

The backend separates HTTP request handling, business logic, and data access.

### Main Components

- **Controllers** — Receive HTTP requests and return API responses.
- **Services** — Implement business logic for projects and tasks.
- **DTOs** — Define request and response contracts.
- **Data** — Contains the Entity Framework Core database context.
- **Middleware** — Handles exceptions centrally.

### Dependency Injection

The application registers the following services:

| Service | Lifetime |
|---|---|
| `ProjectService` | Scoped |
| `TaskService` | Scoped |
| `TaskStatusTransitionValidator` | Singleton |
| `TaskFlowDbContext` | Scoped (via `AddDbContext`) |

Entity Framework Core uses the Npgsql provider to connect to PostgreSQL.

## Getting Started

### Prerequisites

Install the following tools before running the backend:

- .NET 10 SDK
- Git
- .NET EF Core CLI
- Access to a PostgreSQL database (local or hosted)
- An IDE such as Visual Studio, JetBrains Rider, or Visual Studio Code

### 1. Clone the Repository

```bash
git clone https://github.com/ElhamDargi/TaskFlow.git
cd TaskFlow/TaskFlowApi
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure PostgreSQL

TaskFlow uses PostgreSQL for database persistence.

The backend reads its connection string from the following configuration key:

```text
ConnectionStrings:DefaultConnection
```

The application supports both local PostgreSQL and hosted PostgreSQL.

#### Option A: Local PostgreSQL

Create a local PostgreSQL database.

Configure the connection string using .NET User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=TaskFlowDb;Username=postgres;Password=YOUR_PASSWORD"
```

Replace `YOUR_PASSWORD` with your PostgreSQL password.

Update the database name, port, or username if necessary.

#### Option B: Hosted PostgreSQL (Neon)

TaskFlow uses [Neon](https://neon.tech/) as its hosted PostgreSQL provider.

**Hosted database configuration:**

| Setting | Value |
|---|---|
| Provider | Neon |
| Database engine | PostgreSQL |
| Region | AWS Europe Central 1 (Frankfurt) |
| Database | `neondb` |
| Database username | `neondb_owner` |
| Connection type | Direct connection |
| .NET configuration key | `ConnectionStrings:DefaultConnection` |

To configure the hosted database:

1. Create a project in the Neon dashboard.
2. Create or select a PostgreSQL database.
3. Open the database connection details.
4. Select the direct connection option.
5. Copy the Npgsql-compatible connection string.
6. Store the connection string securely using .NET User Secrets.

For local development, run:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<YOUR_NEON_CONNECTION_STRING>"
```

Replace `<YOUR_NEON_CONNECTION_STRING>` with the actual connection string obtained from Neon.

The connection string should include the SSL configuration required by Neon.

**Security considerations:**

- Never commit actual connection strings or passwords to Git.
- Never store production database credentials directly in `appsettings.json`.
- Use .NET User Secrets for local development.
- Use environment variables or the deployment platform's secret management system for production.
- Do not expose database credentials in logs or documentation.

**Note:** .NET User Secrets are intended for local development and are not a production secret management solution.

### 4. Apply Database Migrations

TaskFlow uses Entity Framework Core migrations to manage its PostgreSQL database schema.

If the EF Core CLI is not installed, install it:

```bash
dotnet tool install --global dotnet-ef
```

List the existing migrations:

```bash
dotnet ef migrations list
```

Apply migrations to the configured database:

```bash
dotnet ef database update
```

The database specified by `ConnectionStrings:DefaultConnection` must be accessible before applying migrations.

**Existing migrations:**

| Migration | Purpose |
|---|---|
| `20260915103007_InitialCreate` | Initial database migration |
| `20260915114247_TaskProjectRelationship` | Creates the project and task schema and relationship |

The resulting database contains:

- `Projects` — Stores project information.
- `Tasks` — Stores task information and references projects.
- `__EFMigrationsHistory` — Tracks applied Entity Framework Core migrations.

The `Tasks` table includes a foreign key referencing `Projects`.

EF Core uses the migration history table to determine which migrations have already been applied.

### 5. Run the API

Start the backend in the Development environment:

```bash
dotnet run --environment Development
```

The API address depends on the local launch settings.

In the verified local development setup, the backend runs at:

```text
http://localhost:5072
```

Check the terminal output or `Properties/launchSettings.json` to confirm the address and port.

## Hosted Database Verification

The hosted PostgreSQL database was configured and tested successfully using Neon.

### Completed Verification

- Neon PostgreSQL database created successfully.
- Database credentials configured through .NET User Secrets.
- Local ASP.NET Core backend connected successfully to Neon.
- Existing EF Core migrations applied successfully.
- Database tables verified in the Neon dashboard.
- Project creation and retrieval tested successfully.
- Task creation and retrieval tested successfully.

### API Connectivity Tests

The following operations were tested using Swagger:

| Operation | Result |
|---|---|
| Retrieve projects | Successful |
| Create project | Successful |
| Create task | Successful |
| Retrieve stored data | Successful |

These tests confirmed that the locally running backend could communicate with the hosted PostgreSQL database and persist application data.

### Deployment Connectivity

Connectivity from the future deployed backend host has not yet been tested.

This verification is planned as part of the backend deployment issue.

Production database credentials must be configured securely on the deployment platform.

## API Documentation

Swagger UI is enabled when the application runs in the **Development** environment.

In the verified local setup, Swagger can be accessed at:

```text
http://localhost:5072/swagger
```

The API may use a different address or port depending on the launch configuration.

Swagger provides interactive documentation for the registered API endpoints.

## API Overview

The API supports project management, task creation, task retrieval, and task status updates.

### Projects

| Operation | Description |
|---|---|
| Create project | Create a new project |
| Get projects | Retrieve available projects |

### Tasks

| Operation | Description |
|---|---|
| Create task | Create a task associated with a project |
| Get project tasks | Retrieve tasks belonging to a project |
| Update task status | Change a task's workflow status |

The exact routes for project and task creation and retrieval can be found in Swagger UI or the corresponding controllers.

### Update Task Status

```http
PATCH /api/tasks/{taskId}/status
Content-Type: application/json
```

Example request body:

```json
{
  "status": 1
}
```

The endpoint validates the requested status transition before saving the change.

## Task Status Workflow

TaskFlow uses three task statuses:

| Status | Numeric Value | Description |
|---|---|---|
| To Do | `0` | The task has not started |
| In Progress | `1` | Work on the task is underway |
| Done | `2` | The task is completed |

### Allowed Transitions

```text
To Do ↔ In Progress ↔ Done
```

Tasks can move between adjacent statuses in either direction.

Examples of valid transitions:

- To Do → In Progress
- In Progress → To Do
- In Progress → Done
- Done → In Progress

Direct transitions between **To Do** and **Done** are not allowed.

The backend validates transitions through `TaskStatusTransitionValidator`, ensuring that workflow rules are enforced independently of the frontend.

## Validation and Error Handling

The API uses request DTOs and validation attributes to validate incoming data.

For task status updates, the API checks:

- Whether the requested status value is valid.
- Whether the task exists.
- Whether the requested transition follows the workflow rules.

A custom `ExceptionHandlingMiddleware` is registered in the HTTP pipeline to provide centralized exception handling.

## CORS Configuration

The API includes a CORS policy named `Frontend`.

During local development, this policy allows requests from:

```text
http://localhost:5173
```

The policy allows request headers and HTTP methods from this origin.

If the frontend runs at a different origin, update the CORS configuration in `Program.cs`.

## Frontend Integration

The frontend is built with **React, TypeScript, Vite, and Tailwind CSS**.

It communicates with the backend through REST API requests and uses TanStack Query to manage server state.

Task status changes support optimistic UI updates, allowing tasks to move immediately between Kanban columns while the API processes the request.

If an update fails, the frontend restores the affected task's previous status.

For frontend setup and usage instructions, see the [Frontend README](../taskflow-frontend/README.md).

## Useful Commands

| Command | Description |
|---|---|
| `dotnet restore` | Restore NuGet dependencies |
| `dotnet build` | Build the backend project |
| `dotnet run --environment Development` | Run the API in Development |
| `dotnet ef migrations list` | List EF Core migrations |
| `dotnet ef database update` | Apply pending EF Core migrations |
| `dotnet user-secrets list` | List configured development secrets (may expose credentials) |

**Security note:** Avoid sharing the output of `dotnet user-secrets list`, as it can display sensitive values.

## Future Improvements

Potential enhancements include:

- Authentication and authorization
- Task editing and deletion
- Task assignment and filtering
- Pagination
- Automated unit and integration tests
- CI/CD enhancements
- Production backend deployment
- Production database connectivity verification

## License

No license has been specified for this project.