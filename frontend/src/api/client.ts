export class ApiError extends Error {
  readonly status?: number;
  readonly code?: string;

  constructor(status: number, message: string, code?: string) {
    super(message);
    this.status = status;
    this.code = code;
  }
}

interface ProblemDetails {
  title?: string;
  detail?: string;
  code?: string;
}

export async function apiRequest<T>(
  path: string,
  init?: RequestInit,
): Promise<T> {
  const response = await fetch(`/api${path}`, {
    ...init,
    headers: { "Content-Type": "application/json", ...init?.headers },
  });

  if (!response.ok) {
    const problem: ProblemDetails = await response.json().catch(() => ({}));
    throw new ApiError(
      response.status,
      problem.detail ??
        problem.title ??
        `Request failed with status ${response.status}`,
      problem.code,
    );
  }

  return response.json();
}
