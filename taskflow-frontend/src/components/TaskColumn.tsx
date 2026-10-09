import type { TaskResponse } from "../types/TaskResponse";

type TaskColumnProps = {
  title: string;
  tasks: TaskResponse[];
};

function TaskColumn({ title, tasks }: TaskColumnProps) {
  return (
    <section className="rounded-xl bg-gray-100 p-4">
      <h2 className="mb-4 text-lg font-semibold text-gray-900">
        {title} ({tasks.length})
      </h2>

      {tasks.length === 0 ? (
        <p className="text-sm text-gray-500">
          No tasks in this column.
        </p>
      ) : (
        <ul className="space-y-3">
          {tasks.map((task) => (
            <li
              key={task.id}
              className="rounded-lg border border-gray-200 bg-white p-4 shadow-sm"
            >
              <h3 className="font-semibold text-gray-900">
                {task.title}
              </h3>

              <p className="mt-2 text-sm text-gray-600">
                {task.description ?? "No description"}
              </p>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}

export default TaskColumn;
