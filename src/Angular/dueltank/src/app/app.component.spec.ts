import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import {SideNavigationDirective} from "./shared/directives/sideNavigation.directive";
import {SideNavigationComponent} from "./shared/components/sidenavigation/sidenavigation.component";
import {NavbarComponent} from "./shared/components/navbar/navbar.component";
import {HomeModule} from "./modules/home/home.module";
import {BrowserModule} from "@angular/platform-browser";
import {RouterModule} from "@angular/router";
import { APP_BASE_HREF } from '@angular/common'

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        NavbarComponent,
        SideNavigationComponent,
        SideNavigationDirective
      ],
      imports: [
        BrowserModule,
        HomeModule,
        RouterModule.forRoot([])
      ],
      providers: [{provide: APP_BASE_HREF, useValue : '/' }]
    }).compileComponents();
  }));
  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
  it(`should have as title 'app'`, async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('app');
  }));
  it('should render wrapper', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('div#page-wrapper')).toBeTruthy();
  }));
});
