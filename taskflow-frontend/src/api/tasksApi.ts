import { apiClient } from "./apiClient";
import type { TaskResponse } from "../types/TaskResponse";

export async function fetchTasksByProject(
  projectId: string,
): Promise<TaskResponse[]> {
  const response = await apiClient(`/api/tasks/project/${projectId}`);

  if (!response.ok) {
    throw new Error("Failed to load tasks");
  }

  return response.json();
}
