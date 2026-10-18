export type CreateTaskRequest = {
  title: string;
  description: string | null;
  priority: number;
  dueDate: string | null;
  projectId: string;
};