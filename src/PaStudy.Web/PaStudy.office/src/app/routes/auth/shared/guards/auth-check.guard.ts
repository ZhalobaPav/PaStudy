import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
  UrlSegment,
} from '@angular/router';
import { AuthService } from '../../auth.service';

export const authCheckGuard = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const routePath = route.url
    .map((segment: UrlSegment) => segment.path)
    .join('/');
  if (
    !authService.isAuthorized &&
    state.url !== '/login' &&
    routePath !== 'login'
  ) {
    router.navigate(['login']);
    return false;
  }
  return true;
};
