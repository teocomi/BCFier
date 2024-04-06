import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';

import { BcfViewpoint } from '../../../generated/models';
import { Component } from '@angular/core';
import { DropzoneCdkModule } from '@ngx-dropzone/cdk';
import { DropzoneMaterialModule } from '@ngx-dropzone/material';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { getNewRandomGuid } from '../../functions/uuid';

@Component({
  selector: 'bcfier-add-snapshot-viewpoint',
  standalone: true,
  imports: [
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    DropzoneCdkModule,
    DropzoneMaterialModule,
  ],
  templateUrl: './add-snapshot-viewpoint.component.html',
  styleUrl: './add-snapshot-viewpoint.component.scss',
})
export class AddSnapshotViewpointComponent {
  constructor(public dialogRef: MatDialogRef<AddSnapshotViewpointComponent>) {}

  fileCtrl = new FormControl();

  async save(): Promise<void> {
    if (!this.fileCtrl.value) {
      return;
    }

    const file = this.fileCtrl.value as File;

    // We're reading the file as base64 data
    const reader = new FileReader();
    const base64 = await this.readFileAsBase64(file);

    const viewpoint: BcfViewpoint = {
      id: getNewRandomGuid(),
      clippingPlanes: [],
      lines: [],
      viewpointComponents: {
        coloring: [],
        selectedComponents: [],
        visibility: {
          defaultVisibility: true,
          exceptions: [],
        },
      },
      snapshotBase64: base64,
    };

    this.dialogRef.close(viewpoint);
  }

  close(): void {
    this.dialogRef.close();
  }

  clear(): void {
    this.fileCtrl.setValue(null);
  }

  private readFileAsBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        const dataUrl = reader.result as string;
        // We want everything after the comma
        const base64 = dataUrl.split(',')[1];
        resolve(base64);
      };
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
  }
}
