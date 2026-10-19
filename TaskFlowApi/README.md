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
- PostgreSQL
- Git
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

Create a PostgreSQL database for the application.

The backend reads its connection string from the configuration key:

```text
ConnectionStrings:DefaultConnection
```

For local development, configure the connection string using .NET User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=TaskFlowDb;Username=postgres;Password=YOUR_PASSWORD"
```

Replace `YOUR_PASSWORD` with your PostgreSQL password.

If your database uses a different name, port, or username, update those values accordingly.

**Security:** Never commit real database credentials, passwords, or production secrets to Git.

### 4. Apply Database Migrations

If the database has not been initialized, apply the existing Entity Framework Core migrations:

```bash
dotnet ef database update
```

If the EF Core CLI is not installed, install it first:

```bash
dotnet tool install --global dotnet-ef
```

The database specified in your connection string must be accessible before applying migrations.

### 5. Run the API

Start the backend in the Development environment:

```bash
dotnet run --environment Development
```

The API address depends on your local launch settings.

In the current development setup, the frontend uses:

```text
https://localhost:44312
```

Check the terminal output or `Properties/launchSettings.json` to confirm the address used when running the API.

## API Documentation

Swagger UI is enabled when the application runs in the **Development** environment.

Once the API is running, open:

```text
https://localhost:44312/swagger
```

Replace the host and port if your API is running at a different address.

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
| `dotnet ef database update` | Apply EF Core migrations |
| `dotnet user-secrets list` | List configured development secrets |

## Future Improvements

Potential enhancements include:

- Authentication and authorization
- Task editing and deletion
- Task assignment and filtering
- Pagination
- Automated unit and integration tests
- CI/CD pipelines
- Production deployment

## License

No license has been specified for this project.