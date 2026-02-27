import {
  Component,
  forwardRef,
  inject,
  input,
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
import { take } from 'rxjs';

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

      this.assignmentService
        .uploadFile(file)
        .pipe(take(1))
        .subscribe((url) => {
          const range = this.quillInstance.getSelection(true);

          this.quillInstance.insertEmbed(range.index, 'image', url);
        });
    };
  }
  private insertImage(url: string) {
    const editor = this.quillComponent.quillEditor;
    const range = editor.getSelection(true);

    editor.insertEmbed(range.index, 'image', url);
  }
}
