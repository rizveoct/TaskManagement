import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const roleGuard = (roles: string[]): CanActivateFn => () => {
  const router = inject(Router);
  const storedRoles = JSON.parse(localStorage.getItem('roles') ?? '[]') as string[];

  if (!roles.some((role) => storedRoles.includes(role))) {
    router.navigate(['/dashboard']);
    return false;
  }

  return true;
};
