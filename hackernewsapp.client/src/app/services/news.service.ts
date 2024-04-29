import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { NewsStory } from '../models/news-story';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  constructor(private http: HttpClient) { }

  getNewsPageCountByCategory = (category: string): Observable<number> =>
    this.http.get<number>(`${environment.apiEndpoint}${category}/pagecount`);

  getNewsIdsByCategory = (category: string, pageIndex: Number): Observable<string[]> => 
    this.http.get<string[]>(`${environment.apiEndpoint}${category}?page=${pageIndex}`);
  
  getNewsStoryById = (id: string): Observable<NewsStory> =>
    this.http.get<NewsStory>(`${environment.apiEndpoint}story/${id}`);
}
