import { Routes } from '@angular/router';
import { LayoutComponent } from './routes/profile/layout/layout.component';
import { authCheckGuard } from './routes/auth/shared/guards/auth-check.guard';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authCheckGuard],
    children: [
      {
        path: '',
        redirectTo: 'profile',
        pathMatch: 'full',
      },
      {
        path: 'profile',
        loadChildren: () =>
          import('./routes/profile/profile.routes').then((m) => m.routes),
      },
      {
        path: 'users',
        loadChildren: () =>
          import('./routes/users/user.module').then((mod) => mod.UserModule),
      },
      {
        path: 'courses',
        loadChildren: () =>
          import('./routes/courses/course.module').then(
            (mod) => mod.CoursesModule,
          ),
      },
    ],
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./routes/auth/auth.module').then((mod) => mod.AuthModule),
    data: { breadcrumb: { skip: true } },
  },
];
