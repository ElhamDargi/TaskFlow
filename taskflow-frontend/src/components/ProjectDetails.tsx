import { useParams } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { fetchTasksByProject } from "../api/tasksApi";
import TaskList from "./TaskList";

function ProjectDetails() {
  const { projectId } = useParams<{ projectId: string }>();

  const {
    data: tasks = [],
    isPending,
    isError,
    error,
  } = useQuery({
    queryKey: ["tasks", projectId],
    queryFn: () => fetchTasksByProject(projectId!),
    enabled: !!projectId,
  });

  return (
    <div className="min-h-screen bg-gray-50 px-6 py-10">
      <div className="mx-auto max-w-6xl">
        <h1 className="mb-8 text-3xl font-bold text-gray-900">
          Project Details
        </h1>

        {!projectId ? (
          <p className="text-red-600">Project ID is missing.</p>
        ) : isPending ? (
          <div className="rounded-xl border border-gray-200 bg-white p-6 text-gray-500">
            Loading tasks...
          </div>
        ) : isError ? (
          <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-700">
            <p className="font-medium">Failed to load tasks</p>
            <p className="mt-1 text-sm">{error.message}</p>
          </div>
        ) : tasks.length === 0 ? (
          <div className="rounded-xl border border-dashed border-gray-300 bg-white p-10 text-center">
            <h2 className="text-lg font-semibold text-gray-900">
              No tasks found
            </h2>
            <p className="mt-2 text-sm text-gray-500">
              There are no tasks to display yet.
            </p>
          </div>
        ) : (
          <TaskList tasks={tasks} />
        )}
      </div>
    </div>
  );
}

export default ProjectDetails;