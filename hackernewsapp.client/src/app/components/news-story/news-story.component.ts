import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DistanceTimePipe } from '../pipes/distance-time.pipe';
import { NewsStory } from '../../models/news-story';

@Component({
  selector: 'app-news-list-item',
  standalone: true,
  imports: [CommonModule, DistanceTimePipe],
  templateUrl: './news-story.component.html',
  styleUrls: ['./news-story.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NewsStoryComponent {
  @Input({ required: true }) story!: NewsStory;

  openLink(): void {
    if (this.story.url) {
      window.open(this.story.url, '_blank');
    }
    else {
      alert (`Cannot display the news story ${this.story.id}: URL is undefined.`)
    }
  }
}
