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
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {CommonModule} from "@angular/common";
import {TokenInterceptor} from "./shared/interceptors/token.interceptor";
import {JwtInterceptor} from "./shared/interceptors/jwt.interceptor";
import {AuthenticatedNavbarComponent} from "./shared/components/authenticated-navbar/authenticated-navbar.component";
import {UnAuthenticatedNavbarComponent } from "./shared/components/unauthenticated-navbar/unauthenticated-navbar.component";
import {Globals} from "./globals";
import {DeckModule} from "./modules/deck/deck.module";
import {UploadModule} from "./modules/upload/upload.module";
import {SharedModule} from "./modules/shared/shared.module";
import {PrivatePolicyModule} from "./modules/privacy-policy/privatepolicy.module";
import {BanlistModule} from "./modules/banlist/banlist.module";
import {ArchetypeModule} from "./modules/archetype/archetype.module";
import {UserModule} from "./modules/user/user.module";
import {ContactModule} from "./modules/contact/contact.module";

const appRoutes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: HomePage }
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
    AuthenticatedNavbarComponent,
    UnAuthenticatedNavbarComponent,
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
    PrivatePolicyModule,
    AccountModule,
    DeckModule,
    UploadModule,
    SharedModule,
    BanlistModule,
    SharedModule,
    ArchetypeModule,
    UserModule,
    ContactModule,
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    SharedModule,
    RouterModule
  ],
  providers: [
    AppConfigService,
    Globals,
    { provide: APP_INITIALIZER, useFactory: loadConfigService , deps: [AppConfigService], multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
