import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss'],
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    MatSelectModule,
    MatIconModule,
    MatFormFieldModule,
    MatButtonModule
  ]
})
export class MainPageComponent {
  selectedFormat: string | null = null;
  selectedProcessor: string | null = null;
  file: File | null = null;
  fileName: string = '';
  outputFileName: string = '';

  constructor(private http: HttpClient) {}

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
      this.fileName = this.file.name;
    } else {
      this.file = null;
      this.fileName = '';
    }
  }

  onSubmit() {
    if (this.file && this.selectedFormat && this.selectedProcessor && this.outputFileName) {
      const params = new URLSearchParams();
      params.append('outputFileName', this.outputFileName);
      params.append('fileType', this.selectedFormat);
      params.append('processType', this.selectedProcessor);
  
      const url = `http://localhost:5144/file/process?${params.toString()}`;
  
      const formData = new FormData();
      formData.append('file', this.file);
  
      this.http.post(url, formData, { responseType: 'blob' })
        .subscribe(response => {
          this.downloadFile(response, `output.${this.getFileExtension()}`);
        }, error => {
          console.error('Ошибка при обработке файла:', error);
        });
    }
  }

  private getFileExtension(): string {
    switch (this.selectedFormat) {
      case '1': return 'txt';
      case '2': return 'json';
      case '3': return 'xml';
      case '4': return 'yaml';
      default: return 'txt';
    }
  }

  private downloadFile(data: Blob, fileName: string) {
    const url = window.URL.createObjectURL(data);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
  }
}