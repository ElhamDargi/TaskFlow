export const TaskStatus = {
  Todo: 0,
  InProgress: 1,
  Done: 2,
} as const;

export type TaskStatus =
  (typeof TaskStatus)[keyof typeof TaskStatus];

export const TaskPriority = {
  Low: 0,
  Medium: 1,
  High: 2,
} as const;

export type TaskPriority =
  (typeof TaskPriority)[keyof typeof TaskPriority];

export type TaskResponse = {
  id: string;
  title: string;
  description: string | null;
  status: TaskStatus;
  priority: TaskPriority;
  createdAt: string;
  dueDate: string | null;
  projectId: string;
};