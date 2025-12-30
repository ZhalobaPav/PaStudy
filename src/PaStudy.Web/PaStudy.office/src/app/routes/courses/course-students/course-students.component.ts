import { Component, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../../users/user.service';
import { ActivatedRoute } from '@angular/router';
import { FetchOptions } from '../../../shared/components/table/models/table.models';
import { take, tap } from 'rxjs';
import { FilterUserProfile } from '../../users/enums/filterUserProfile';
import { UserFilter } from '../../users/models/user-filter';
import { User } from '../../../shared/models/user';
import { tableConfig } from '../../users/helpers/user-list-table.config';

@Component({
  selector: 'app-course-students',
  templateUrl: './course-students.component.html',
  styleUrl: './course-students.component.scss',
})
export class CourseStudentsComponent implements OnInit {
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);
  public tableConfig = tableConfig;
  public users = signal<User[]>([]);
  ngOnInit(): void {
    this.fetchUsers();
  }

  public fetchUsers() {
    const courseId = this.route.snapshot.paramMap.get('id');
    if (!courseId) {
      throw new Error('No course id');
    }
    const filters: UserFilter = {
      filterUserProfile: FilterUserProfile.OnlyStudents,
      courseId: +courseId,
    };
    const options: FetchOptions<UserFilter> = {
      ...filters,
      filters: undefined,
    };
    this.userService
      .fetchUsers(options)
      .pipe(
        take(1),
        tap((response) => {
          this.users.set(response);
        })
      )
      .subscribe();
  }
}
