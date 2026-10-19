import type { CreateProjectRequest } from "../types/CreateProjectRequest";
import type { Project } from "../types/Project";
import { apiClient } from "./apiClient";

export async  function fetchProjects(): Promise<Project[]> {
  const response = await apiClient("/api/projects");

  if (!response.ok) {
    throw new Error("Failed to fetch projects");
  }

  return response.json();
}

export async  function createProject(project: CreateProjectRequest) {
  let response: Response;

  try {
    response = await apiClient("/api/projects", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(project),
    });
  } catch {
    throw new Error("Unable to connect to the server. Please try again.");
  }

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    throw new Error(errorData?.message || "Failed to create project");
  }

  return response.json();
}