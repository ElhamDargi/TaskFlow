import { apiClient } from "./apiClient";
import type { TaskResponse, TaskStatus } from "../types/TaskResponse";
import type { CreateTaskRequest } from "../types/CreateTaskRequest";

export async function fetchTasksByProject(
  projectId: string,
): Promise<TaskResponse[]> {
  const response = await apiClient(`/api/tasks/project/${projectId}`);

  if (!response.ok) {
    throw new Error("Failed to load tasks");
  }

  return response.json();
  }

  export async function createTask(
    request: CreateTaskRequest,
  ): Promise<TaskResponse> {
    const response = await apiClient("/api/tasks", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error("Failed to create task");
    }

    return response.json();
  }

  export async function updateTaskStatus(
  taskId: string,
  status: TaskStatus
): Promise<TaskResponse> {
  const response = await apiClient(`/api/tasks/${taskId}/status`, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ status }),
  });

  if (!response.ok) {
    throw new Error("Failed to update task status");
  }

  return response.json();
}
