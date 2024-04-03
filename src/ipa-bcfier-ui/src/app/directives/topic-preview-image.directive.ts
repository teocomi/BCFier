import { BcfFile, BcfTopic } from '../../generated/models';
import {
  Directive,
  ElementRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';

@Directive({
  selector: '[bcfierTopicPreviewImage]',
  standalone: true,
})
export class TopicPreviewImageDirective implements OnInit, OnChanges {
  @Input() bcfierTopicPreviewImage!: BcfTopic;

  constructor(private elementRef: ElementRef) {}

  ngOnInit(): void {
    this.handleImage();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['bcfierTopicPreviewImage']) {
      this.handleImage();
    }
  }

  private handleImage(): void {
    const viewpoints = this.bcfierTopicPreviewImage.viewpoints.filter(
      (vp) => vp.snapshotBase64
    );

    if (viewpoints.length === 0) {
      this.elementRef.nativeElement.style.display = 'none';
      return;
    }

    this.elementRef.nativeElement.style.display = null;

    // We're using the base64 data from the snapshot to
    // generate a base64 data url with png type
    const imageUrl = `data:image/png;base64,${viewpoints[0].snapshotBase64}`;
    // Then we're appending it to the host element as src attribute
    this.elementRef.nativeElement.src = imageUrl;
  }
}
