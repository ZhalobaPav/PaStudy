import { Component, inject, OnInit, signal } from '@angular/core';
import { ICourse } from '../../../shared/models/course';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from '../course.service';
import { take, tap } from 'rxjs';

@Component({
  selector: 'app-course-details',
  standalone: true,
  imports: [],
  templateUrl: './course-details.component.html',
  styleUrl: './course-details.component.scss',
})
export class CourseDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  course = signal<ICourse | null>(null);
  ngOnInit(): void {
    const courseIdStr = this.route.snapshot.paramMap.get('id');
    if (!courseIdStr) {
      throw new Error('Course does not exist');
    }
    const courseId = +courseIdStr;
    this.courseService
      .getCourse(courseId)
      .pipe(
        take(1),
        tap((response) => {
          if (!response) {
            return;
          }
          this.course.set(response);
        })
      )
      .subscribe();
  }
}
