import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ICourse } from '../../../shared/models/course';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../course.service';
import { take, tap } from 'rxjs';
import {
  CourseHeaderTitles,
  HeaderConfig,
  headerConfig,
} from '../config/headers-config';

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
  private courseService = inject(CourseService);
  public readonly tab = CourseHeaderTitles;
  readonly headerConfig: HeaderConfig[] = headerConfig;
  public isEditMode = false;
  public isCreatingAssignment = computed(() =>
    this.router.url.includes('assignment/create'),
  );

  ngOnInit(): void {
    const courseId = this.route.snapshot.paramMap.get('id');
    if (!courseId) {
      throw new Error('Course does not exist');
    }
    this.courseService
      .getCourse(+courseId)
      .pipe(
        take(1),
        tap((response) => {
          if (!response) {
            return;
          }
          this.course.set(response);
        }),
      )
      .subscribe();
  }

  setTab(title: CourseHeaderTitles) {
    this.activeTab.set(title);
  }
}
