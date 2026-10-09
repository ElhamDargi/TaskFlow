
import { TaskStatus } from "../types/TaskResponse";
import type { TaskResponse } from "../types/TaskResponse";
import TaskColumn from "./TaskColumn";

type TaskListProps = {
  tasks: TaskResponse[];
};

function TaskList({ tasks }: TaskListProps) {
  const todoTasks = tasks.filter(
    (task) => task.status === TaskStatus.Todo
  );

  const inProgressTasks = tasks.filter(
    (task) => task.status === TaskStatus.InProgress
  );

  const doneTasks = tasks.filter(
    (task) => task.status === TaskStatus.Done
  );

  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
      <TaskColumn title="To Do" tasks={todoTasks} />
      <TaskColumn title="In Progress" tasks={inProgressTasks} />
      <TaskColumn title="Done" tasks={doneTasks} />
    </div>
  );
}

export default TaskList;
