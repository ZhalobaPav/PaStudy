import {
  Component,
  inject,
  Input,
  OnInit,
  booleanAttribute,
  ViewEncapsulation,
} from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { MenuService } from '../../../core/bootstrap/menu.service';

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

  @Input() title: string = '';
  @Input() subtitle: string = '';
  @Input() nav: string[] = [];
  @Input({ transform: booleanAttribute }) hideBreadcrumb = false;
  ngOnInit(): void {
    const routes = this.router.url.slice(1).split('/');
    const menuLevel = this.menuService.getLevel(routes);

    this.title = this.title || menuLevel[menuLevel.length - 1];
  }
}
