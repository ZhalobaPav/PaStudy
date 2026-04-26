import {
  Component,
  EventEmitter,
  forwardRef,
  inject,
  input,
  Output,
  signal,
  ViewChild,
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { AssignmentService } from '../../../routes/courses/assignments/assignment.service';
import { PendingImage } from '../../../routes/courses/assignments/models/attachment';
import Quill from 'quill';

@Component({
  selector: 'app-text-editor',
  standalone: true,
  imports: [ReactiveFormsModule, QuillModule, FormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextEditorComponent),
      multi: true,
    },
  ],
  templateUrl: './text-editor.component.html',
  styleUrl: './text-editor.component.scss',
})
export class TextEditorComponent implements ControlValueAccessor {
  @ViewChild('quillEditor') quillComponent: any;
  @Output() imageUploaded = new EventEmitter<PendingImage>();
  private assignmentService = inject(AssignmentService);
  value: string = '';
  placeholder = input<string>('');
  onChange: any = () => {};
  onTouched: any = () => {};
  isDisabled = signal<boolean>(false);

  private quillInstance: any;

  onEditorCreated(editor: any) {
    this.quillInstance = editor;

    const toolbar = editor.getModule('toolbar');

    toolbar.addHandler('image', () => this.imageHandler());
    const Image = Quill.import('formats/image') as any;
    Image.sanitize = (url: string) => url;
  }

  onChanged(event: any) {
    this.value = event.html;
    this.onChange(this.value);
  }

  writeValue(obj: any): void {
    this.value = obj;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }

  private imageHandler() {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/*';
    input.click();

    input.onchange = () => {
      const file = input.files?.[0];
      if (!file) return;

      // Створюємо тимчасовий URL для відображення в редакторі
      const blobUrl = URL.createObjectURL(file);

      const range = this.quillInstance.getSelection(true);

      // Вставляємо саме картинку (Quill створить <img src="blob:...">)
      this.quillInstance.insertEmbed(range.index, 'image', blobUrl, 'user');
      this.quillInstance.setSelection(range.index + 1);

      // Повідомляємо батька, що у нас новий файл і його ключ - це blobUrl
      this.imageUploaded.emit({
        file: file,
        tempUrl: blobUrl,
      });
    };
  }
  private insertImage(url: string) {
    const editor = this.quillComponent.quillEditor;
    const range = editor.getSelection(true);

    editor.insertEmbed(range.index, 'image', url);
  }
}
