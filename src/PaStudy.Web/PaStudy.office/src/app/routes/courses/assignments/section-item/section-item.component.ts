import { Component, input } from '@angular/core';
import { Section } from '../models/section';

@Component({
  selector: 'app-section-item',
  templateUrl: './section-item.component.html',
  styleUrl: './section-item.component.scss',
  standalone: false,
})
export class SectionItemComponent {
  section = input.required<Section>();
  isEditMode = input.required<boolean>();
}
