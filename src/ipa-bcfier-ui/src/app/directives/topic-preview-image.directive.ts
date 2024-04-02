import { BcfFile, BcfTopic } from '../../generated/models';
import { Directive, ElementRef, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[bcfierTopicPreviewImage]',
  standalone: true,
})
export class TopicPreviewImageDirective implements OnInit {
  @Input() bcfierTopicPreviewImage!: BcfTopic;

  constructor(private elementRef: ElementRef) {}

  ngOnInit(): void {
    const viewpoints = this.bcfierTopicPreviewImage.viewpoints.filter(
      (vp) => vp.snapshotBase64
    );
    if (viewpoints.length === 0) {
      return;
    }

    // We're using the base64 data from the snapshot to
    // generate a base64 data url with png type
    const imageUrl = `data:image/png;base64,${viewpoints[0].snapshotBase64}`;
    // Then we're appending it to the host element as src attribute
    this.elementRef.nativeElement.src = imageUrl;
  }
}
