import {
  Component,
  inject,
  Input,
  OnInit,
  booleanAttribute,
  ViewEncapsulation,
  signal,
  computed,
} from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { MenuService } from '../../../core/bootstrap/menu.service';
import { AuthService } from '../../../routes/auth/auth.service';
import { NotificationService } from '../../services/notification.service';
import { MessagingService } from '../../services/messaging.service';
import { take, tap } from 'rxjs';
import { Notification } from '../../models/notification';
import { Location } from '@angular/common';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [BreadcrumbComponent, RouterLink],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss',
  host: {
    class: 'page-header',
  },
  encapsulation: ViewEncapsulation.None,
})
export class PageHeaderComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly menuService = inject(MenuService);
  private readonly authService = inject(AuthService);
  private notificationsService = inject(MessagingService);
  private route = inject(ActivatedRoute);
  private location = inject(Location);
  public notifications = signal<Notification[]>([]);
  public readonly isAuthenticated = this.authService.isAuthorized;
  @Input() title: string = '';
  @Input() subtitle: string = '';
  @Input() nav: string[] = [];
  @Input({ transform: booleanAttribute }) hideBreadcrumb = false;
  ngOnInit(): void {
    const routes = this.router.url.slice(1).split('/');
    const menuLevel = this.menuService.getLevel(routes);

    this.title = this.title || menuLevel[menuLevel.length - 1];
    this.fetchNotifications();
  }
  logout() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
  public toggleDropdown() {
    this.isDropdownOpen.update((v) => !v);
  }
  public readNotif(notif: Notification) {
    if (!notif.isRead) {
    }
    this.isDropdownOpen.set(false);

    if (notif.clickActionUrl) {
      this.router.navigateByUrl(notif.clickActionUrl);
    }
  }
  public isDropdownOpen = signal(false);

  public unreadCount = computed(
    () => this.notifications().filter((n) => !n.isRead).length,
  );
  public goBackToCourse(): void {
    this.location.back();
  }
  private fetchNotifications() {
    this.notificationsService
      .getNotifications()
      .pipe(
        tap((notifications) => {
          this.notifications.set(notifications);
        }),
        take(1),
      )
      .subscribe();
  }
}
