import {
  BcfFile,
  BcfProjectExtensions,
  BcfTopic,
} from '../../../generated/models';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { AddSnapshotViewpointComponent } from '../add-snapshot-viewpoint/add-snapshot-viewpoint.component';
import { AddStringValueComponent } from '../add-string-value/add-string-value.component';
import { AppConfigService } from '../../services/AppConfigService';
import { CommentsDetailComponent } from '../comments-detail/comments-detail.component';
import { CommentsViewpointFilterPipe } from '../../pipes/comments-viewpoint-filter.pipe';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'bcfier-topic-detail',
  standalone: true,
  imports: [
    FormsModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    CommonModule,
    MatSelectModule,
    MatDialogModule,
    AddStringValueComponent,
    CommentsViewpointFilterPipe,
    CommentsDetailComponent,
  ],
  templateUrl: './topic-detail.component.html',
  styleUrl: './topic-detail.component.scss',
})
export class TopicDetailComponent implements OnInit {
  @Input() topic!: BcfTopic;
  @Input() bcfFile!: BcfFile;

  extensions!: BcfProjectExtensions;

  constructor(
    private matDialog: MatDialog,
    private appConfigService: AppConfigService
  ) {}

  ngOnInit(): void {
    this.extensions = this.bcfFile?.projectExtensions || {
      priorities: [],
      snippetTypes: [],
      topicLabels: [],
      topicStatuses: ['Open', 'Closed', 'InProgress', 'ReOpened'],
      topicTypes: [
        'Info',
        'Issue',
        'Error',
        'Comment',
        'Request',
        'Structural',
      ],
      users: [],
    };
  }

  addNewStatus(): void {
    this.matDialog
      .open(AddStringValueComponent, {
        data: {
          header: 'Status',
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.extensions.topicStatuses.push(result);
          this.topic.topicStatus = result;
        }
      });
  }

  addNewType(): void {
    this.matDialog
      .open(AddStringValueComponent, {
        data: {
          header: 'Topic Type',
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.extensions.topicTypes.push(result);
          this.topic.topicType = result;
        }
      });
  }

  addViewpoint(): void {
    if (this.appConfigService.getFrontendConfig().isInElectronMode) {
      this.matDialog
        .open(AddSnapshotViewpointComponent)
        .afterClosed()
        .subscribe((viewpoint) => {
          if (viewpoint) {
            this.topic.viewpoints.push(viewpoint);
          }
        });
    } else {
      throw new Error('Not implemented');
    }
  }
}
