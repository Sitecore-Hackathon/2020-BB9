import { Component, OnInit } from '@angular/core';
import { ItemService } from './item.service';
import { Team } from './team';
import { Topic } from './topic';

import { SciAuthService } from '@speak/ng-sc/auth';
import { SciLogoutService } from '@speak/ng-sc/logout';

@Component({
  selector: 'app-topic-team-selector',
  templateUrl: './topic-team-selector.component.html'
})
export class TaskPageComponent implements OnInit {
  ngOnInit(): void {
    this.getTeams();
    this.getTopics();
  }

  teams: Team[];
  topics: Topic[];

  isErrorResponse = false;

  constructor(
    public authService: SciAuthService,
    public logoutService: SciLogoutService,
    public itemService: ItemService
  ) { }

  getTeams() {
    this.itemService.getTeams().subscribe({
      next: data => {
        this.teams = data as Team[];
      },
      error: error => {
      }
    });
  }

  getTopics() {
    this.itemService.getTopics().subscribe({
      next: data => {
        this.topics = data as Topic[];
      },
      error: error => {
      }
    });
  }

  editTeam(topicId : string, teamId : string) {
    let team = this.teams.find(y => y.Id == teamId);
    team.TopicId = topicId;
    team.TopicName = this.topics.find(y => y.Id == topicId).Title;
  }

  save() {
    this.itemService.save(this.teams).subscribe({
      next: data => {
      },
      error: error => {
      }
    });
  }
}
