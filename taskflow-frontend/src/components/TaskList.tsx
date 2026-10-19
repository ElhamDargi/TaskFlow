import { useMutation, useQueryClient } from "@tanstack/react-query";
import { TaskStatus } from "../types/TaskResponse";
import type { TaskResponse } from "../types/TaskResponse";
import TaskColumn from "./TaskColumn";
import { DndContext } from "@dnd-kit/core";
import type { DragEndEvent } from "@dnd-kit/core";
import { updateTaskStatus } from "../api/tasksApi";
import { useState } from "react";

type TaskListProps = {
  tasks: TaskResponse[];
  projectId: string;
};

function TaskList({ tasks, projectId }: TaskListProps) {
  const todoTasks = tasks.filter((task) => task.status === TaskStatus.Todo);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const inProgressTasks = tasks.filter(
    (task) => task.status === TaskStatus.InProgress,
  );

  const doneTasks = tasks.filter((task) => task.status === TaskStatus.Done);

  const queryClient = useQueryClient();

  const updateStatusMutation = useMutation({
    mutationFn: ({ taskId, status }: { taskId: string; status: TaskStatus }) =>
      updateTaskStatus(taskId, status),

    onMutate: async ({ taskId, status }) => {
      const queryKey = ["tasks", projectId];

      await queryClient.cancelQueries({ queryKey });

      const previousTask = queryClient
        .getQueryData<TaskResponse[]>(queryKey)
        ?.find((task) => task.id === taskId);

      queryClient.setQueryData<TaskResponse[]>(queryKey, (oldTasks) =>
        oldTasks?.map((task) =>
          task.id === taskId ? { ...task, status } : task,
        ),
      );

      return { previousTask };
    },

    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey: ["tasks"],
      });
    },

    onError: (_error, { taskId }, context) => {
      const previousTask = context?.previousTask;

      if (previousTask) {
        queryClient.setQueryData<TaskResponse[]>(
          ["tasks", projectId],
          (oldTasks) =>
            oldTasks?.map((task) => (task.id === taskId ? previousTask : task)),
        );
      }

      setErrorMessage("Could not move task. The transition may be invalid.");
    },
  });

  function handleDragEnd(event: DragEndEvent) {
    setErrorMessage(null);
    const { active, over } = event;

    if (!over) return;

    const taskId = String(active.id);
    const newStatus = Number(over.id) as TaskStatus;

    const task = tasks.find((task) => task.id === taskId);

    if (!task) return;

    if (task.status === newStatus) return;

    updateStatusMutation.mutate({
      taskId,
      status: newStatus,
    });
  }

  return (
    <>
      {errorMessage && (
        <p
          role="alert"
          className="mb-4 rounded-lg border border-red-200 bg-red-50 p-3 text-sm text-red-700"
        >
          {errorMessage}
        </p>
      )}
      <DndContext onDragEnd={handleDragEnd}>
        <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
          <TaskColumn
            title="To Do"
            status={TaskStatus.Todo}
            tasks={todoTasks}
          />

          <TaskColumn
            title="In Progress"
            status={TaskStatus.InProgress}
            tasks={inProgressTasks}
          />

          <TaskColumn title="Done" status={TaskStatus.Done} tasks={doneTasks} />
        </div>
      </DndContext>
    </>
  );
}

export default TaskList;
