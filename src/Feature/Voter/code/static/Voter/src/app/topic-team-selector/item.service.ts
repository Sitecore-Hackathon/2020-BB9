import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Team } from './team';


@Injectable()
export class ItemService {

  constructor(private http: HttpClient) { }

  getTeams() {
    return this.http.get(`/api/hackathon/voter/voter/getteams`);
  }

  getTopics() {
    return this.http.get(`/api/hackathon/voter/voter/gettopics`);
  }

  save(teams : Team[]) {
    return this.http.post(`/api/hackathon/voter/voter/save`, teams);
  }
}
