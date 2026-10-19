import type { TaskResponse } from "../types/TaskResponse";
import DraggableTask from "./DraggableTask";
import { useDroppable } from "@dnd-kit/core";
import type { TaskStatus } from "../types/TaskResponse";

type TaskColumnProps = {
  title: string;
  tasks: TaskResponse[];
  status: TaskStatus;
};

function TaskColumn({ title, tasks, status }: TaskColumnProps) {
  const { setNodeRef, isOver } = useDroppable({
    id: status,
  });
  return (
    <section
      ref={setNodeRef}
      className={`rounded-xl p-4 ${isOver ? "bg-blue-100" : "bg-gray-100"}`}
    >
      <h2 className="mb-4 text-lg font-semibold text-gray-900">
        {title} ({tasks.length})
      </h2>

      {tasks.length === 0 ? (
        <p className="text-sm text-gray-500">No tasks in this column.</p>
      ) : (
        <ul className="space-y-3">
          {tasks.map((task) => (
            <DraggableTask key={task.id} task={task} />
          ))}
        </ul>
      )}
    </section>
  );
}

export default TaskColumn;
