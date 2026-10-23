# TaskFlow

TaskFlow is a full-stack task and project management application.

## Tech Stack

- **Frontend:** React, TypeScript, Vite
- **Backend:** ASP.NET Core 10, Entity Framework Core
- **Database:** PostgreSQL (Neon)
- **Frontend Hosting:** Vercel
- **Backend Hosting:** Azure App Service
- **CI/CD:** GitHub Actions and Vercel Git Integration

## Live Application

- **Frontend:** https://task-flow-sigma-blush.vercel.app/projects
- **Backend API:** https://taskflow-api-elham-c4djeqb7d3c2erfg.westus3-01.azurewebsites.net/api/projects

## CI/CD Workflow

1. Create a feature or fix branch.
2. Open a Pull Request targeting `main`.
3. GitHub Actions runs frontend lint and build checks, plus the backend build check.
4. Required checks must pass before merging.
5. After merging into `main`, GitHub Actions builds and deploys the backend to Azure.
6. Vercel independently builds and deploys the frontend through its GitHub integration.
7. Deployment results and logs can be reviewed in GitHub Actions and the Vercel dashboard.

Deployment credentials are managed through GitHub Secrets and hosting-provider configuration rather than being committed to the repository.