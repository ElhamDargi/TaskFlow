import { z } from "zod";
import { TaskPriority } from "../types/TaskResponse";

export const createTaskSchema = z.object({
  title: z
    .string()
    .trim()
    .min(1, "Task title is required")
    .max(100, "Title must be 100 characters or less"),

  description: z
    .string()
    .max(500, "Description must be 500 characters or less"),

  priority: z.union([
    z.literal(TaskPriority.Low),
    z.literal(TaskPriority.Medium),
    z.literal(TaskPriority.High),
  ]),

  dueDate: z.string().optional(),
});

export type CreateTaskFormValues = z.infer<typeof createTaskSchema>;
