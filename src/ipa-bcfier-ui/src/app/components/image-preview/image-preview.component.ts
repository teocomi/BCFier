import { Component, Inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { BcfViewpoint } from '../../../generated/models';
import { ViewpointImageDirective } from '../../directives/viewpoint-image.directive';

@Component({
  selector: 'bcfier-image-preview',
  standalone: true,
  imports: [MatDialogModule, ViewpointImageDirective],
  templateUrl: './image-preview.component.html',
  styleUrl: './image-preview.component.scss',
})
export class ImagePreviewComponent {
  constructor(
    matDialogRef: MatDialogRef<ImagePreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public viewpoint: BcfViewpoint
  ) {}
}
