import { Component, Input } from '@angular/core';

import { BcfFile } from '../../../generated/models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { TopicFilterPipe } from '../../pipes/topic-filter.pipe';
import { TopicPreviewImageDirective } from '../../directives/topic-preview-image.directive';

@Component({
  selector: 'bcfier-bcf-file',
  standalone: true,
  imports: [
    MatButtonModule,
    MatInputModule,
    CommonModule,
    MatCardModule,
    MatIconModule,
    TopicPreviewImageDirective,
    FormsModule,
    TopicFilterPipe,
  ],
  templateUrl: './bcf-file.component.html',
  styleUrl: './bcf-file.component.scss',
})
export class BcfFileComponent {
  @Input() bcfFile!: BcfFile;

  private _search = '';
  public set search(value: string) {
    this._search = value;
  }
  public get search(): string {
    return this._search;
  }
}
