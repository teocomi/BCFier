import { Pipe, PipeTransform } from '@angular/core';

import { BcfComment } from '../../generated/models';

@Pipe({
  name: 'commentsViewpointFilter',
  standalone: true,
})
export class CommentsViewpointFilterPipe implements PipeTransform {
  transform(value: BcfComment[], viewpointId?: string): BcfComment[] {
    const filteredComments = value.filter((comment) =>
      viewpointId ? comment.viewpointId === viewpointId : !comment.viewpointId
    );

    return filteredComments;
  }
}
