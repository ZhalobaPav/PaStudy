import { Component, OnInit } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { RouterOutlet } from '@angular/router';
import { User } from '../../../shared/models/user';
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [PageHeaderComponent, RouterOutlet],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent implements OnInit {
  logout() {
    throw new Error('Method not implemented.');
  }
  user!: User;
  ngOnInit(): void {}
}
