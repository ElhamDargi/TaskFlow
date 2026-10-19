import { useDraggable } from "@dnd-kit/core";
import type { TaskResponse } from "../types/TaskResponse";

type DraggableTaskProps = {
  task: TaskResponse;
};

function DraggableTask({ task }: DraggableTaskProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    isDragging,
  } = useDraggable({
    id: task.id,
  });

  const style = transform
    ? {
        transform: `translate3d(${transform.x}px, ${transform.y}px, 0)`,
      }
    : undefined;

  return (
    <li
      ref={setNodeRef}
      style={style}
      {...listeners}
      {...attributes}
      className={`cursor-grab rounded-lg border border-gray-200 bg-white p-4 shadow-sm ${
        isDragging ? "opacity-50" : ""
      }`}
    >
      <h3 className="font-semibold text-gray-900">
        {task.title}
      </h3>

      <p className="mt-2 text-sm text-gray-600">
        {task.description ?? "No description"}
      </p>
    </li>
  );
}

export default DraggableTask;