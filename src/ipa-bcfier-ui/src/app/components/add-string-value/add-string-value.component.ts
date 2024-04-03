import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'bcfier-add-string-value',
  standalone: true,
  imports: [MatInputModule, FormsModule, MatDialogModule, MatButtonModule],
  templateUrl: './add-string-value.component.html',
  styleUrl: './add-string-value.component.scss',
})
export class AddStringValueComponent {
  value = '';

  constructor(
    public dialogRef: MatDialogRef<AddStringValueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { header: string }
  ) {}

  close(shouldSave: boolean): void {
    if (shouldSave) {
      this.dialogRef.close(this.value);
    } else {
      this.dialogRef.close();
    }
  }
}
