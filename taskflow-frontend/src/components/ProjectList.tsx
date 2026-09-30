import { useEffect, useState } from "react";
import type { Project } from "../types/Project";
import { apiClient } from "../api/apiClient";

function ProjectList() {
  const [loading, setLoading] = useState(true);
  const [projects, setProjects] = useState<Project[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const response = await apiClient("/api/projects");
        if (!response.ok) {
          throw new Error("Failed to fetch projects");
        }
        const data: Project[] = await response.json();
        setProjects(data);
      } catch (error) {
        setError((error as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchProjects();
  }, []);
  return (
    <div>
      <h1>Project List</h1>
      {loading && <p>Loading...</p>}
      {error && <p>Error: {error}</p>}
      {!loading && !error && (
        <ul>
          {projects.length === 0 ? (
            <li>No projects found.</li>
          ) : (
            projects.map((project) => (
              <li key={project.id}>
                <h2>{project.name}</h2>
                <p>{project.description}</p>
                <p>
                  Created At: {new Date(project.createdAt).toLocaleString()}
                </p>
              </li>
            ))
          )}
        </ul>
      )}
    </div>
  );
}

export default ProjectList;
