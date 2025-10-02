import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly isDark = signal(true);

  toggle() {
    this.isDark.update((value) => !value);
    document.documentElement.classList.toggle('dark', this.isDark());
  }

  current() {
    return this.isDark.asReadonly();
  }
}
