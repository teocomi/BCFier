import { BcfFile, BcfTopic } from '../../../generated/models';
import { Component, Input } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { TopicDetailComponent } from '../topic-detail/topic-detail.component';
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
    MatProgressBarModule,
    TopicDetailComponent,
  ],
  templateUrl: './bcf-file.component.html',
  styleUrl: './bcf-file.component.scss',
})
export class BcfFileComponent {
  @Input() bcfFile!: BcfFile;

  selectedTopic: BcfTopic | null = null;

  ngOnInit() {
    this.selectedTopic = this.bcfFile.topics[0] || null;
  }

  private _search = '';
  public set search(value: string) {
    this._search = value;
  }
  public get search(): string {
    return this._search;
  }

  selectTopic(topic: BcfTopic) {
    this.selectedTopic = topic;
  }

  addIssue(): void {
    const newIssue: BcfTopic = {
      comments: [],
      id: crypto.randomUUID(),
      files: [],
      labels: [],
      referenceLinks: [],
      documentReferences: [],
      relatedTopicIds: [],
      viewpoints: [],
      assignedTo: '',
      creationAuthor: '',
      description: '',
      priority: '',
      title: 'New Issue',
      topicStatus: '',
      stage: '',
      topicType: '',
      serverAssignedId: '',
      modifiedAuthor: '',
      creationDate: new Date(),
    };
    this.bcfFile.topics.push(newIssue);
    this.selectedTopic = newIssue;
  }

  removeIssue(): void {
    if (!this.selectedTopic) {
      return;
    }

    this.bcfFile.topics = this.bcfFile.topics.filter(
      (topic) => topic.id !== this.selectedTopic?.id
    );

    if (this.bcfFile.topics.length > 0) {
      this.selectedTopic = this.bcfFile.topics[0];
    } else {
      this.selectedTopic = null;
    }
  }
}
