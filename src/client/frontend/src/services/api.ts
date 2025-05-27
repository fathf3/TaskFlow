const API_BASE_URL = 'https://localhost:7057/api'; // Adjust this to your API URL

// Types based on your .NET entities
export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  createdAt: string;
  isActive: boolean;
}

export interface Project {
  id: number;
  name: string;
  description: string;
  startDate: string;
  endDate?: string;
  status: string;
  createdByUserId: number;
  createdAt: string;
}

export interface TaskItem {
  id: number;
  title: string;
  description: string;
  status: string;
  priority: string;
  dueDate?: string;
  createdAt: string;
  completedAt?: string;
  projectId: number;
  createdByUserId: number;
  assignedToUserId?: number;
}

export interface ProjectMember {
  id: number;
  projectId: number;
  userId: number;
  role: string;
  joinedAt: string;
}

class ApiService {
  private async request<T>(endpoint: string, options?: RequestInit): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    const token = localStorage.getItem('token');
    const response = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options?.headers,
      },
      ...options,
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
  }

  // User endpoints
  async getUsers(): Promise<User[]> {
    return this.request<User[]>('/users');
  }

  async createUser(userData: Omit<User, 'id' | 'createdAt'>): Promise<User> {
    return this.request<User>('/users', {
      method: 'POST',
      body: JSON.stringify(userData),
    });
  }

  // Project endpoints
  async getProjects(): Promise<Project[]> {
    return this.request<Project[]>('/projects');
  }

  async createProject(projectData: Omit<Project, 'id' | 'createdAt'>): Promise<Project> {
    return this.request<Project>('/projects', {
      method: 'POST',
      body: JSON.stringify(projectData),
    });
  }

  async getProject(id: number): Promise<Project> {
    return this.request<Project>(`/projects/${id}`);
  }

  // Task endpoints
  async getTasks(projectId?: number): Promise<TaskItem[]> {
    const endpoint = projectId ? `/tasks?projectId=${projectId}` : '/tasks';
    return this.request<TaskItem[]>(endpoint);
  }

  async createTask(taskData: Omit<TaskItem, 'id' | 'createdAt' | 'completedAt'>): Promise<TaskItem> {
    return this.request<TaskItem>('/tasks', {
      method: 'POST',
      body: JSON.stringify(taskData),
    });
  }

  async updateTask(id: number, taskData: Partial<TaskItem>): Promise<TaskItem> {
    return this.request<TaskItem>(`/tasks/${id}`, {
      method: 'PUT',
      body: JSON.stringify(taskData),
    });
  }

  // Project Member endpoints
  async getProjectMembers(projectId: number): Promise<ProjectMember[]> {
    return this.request<ProjectMember[]>(`/projects/${projectId}/members`);
  }

  async addProjectMember(memberData: Omit<ProjectMember, 'id' | 'joinedAt'>): Promise<ProjectMember> {
    return this.request<ProjectMember>('/project-members', {
      method: 'POST',
      body: JSON.stringify(memberData),
    });
  }

  // Auth endpoints
  async login(credentials: { email: string; password: string }) {
    return this.request<{ token: string; user: User }>('/Auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
  }

  async register(data: { firstName: string; lastName: string; email: string; password: string }) {
    return this.request('/Auth/register', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }
}

export const apiService = new ApiService();
