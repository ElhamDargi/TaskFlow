# TaskFlow — Frontend

TaskFlow is a task and project management application inspired by tools like Trello and Jira. It provides an interactive interface for organizing projects, creating tasks, and tracking their progress through a Kanban-style board.

This frontend is built with **React, TypeScript, Vite, and Tailwind CSS** and communicates with an ASP.NET Core REST API.

## Features

- **Project Management** — View projects and create new projects.
- **Project Details** — View tasks belonging to a specific project.
- **Kanban Board** — Organize tasks into To Do, In Progress, and Done columns.
- **Task Creation** — Create tasks using a modal form with client-side validation.
- **Drag and Drop** — Move tasks between status columns.
- **Optimistic Updates** — Instantly reflect task status changes before the server responds.
- **Error Rollback** — Restore the previous task status when an update fails.
- **API Integration** — Fetch and update data using TanStack Query.
- **Form Validation** — Validate user input using React Hook Form and Zod.

## Tech Stack

| Technology | Purpose |
|---|---|
| React 19 | UI development |
| TypeScript 6 | Type safety |
| Vite 8 | Development server and build tool |
| Tailwind CSS 4 | Styling |
| React Router 7 | Client-side routing |
| TanStack Query 5 | Server state management and caching |
| React Hook Form | Form state management |
| Zod | Schema validation |
| dnd-kit | Drag-and-drop interactions |
| pnpm | Package management |

## Getting Started

### Prerequisites

Make sure you have the following installed:

- Node.js compatible with Vite 8
- pnpm
- TaskFlow backend API running locally

### Installation

Clone the repository:

```bash
git clone https://github.com/ElhamDargi/TaskFlow.git
cd TaskFlow/taskflow-frontend
```

Install dependencies:

```bash
pnpm install
```

### Environment Variables

Create a `.env` file in the `taskflow-frontend` directory:

```env
VITE_API_BASE_URL=https://localhost:44312
```

Update the URL if your backend runs on a different address or port.

### Run the Application

Start the development server:

```bash
pnpm dev
```

The application will typically be available at:

```text
http://localhost:5173
```

Make sure the backend API is running before using features that require server data.

## Available Scripts

| Command | Description |
|---|---|
| `pnpm dev` | Start the Vite development server |
| `pnpm build` | Run TypeScript checks and create a production build |
| `pnpm preview` | Preview the production build locally |
| `pnpm lint` | Run ESLint |

## Application Routes

| Route | Description |
|---|---|
| `/projects` | View and create projects |
| `/projects/:projectId` | View project tasks and the Kanban board |

## Task Status Workflow

TaskFlow supports three task statuses:

1. **To Do** — Tasks waiting to be started.
2. **In Progress** — Tasks currently being worked on.
3. **Done** — Completed tasks.

Tasks can move between adjacent columns in either direction:

```text
To Do ↔ In Progress ↔ Done
```

Direct transitions between To Do and Done are not allowed.

When a task is dropped into another column, the interface updates immediately. If the API rejects the change, the task is returned to its previous status and an error message is displayed.

## Backend Integration

The frontend communicates with the TaskFlow ASP.NET Core backend through REST API endpoints.

The backend handles project and task persistence, status transition validation, and database operations.

For backend setup instructions, see the [Backend README](../TaskFlowApi/README.md).

## Future Improvements

Potential enhancements include:

- User authentication and authorization
- Task editing and deletion
- Task assignment and filtering
- Task ordering within columns
- Automated testing
- Deployment and CI/CD

## License

No license has been specified for this project.