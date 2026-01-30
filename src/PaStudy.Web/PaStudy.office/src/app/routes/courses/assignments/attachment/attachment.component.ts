import { Component, input } from '@angular/core';
import { Attachment } from '../models/attachment';

@Component({
  selector: 'app-attachment',
  standalone: true,
  imports: [],
  templateUrl: './attachment.component.html',
  styleUrl: './attachment.component.scss',
})
export class AttachmentComponent {
  public attachment = input.required<Attachment>();
}
