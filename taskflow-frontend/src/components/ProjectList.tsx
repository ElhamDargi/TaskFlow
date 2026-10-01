import type { Project } from "../types/Project";
import { apiClient } from "../api/apiClient";
import { useQuery } from "@tanstack/react-query";

async function fetchProjects(): Promise<Project[]> {
  const response = await apiClient("/api/projects");

  if (!response.ok) {
    throw new Error("Failed to fetch projects");
  }

  return response.json();
}

function formatDate(dateString: string): string {
  const date = new Date(dateString);

  return isNaN(date.getTime())
    ? "Invalid date"
    : date.toLocaleString();
}

function ProjectList() {
  const {
    data: projects = [],
    isLoading,
    error,
  } = useQuery({
    queryKey: ["projects"],
    queryFn: fetchProjects,
  });

  return (
    <div className="min-h-screen bg-gray-50 px-6 py-10">
      <div className="mx-auto max-w-5xl">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">Projects</h1>

          <p className="mt-2 text-gray-500">View and manage your projects.</p>
        </div>
        {isLoading && (
          <div className="rounded-xl border border-gray-200 bg-white p-6 text-gray-500">
            Loading projects...
          </div>
        )}
        {error && (
          <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-700">
            <p className="font-medium">Failed to load projects</p>
            <p className="mt-1 text-sm">{error.message}</p>
          </div>
        )}
        {!isLoading && !error && (
          <>
            {projects.length === 0 ? (
              <div className="rounded-xl border border-dashed border-gray-300 bg-white p-10 text-center">
                <h2 className="text-lg font-semibold text-gray-900">
                  No projects found
                </h2>

                <p className="mt-2 text-sm text-gray-500">
                  There are no projects to display yet.
                </p>
              </div>
            ) : (
              <ul className="space-y-4">
                {projects.map((project) => (
                  <li
                    key={project.id}
                    className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
                  >
                    <h2 className="text-xl font-semibold text-gray-900">
                      {project.name}
                    </h2>

                    <p className="mt-2 text-gray-600">
                      {project.description ?? "No description"}
                    </p>

                    <p className="mt-4 text-sm text-gray-400">
                      Created At: {formatDate(project.createdAt)}
                    </p>
                  </li>
                ))}
              </ul>
            )}
          </>
        )}
      </div>
    </div>
  );
}

export default ProjectList;
