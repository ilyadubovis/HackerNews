export interface NewsStory {
  id: string;
  title: string;
  type: string;
  time: number;
  url: string;
  by: string;
  descendants: number;
  kids?: string[];
  score: number;
}
