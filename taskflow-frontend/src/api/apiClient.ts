const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function apiClient(
  endpoint: string,
  options?: RequestInit
) {
  const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
  return response;
}