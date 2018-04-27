import { BrowserModule } from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';
import { AppComponent } from './app.component';
import {HomeModule} from "./modules/home/home.module";
import {RouterModule, Routes} from "@angular/router";
import {HomePage} from "./modules/home/pages/home/home.page";
import {NavbarComponent} from "./shared/components/navbar/navbar.component";
import {SideNavigationComponent} from "./shared/components/sidenavigation/sidenavigation.component";
import {SideNavigationDirective} from "./shared/directives/sideNavigation.directive";
import {AccountModule} from "./modules/account/account.module";
import {ShowAuthedDirective} from "./shared/directives/showAuthed.directive";
import {AppConfigService} from "./shared/services/app-config.service";
import {HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {SignInLoadingComponent} from "./shared/components/signin-loading/signin-loading.component";
import {CommonModule} from "@angular/common";

const appRoutes: Routes = [
  {   path: "", component: HomePage, pathMatch: "full"}
];

/**
 * Exported function so that it works with AOT
 * @param {AppConfigService} configService
 * @returns {Function}
 */
export function loadConfigService(configService: AppConfigService): Function
{
  return () => { return configService.load() };
}

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SideNavigationComponent,
    SideNavigationDirective,
    ShowAuthedDirective
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    HomeModule,
    AccountModule,
    RouterModule.forRoot(appRoutes)
  ],
  exports: [RouterModule],
  providers: [
    AppConfigService,
    { provide: APP_INITIALIZER, useFactory: loadConfigService , deps: [AppConfigService], multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
