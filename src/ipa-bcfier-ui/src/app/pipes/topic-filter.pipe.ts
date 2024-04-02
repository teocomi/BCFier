import { Pipe, PipeTransform } from '@angular/core';

import { BcfTopic } from '../../generated/models';

@Pipe({
  name: 'topicFilter',
  standalone: true,
})
export class TopicFilterPipe implements PipeTransform {
  transform(value: BcfTopic[], filter: string): BcfTopic[] {
    if (!filter) {
      return value;
    }

    return value.filter((topic) => {
      if (
        topic.title != null &&
        topic.title.toUpperCase().indexOf(filter.toUpperCase()) !== -1
      ) {
        return true;
      }

      if (
        topic.description != null &&
        topic.description.toUpperCase().indexOf(filter.toUpperCase()) !== -1
      ) {
        return true;
      }

      return false;
    });
  }
}
