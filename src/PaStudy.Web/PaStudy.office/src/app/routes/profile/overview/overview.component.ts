import {
  Component,
  computed,
  inject,
  OnInit,
  Signal,
  signal,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { OverviewService } from './overview.service';
import { ProfileInfo } from '../models/profile.model';
import { finalize, switchMap, take, tap } from 'rxjs';
import { LoaderService } from '../../../shared/services/loader.service';
import { UserRole } from '../../../shared/enums/userRole';
import { getRoleName } from '../../../shared/functions/common.functions';
import { CourseRoutingModule } from '../../courses/course-routing.module';
import { RouterLink } from '@angular/router';
import { ModalService } from '../../../shared/components/modals/modal.service';
import { CreateCourseModalComponent } from '../../../shared/components/modals/create-course-modal/create-course-modal.component';
import { Category } from '../../../shared/models/category';
import { CreateCourseResponseDto } from '../../../shared/models/course';
import { register } from 'swiper/element/bundle';
register();
@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [CourseRoutingModule, RouterLink],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class OverviewComponent implements OnInit {
  private overviewService = inject(OverviewService);
  public profileInfo = signal<ProfileInfo | null>(null);
  private loaderService = inject(LoaderService);
  private modalService = inject(ModalService);
  public UserRole = UserRole;
  ngOnInit(): void {
    this.fetchProfileInfo();
  }
  public currentUserRole = signal<string | null>('');
  public isLoading: Signal<boolean> = computed(() => {
    return this.loaderService.isLoading();
  });
  public defineRoleName(role: UserRole | undefined): void {
    this.currentUserRole.set(getRoleName(role));
  }

  public isTeacher = computed(
    () => this.profileInfo()?.userRole === UserRole.Teacher,
  );
  private fetchProfileInfo() {
    this.loaderService.busy();
    this.overviewService
      .getProfileInfo()
      .pipe(
        tap((response) => {
          this.profileInfo.set(response);
          this.defineRoleName(this.profileInfo()?.userRole);
        }),
        take(1),
        finalize(() => {
          this.loaderService.idle();
        }),
      )
      .subscribe();
  }

  public openCreateCourseModal() {
    this.overviewService
      .getInfoForCreateCourse()
      .pipe(
        switchMap((categories: Category[]) => {
          const modalRef = this.modalService.open(CreateCourseModalComponent, {
            courseInfo: { categories },
          });

          return modalRef.closed;
        }),
        tap((course: CreateCourseResponseDto) => {
          if (course) {
            this.fetchProfileInfo();
          }
        }),
        take(1),
      )
      .subscribe();
  }
  public title = signal<string>('');

  public hasManyCourses = computed(() => {
    const courses = this.profileInfo()?.courses;
    return courses ? courses.length > 3 : false;
  });
}
