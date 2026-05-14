import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ICourse } from '../../../shared/models/course';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../course.service';
import { finalize, of, switchMap, take, tap } from 'rxjs';
import {
  CourseHeaderTitles,
  HeaderConfig,
  headerConfig,
  RoleGuardEnum,
} from '../config/headers-config';
import { AuthService } from '../../auth/auth.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { LoaderService } from '../../../shared/services/loader.service';

@Component({
  selector: 'app-course-details',
  templateUrl: './course-details.component.html',
  styleUrl: './course-details.component.scss',
  standalone: false,
})
export class CourseDetailsComponent implements OnInit {
  course = signal<ICourse | null>(null);
  activeTab = signal<CourseHeaderTitles>(CourseHeaderTitles.Course);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private courseService = inject(CourseService);
  private toasterService = inject(NotificationService);
  private loaderService = inject(LoaderService);
  public UserRoleGuard = RoleGuardEnum;
  public readonly tab = CourseHeaderTitles;
  readonly headerConfig: HeaderConfig[] = headerConfig;
  public isEditMode = false;
  public isTeacher = this.authService.isTeacher();
  public isStudent = this.authService.isStudent();
  public isCreatingAssignment = computed(() =>
    this.router.url.includes('assignment/create'),
  );

  ngOnInit(): void {
    const courseId = this.route.snapshot.paramMap.get('id');
    if (!courseId) {
      throw new Error('Course does not exist');
    }
    this.fetchCourse(+courseId).subscribe();
  }

  fetchCourse(courseId: number) {
    this.loaderService.busy();
    return this.courseService.getCourse(courseId).pipe(
      take(1),
      tap((response) => {
        if (!response) {
          return;
        }
        this.course.set(response);
      }),
      finalize(() => {
        this.loaderService.idle();
      }),
    );
  }

  setTab(title: CourseHeaderTitles) {
    this.activeTab.set(title);
  }

  public isEnrollAvailable = computed(() => {
    return this.authService.isStudent() && !this.course()?.isEnrolled;
  });

  public enrollToCourse() {
    if (!this.course()) {
      return;
    }
    return this.courseService
      .enrollToCourse(this.course()!.id)
      .pipe(
        take(1),
        tap((response) => {
          this.toasterService.success(
            'Ви успішно зараховані на курс',
            'Вітаємо',
          );
        }),
        switchMap((_) => {
          if (!this.course()) {
            return of(null);
          }
          return this.fetchCourse(this.course()!.id);
        }),
      )
      .subscribe();
  }
}
