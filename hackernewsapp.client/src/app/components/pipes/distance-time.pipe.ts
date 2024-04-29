import { Pipe, PipeTransform } from '@angular/core';
import { formatDistanceToNow } from 'date-fns';

@Pipe({
  name: 'distanceTime',
  standalone: true
})
export class DistanceTimePipe implements PipeTransform {
  transform(timespan: number): string {
    const date = new Date(1000 * timespan);
    return `${formatDistanceToNow(date)} ago`;
  }
}
