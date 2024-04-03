import { Directive, ElementRef, Input, SimpleChanges } from '@angular/core';

import { BcfViewpoint } from '../../generated/models';

@Directive({
  selector: '[bcfierViewpointImage]',
  standalone: true,
})
export class ViewpointImageDirective {
  @Input() bcfierViewpointImage: BcfViewpoint | null = null;

  constructor(private elementRef: ElementRef) {}

  ngOnInit(): void {
    this.handleImage();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['bcfierViewpointImage']) {
      this.handleImage();
    }
  }

  private handleImage(): void {
    if (!this.bcfierViewpointImage?.snapshotBase64) {
      this.elementRef.nativeElement.style.display = 'none';
      return;
    }

    this.elementRef.nativeElement.style.display = null;

    // We're using the base64 data from the snapshot to
    // generate a base64 data url with png type
    const imageUrl = `data:image/png;base64,${this.bcfierViewpointImage.snapshotBase64}`;
    // Then we're appending it to the host element as src attribute
    this.elementRef.nativeElement.src = imageUrl;
  }
}
