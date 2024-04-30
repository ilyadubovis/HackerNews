import { NewsService } from './news.service';
import { NewsStory } from '../models/news-story';
import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'
import { MockNewsIds, MockNewsStories } from "../../assets/mock-data/mock-news";
import { environment } from '../../environments/environment';

describe('NewsService', () => {
    let service: NewsService;
    let testingController: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule, RouterModule.forRoot([])],
            providers: [NewsService]
        });
        service = TestBed.inject(NewsService);
        testingController = TestBed.inject(HttpTestingController)

    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should return expected news ids', () => {
        service.getNewsIdsByCategory('topstories', 1).subscribe((ids: string[]) => {
            expect(ids).toBeTruthy();
            expect(ids.length).toBe(2);
            expect(ids).toEqual(['123', '456']);
        });
      console.log(MockNewsIds)
        const mockReq = testingController.expectOne(`${environment.apiEndpoint}topstories`);
        mockReq.flush(MockNewsIds);

    });

    it('should return expected news stories', () => {
        service.getNewsStoryById('123').subscribe((story: NewsStory) => {
            expect(story).toBeTruthy();
            expect(story).toEqual({
                id: '123',
                title: 'news story 123',
                type: 'type 1',
                time: Number(Date.now),
                url: 'http://www.abc.com',
                by: 'John Doe',
                descendants: 0,
                score: 123
            });
        });
        const mockReq = testingController.expectOne(`${environment.apiEndpoint}story/123`);
        const story = MockNewsStories.find((x: { id: string; }) => x.id === '123') as NewsStory;
        mockReq.flush(story);
    });

    it('should return no news story for invalid id', () => {
        service.getNewsStoryById('999').subscribe((story: NewsStory) => {
            expect(story).toBeNull();
        });
        const mockReq = testingController.expectOne(`${environment.apiEndpoint}story/999`);
        const story = MockNewsStories.find(x => x.id === '999');
        mockReq.flush(story || null);
    });
});


