import { BcfComment, BcfTopic, BcfViewpoint } from '../../../generated/models';
import { Component, Input, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ImagePreviewComponent } from '../image-preview/image-preview.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { NotificationsService } from '../../services/notifications.service';
import { SettingsMessengerService } from '../../services/settings-messenger.service';
import { ViewpointImageDirective } from '../../directives/viewpoint-image.directive';
import { take } from 'rxjs';

@Component({
  selector: 'bcfier-comments-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    ViewpointImageDirective,
  ],
  templateUrl: './comments-detail.component.html',
  styleUrl: './comments-detail.component.scss',
})
export class CommentsDetailComponent implements OnInit {
  @Input() comments!: BcfComment[];
  @Input() viewpoint: BcfViewpoint | null = null;
  @Input() topic!: BcfTopic;

  newComment = '';

  constructor(
    private settingsMessengerService: SettingsMessengerService,
    private notificationsService: NotificationsService,
    private matDialog: MatDialog
  ) {}

  ngOnInit(): void {}

  addComment(): void {
    if (!this.newComment) {
      return;
    }

    this.settingsMessengerService.settings
      .pipe(take(1))
      .subscribe((settings) => {
        const newComment = {
          id: crypto.randomUUID(),
          author: settings.username,
          creationDate: new Date(),
          viewpointId: this.viewpoint?.id,
          text: this.newComment,
        };
        this.comments.push(newComment);
        this.topic.comments.push(newComment);

        this.newComment = '';

        this.notificationsService.success('Comment added');
      });
  }

  removeComment(comment: BcfComment): void {
    this.comments = this.comments.filter((c) => c.id !== comment.id);
    this.topic.comments = this.topic.comments.filter(
      (c) => c.id !== comment.id
    );

    this.notificationsService.success('Comment removed');
  }

  deleteViewpoint(viewpoint: BcfViewpoint): void {
    this.topic.viewpoints = this.topic.viewpoints.filter(
      (v) => v.id !== viewpoint.id
    );

    this.topic.comments.forEach((c) => {
      if (c.viewpointId === viewpoint.id) {
        c.viewpointId = undefined;
      }
    });

    // When we're deleting a viewpoint, we also want to ensure that
    // all other places where the comments are used are reevaluated,
    // since comments that originally belonged to the viewpoint
    // are moved to general comments now
    this.topic.comments = [...this.topic.comments];
  }

  showImageFullScreen(viewpoint: BcfViewpoint): void {
    this.matDialog.open(ImagePreviewComponent, {
      data: viewpoint,
    });
  }
}
