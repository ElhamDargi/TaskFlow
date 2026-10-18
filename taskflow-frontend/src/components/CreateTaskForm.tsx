import { useForm } from "react-hook-form";
import { TaskPriority } from "../types/TaskResponse";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createTask } from "../api/tasksApi";
import {
  createTaskSchema,
  type CreateTaskFormValues,
} from "../schemas/createTaskSchema";

type CreateTaskFormProps = {
  projectId: string;
  onClose: () => void;
};

function CreateTaskForm({ projectId, onClose }: CreateTaskFormProps) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateTaskFormValues>({
    resolver: zodResolver(createTaskSchema),
    defaultValues: {
      title: "",
      description: "",
      priority: TaskPriority.Low,
      dueDate: "",
    },
  });

  const createTaskMutation = useMutation({
    mutationFn: createTask,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["tasks", projectId],
      });
      onClose();
    },
  });

  function onSubmit(data: CreateTaskFormValues) {
    createTaskMutation.mutate({
      title: data.title,
      description: data.description.trim() || null,
      priority: data.priority,
      dueDate: data.dueDate ? new Date(data.dueDate).toISOString() : null,
      projectId: projectId,
    });
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <div>
        <label
          htmlFor="task-title"
          className="mb-1 block text-sm font-medium text-gray-700"
        >
          Title
        </label>

        <input
          id="task-title"
          {...register("title")}
          className="w-full rounded-lg border border-gray-300 px-3 py-2"
          placeholder="Enter task title"
        />

        {errors.title && (
          <p className="mt-1 text-sm text-red-600">{errors.title.message}</p>
        )}
      </div>

      <div>
        <label
          htmlFor="task-description"
          className="mb-1 block text-sm font-medium text-gray-700"
        >
          Description
        </label>

        <textarea
          id="task-description"
          {...register("description")}
          rows={3}
          className="w-full rounded-lg border border-gray-300 px-3 py-2"
          placeholder="Enter task description"
        />

        {errors.description && (
          <p className="mt-1 text-sm text-red-600">
            {errors.description.message}
          </p>
        )}
      </div>

      <div>
        <label
          htmlFor="task-priority"
          className="mb-1 block text-sm font-medium text-gray-700"
        >
          Priority
        </label>

        <select
          id="task-priority"
          {...register("priority", { valueAsNumber: true })}
          className="w-full rounded-lg border border-gray-300 px-3 py-2"
        >
          <option value={TaskPriority.Low}>Low</option>
          <option value={TaskPriority.Medium}>Medium</option>
          <option value={TaskPriority.High}>High</option>
        </select>

        {errors.priority && (
          <p className="mt-1 text-sm text-red-600">{errors.priority.message}</p>
        )}
      </div>

      <div>
        <label
          htmlFor="task-due-date"
          className="mb-1 block text-sm font-medium text-gray-700"
        >
          Due Date
        </label>

        <input
          id="task-due-date"
          type="date"
          {...register("dueDate")}
          className="w-full rounded-lg border border-gray-300 px-3 py-2"
        />

        {errors.dueDate && (
          <p className="mt-1 text-sm text-red-600">{errors.dueDate.message}</p>
        )}
      </div>

      {createTaskMutation.isError && (
        <div
          role="alert"
          className="rounded-lg border border-red-200 bg-red-50 p-3 text-sm text-red-700"
        >
          <p className="font-medium">Failed to create task</p>

          <p className="mt-1">{createTaskMutation.error.message}</p>
        </div>
      )}

      <button
        type="submit"
        disabled={createTaskMutation.isPending}
        className="w-full rounded-lg bg-blue-600 px-4 py-2 font-medium text-white hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
      >
        {createTaskMutation.isPending ? "Creating..." : "Create Task"}
      </button>
    </form>
  );
}

export default CreateTaskForm;
