import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api/apiClient";
import { createProjectSchema } from "../schemas/createProjectSchema";
import type { CreateProjectRequest } from "../types/CreateProjectRequest";

async function createProject(project: CreateProjectRequest) {
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

export default function CreateProjectForm() {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<CreateProjectRequest>({
    resolver: zodResolver(createProjectSchema),
    defaultValues: {
      name: "",
      description: "",
    },
  });

  const mutation = useMutation({
    mutationFn: createProject,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projects"] });
      reset();
    },
  });

  const onSubmit = (data: CreateProjectRequest) => {
    mutation.mutate(data);
  };

  return (
    <div className="bg-gray-50 px-6 pt-10">
      <div className="mx-auto max-w-5xl">
        <form
          onSubmit={handleSubmit(onSubmit)}
          className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
        >
          <h2 className="mb-6 text-xl font-semibold text-gray-900">
            Create Project
          </h2>

          <div className="mb-4">
            <label
              htmlFor="project-name"
              className="mb-2 block text-sm font-medium text-gray-700"
            >
              Project name
            </label>
            <input
              id="project-name"
              type="text"
              {...register("name")}
              aria-invalid={Boolean(errors.name)}
              aria-describedby={errors.name ? "name-error" : undefined}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 outline-none focus:border-blue-500"
              placeholder="Enter project name"
            />
            {errors.name && (
              <p id="name-error" className="mt-2 text-sm text-red-600">
                {errors.name.message}
              </p>
            )}
          </div>

          <div className="mb-6">
            <label
              htmlFor="project-description"
              className="mb-2 block text-sm font-medium text-gray-700"
            >
              Description
            </label>
            <textarea
              id="project-description"
              rows={3}
              {...register("description")}
              aria-invalid={Boolean(errors.description)}
              aria-describedby={errors.description ? "description-error" : undefined}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 outline-none focus:border-blue-500"
              placeholder="Enter project description (optional)"
            />
            {errors.description && (
              <p id="description-error" className="mt-2 text-sm text-red-600">
                {errors.description.message}
              </p>
            )}
          </div>

          <button
            type="submit"
            disabled={mutation.isPending}
            className="rounded-lg bg-blue-600 px-5 py-2.5 font-medium text-white hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {mutation.isPending ? "Creating..." : "Create Project"}
          </button>

          {mutation.isSuccess && (
            <p className="mt-4 text-sm text-green-600">
              Project created successfully!
            </p>
          )}

          {mutation.isError && (
            <p className="mt-4 text-sm text-red-600">
              {mutation.error.message}
            </p>
          )}
        </form>
      </div>
    </div>
  );
}