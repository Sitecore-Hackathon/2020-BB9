import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ScAccountInformationModule } from '@speak/ng-bcl/account-information';
import { ScActionBarModule } from '@speak/ng-bcl/action-bar';
import { ScApplicationHeaderModule } from '@speak/ng-bcl/application-header';
import { ScButtonModule } from '@speak/ng-bcl/button';
import { ScGlobalHeaderModule } from '@speak/ng-bcl/global-header';
import { ScGlobalLogoModule } from '@speak/ng-bcl/global-logo';
import { ScIconModule } from '@speak/ng-bcl/icon';
import { ScPageModule } from '@speak/ng-bcl/page';
import { ScDropdownModule } from '@speak/ng-bcl/dropdown';

import { CONTEXT, DICTIONARY } from '@speak/ng-bcl';

import { NgScModule } from '@speak/ng-sc';
import { SciAntiCSRFModule } from '@speak/ng-sc/anti-csrf';

import { AppComponent } from './app.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { TaskPageComponent } from './topic-team-selector/topic-team-selector.component';
import { ItemService } from './topic-team-selector/item.service';

@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    TaskPageComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: LandingPageComponent, pathMatch: 'full' },
      { path: 'topicteamselector', component: TaskPageComponent }
    ]),
    ScAccountInformationModule,
    ScActionBarModule,
    ScApplicationHeaderModule,
    ScButtonModule,
    ScGlobalHeaderModule,
    ScGlobalLogoModule,
    ScIconModule,
    ScPageModule,
    ScDropdownModule,
    SciAntiCSRFModule,
    NgScModule.forRoot({
      // The ItemId refers to '/sitecore/client/Applications/ScIntegrationRefApp/UserAccess' AccessFolder item
      authItemId: '1023A91F-E7C0-410C-BE84-472204C71FD7',
      contextToken: CONTEXT,
      dictionaryToken: DICTIONARY,
      // The ItemId refers to '/sitecore/client/Applications/ScIntegrationRefApp/Translations' Speak3DictionaryFolder item
      translateItemId: 'B76C8EC2-1139-4BB1-915D-0F0DB4A04FE4'
    })
  ],
  providers: [
    ItemService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
