import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface TaskRequest {
  boardId: string;
  title: string;
  description: string;
  priority: string;
  status: string;
  dueDate?: string;
  assigneeIds: string[];
}

@Injectable({ providedIn: 'root' })
export class TaskService {
  constructor(private readonly http: HttpClient) {}

  createTask(request: TaskRequest) {
    return this.http.post(`${environment.apiUrl}/api/tasks`, request);
  }

  getTask(id: string) {
    return this.http.get(`${environment.apiUrl}/api/tasks/${id}`);
  }
}
