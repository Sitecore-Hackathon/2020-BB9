import { Component } from '@angular/core';
import { SciLogoutService } from '@speak/ng-sc/logout';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.scss']
})
export class LandingPageComponent {

  isNavigationShown = false;

  constructor(
    public logoutService: SciLogoutService
  ) { }

}
