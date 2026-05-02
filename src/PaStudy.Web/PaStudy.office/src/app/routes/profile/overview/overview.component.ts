import {
  Component,
  computed,
  inject,
  OnInit,
  Signal,
  signal,
} from '@angular/core';
import { OverviewService } from './overview.service';
import { ProfileInfo } from '../models/profile.model';
import { finalize, take, tap } from 'rxjs';
import { LoaderService } from '../../../shared/services/loader.service';
import { UserRole } from '../../../shared/enums/userRole';
import { getRoleName } from '../../../shared/functions/common.functions';

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss',
})
export class OverviewComponent implements OnInit {
  private overviewService = inject(OverviewService);
  public profileInfo = signal<ProfileInfo | null>(null);
  private loaderService = inject(LoaderService);
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
  public title = signal<string>('');
}
