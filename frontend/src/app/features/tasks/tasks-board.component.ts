import { Component, signal } from '@angular/core';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { NgFor } from '@angular/common';

interface BoardColumn {
  name: string;
  tasks: TaskCard[];
}

interface TaskCard {
  id: string;
  title: string;
  assignees: string[];
  priority: 'Low' | 'Medium' | 'High' | 'Critical';
  dueDate?: Date;
}

@Component({
  selector: 'tm-tasks-board',
  standalone: true,
  imports: [DragDropModule, NgFor],
  template: `
    <section class="flex gap-4 overflow-x-auto pb-6">
      <div *ngFor="let column of board()" class="w-80 flex-shrink-0">
        <div class="column">
          <header class="flex items-center justify-between mb-3">
            <h3 class="font-semibold">{{ column.name }}</h3>
            <span class="text-xs text-foreground/60">{{ column.tasks.length }}</span>
          </header>
          <div
            cdkDropList
            [cdkDropListData]="column.tasks"
            class="space-y-3"
            (cdkDropListDropped)="onDrop($event, column.name)"
          >
            <article
              *ngFor="let task of column.tasks"
              cdkDrag
              class="task-card"
              [class.priority-critical]="task.priority === 'Critical'"
            >
              <h4 class="font-medium mb-1">{{ task.title }}</h4>
              <div class="text-xs text-foreground/60 mb-2">Due {{ task.dueDate | date }}</div>
              <div class="flex -space-x-2">
                <div *ngFor="let assignee of task.assignees" class="avatar">{{ assignee }}</div>
              </div>
            </article>
          </div>
        </div>
      </div>
    </section>
  `,
  styles: [
    `
      .column {
        @apply bg-layer-1 border border-border rounded-xl p-4 shadow-sm h-full;
      }
      .task-card {
        @apply bg-layer-2 border border-border rounded-lg p-3 shadow hover:shadow-md transition;
      }
      .task-card.priority-critical {
        @apply border-rose-400 shadow-rose-200/50;
      }
      .avatar {
        @apply w-8 h-8 rounded-full bg-layer-3 text-foreground flex items-center justify-center text-xs font-semibold border border-border;
      }
    `,
  ],
})
export class TasksBoardComponent {
  readonly board = signal<BoardColumn[]>([
    {
      name: 'To Do',
      tasks: this.generateTasks('T', 5),
    },
    {
      name: 'In Progress',
      tasks: this.generateTasks('IP', 4),
    },
    {
      name: 'Review',
      tasks: this.generateTasks('R', 3),
    },
    {
      name: 'Done',
      tasks: this.generateTasks('D', 6),
    },
  ]);

  onDrop(event: CdkDragDrop<TaskCard[]>, status: string) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);
    }

    console.log('Task moved to', status);
  }

  private generateTasks(prefix: string, count: number): TaskCard[] {
    return Array.from({ length: count }).map((_, index) => ({
      id: `${prefix}-${index}`,
      title: `Collaborative Task ${index + 1}`,
      assignees: ['AJ', 'MS', 'TD'].slice(0, (index % 3) + 1),
      priority: ['Low', 'Medium', 'High', 'Critical'][index % 4] as TaskCard['priority'],
      dueDate: new Date(Date.now() + index * 86400000),
    }));
  }
}
