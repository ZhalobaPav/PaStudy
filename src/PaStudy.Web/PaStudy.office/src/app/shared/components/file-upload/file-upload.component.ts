import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss'],
})
export class FileUploadComponent {
  @Output() fileSelected = new EventEmitter<File>();

  selectedFile: File | null = null;
  errorMessage: string | null = null;
  isDragOver = false;

  // Дозволені формати: PDF та Word (.doc, .docx)
  readonly allowedTypes = [
    'application/pdf',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
  ];
  readonly maxFileSize = 10 * 1024 * 1024; // 10 MB

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.validateAndSelectFile(input.files[0]);
    }
  }

  // Логіка Drag & Drop
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      this.validateAndSelectFile(event.dataTransfer.files[0]);
    }
  }

  private validateAndSelectFile(file: File): void {
    this.errorMessage = null;

    // 1. Валідація типу файлу
    if (!this.allowedTypes.includes(file.type)) {
      this.errorMessage =
        'Дозволено завантажувати лише файли формату PDF або Word (.doc, .docx)';
      return;
    }

    // 2. Валідація розміру
    if (file.size > this.maxFileSize) {
      this.errorMessage = 'Розмір файлу не повинен перевищувати 10 МБ';
      return;
    }

    this.selectedFile = file;
    this.fileSelected.emit(file);
  }

  removeFile(event: Event): void {
    event.stopPropagation(); // щоб не спрацьовував клік по контейнеру
    this.selectedFile = null;
    this.errorMessage = null;
  }

  getFileIcon(): string {
    if (!this.selectedFile) return '';
    return this.selectedFile.type === 'application/pdf' ? '📄 PDF' : '📝 WORD';
  }
}
