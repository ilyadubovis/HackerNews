import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, DestroyRef, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, combineLatest, forkJoin, mergeMap, of, switchMap } from 'rxjs';
import { FormsModule } from "@angular/forms";
import { NewsStoryComponent } from '../news-story/news-story.component';
import { NewsService } from '../../services/news.service';
import { NewsStory } from '../../models/news-story';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-news-list',
  standalone: true,
  imports: [CommonModule, NewsStoryComponent, FormsModule],
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NewsListComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private newsService: NewsService,
    private destroyRef: DestroyRef
  ) { }

  category = signal(environment.defaultCategory);
  pageIndex = signal(1);
  newsStoryIds = signal<string[]>([]);
  newsList = signal<NewsStory[]>([]);
  pageCount = signal(0);

  readonly trackByNewsItemId = (_: number, item: NewsStory): string => item.id;

  ngOnInit(): void {

    combineLatest([this.route.paramMap, this.route.queryParamMap])
      .pipe(
        switchMap(([paramMap, queryParamMap]) => {
          const category = paramMap.get('category') || environment.defaultCategory;
          this.category.set(category);

          this.pageIndex.set(Number(queryParamMap.get('page')) || 1);

          return this.newsService.getNewsPageCountByCategory(category).pipe(
            switchMap
              (count => {
              this.pageCount.set(count);

              return this.newsService.getNewsIdsByCategory(category, this.pageIndex()).pipe(
                mergeMap(ids => {
                  this.newsStoryIds.set(ids);
                  return forkJoin(
                    ids//.slice((this.pageIndex() - 1) * this.pageSize + 1, this.pageIndex() * this.pageSize + 1)
                      .map(id => this.newsService.getNewsStoryById(id))
                  );
                }),
                catchError(() => of([] as NewsStory[]))
              );
              }),
            catchError(() => of([] as NewsStory[]))
          );
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: items => {
          this.newsList.set(items);
        },
        error: () => {
          this.newsList.set([]);
        }
      });
  }

  nextPage = (): Promise<boolean> =>
    this.navigateTo(this.category(), Math.min(this.pageIndex() + 1, this.pageCount()));


  prevPage = (): Promise<boolean> =>
    this.navigateTo(this.category(), Math.max(this.pageIndex() - 1, 1));


  navigateTo = (category: string, pageIndex: number): Promise<boolean> =>
    this.router.navigate(['/news', category], {
      queryParams: { page: pageIndex }
    });

}
