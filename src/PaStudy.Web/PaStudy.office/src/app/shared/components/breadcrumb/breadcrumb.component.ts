import { Component, inject, Input, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter, startWith } from 'rxjs';
import { MenuService } from '../../../core/bootstrap/menu.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
})
export class BreadcrumbComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly menuService = inject(MenuService);
  @Input() nav: string[] = [];

  navItems: string[] = [];

  public trackByNavItem(index: number, item: string): string {
    return item;
  }
  ngOnInit(): void {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        startWith(this.router)
      )
      .subscribe(() => {
        this.genBreadcrumb();
      });
  }

  public genBreadcrumb() {
    const routes = this.router.url.slice(1).split('/');
    if(this.nav.length > 0) {
      this.navItems = [...this.nav];
    } else {
      this.navItems = this.menuService.getLevel(routes);
      this.navItems.unshift('home');
    }
  }
}
