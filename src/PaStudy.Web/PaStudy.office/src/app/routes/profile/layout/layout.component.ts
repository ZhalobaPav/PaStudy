import { Component, OnInit } from '@angular/core';
import { PageHeaderComponent } from "../../../shared/components/page-header/page-header.component";
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { User } from '../../../shared/models/user';
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [PageHeaderComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit{
logout() {
throw new Error('Method not implemented.');
}
  user!: User;
  ngOnInit(): void {
    
  }
}
