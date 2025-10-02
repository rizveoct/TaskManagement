import { Component } from '@angular/core';
import { ShellComponent } from './shared/components/shell.component';

@Component({
  selector: 'tm-root',
  standalone: true,
  imports: [ShellComponent],
  template: `
    <div class="min-h-screen bg-surface text-foreground">
      <tm-shell></tm-shell>
    </div>
  `,
})
export class AppComponent {}
