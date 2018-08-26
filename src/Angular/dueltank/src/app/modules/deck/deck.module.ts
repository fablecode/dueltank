import {RouterModule, Routes} from "@angular/router";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {NgModule} from "@angular/core";
import {ModalModule, TabsModule} from "ngx-bootstrap";
import {DeckListPage} from "./pages/deck-list/deck-list.page";
import {DeckViewPage} from "./pages/deck-view/deck-view.page";
import {CardNameToUrlPipe} from "../../shared/pipes/cardNameToUrl.pipe";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BrowserModule} from "@angular/platform-browser";
import {DeckCurrentCardComponent} from "./components/deck-current-card/deck-current-card.component";
import {DeckViewCardSearchComponent} from "./components/deck-view-card-search/deck-view-card-search.component";
import {DeckViewFormOptionsComponent} from "./components/deck-view-form-options/deck-view-form-options.component";
import {DeckService} from "../../shared/services/deck.service";
import {SelectedDeckResolve} from "./resolvers/selected-deck.resolve";
import {SharedModule} from "../shared/shared.module";
import {ClipboardModule} from "ngx-clipboard";
import { YoutubePlayerModule } from 'ngx-youtube-player';
import {SafePipe} from "../../shared/pipes/safe.pipe";
import {DeckCardFiltersComponent} from "./components/deck-card-filters/deck-card-filters.component";
import {FormatService} from "../../shared/services/format.service";
import {CategoryService} from "../../shared/services/category.service";
import {SubCategoryService} from "../../shared/services/subcategory.service";
import {AttributeService} from "../../shared/services/attribute.service";
import {TypeService} from "../../shared/services/type.service";
import {LimitService} from "../../shared/services/limit.service";

const deckRoutes: Routes = [
  {
    path: "decks",
    children: [
      { path: '', component: DeckListPage },
      {
        path: ":id/:name",
        component: DeckViewPage,
        resolve: {
          deck: SelectedDeckResolve
        }
      }
    ]
  }
];

@NgModule({
  declarations: [
    CardNameToUrlPipe,
    SafePipe,
    DeckCurrentCardComponent,
    DeckListPage,
    DeckViewPage,
    DeckViewCardSearchComponent,
    DeckViewFormOptionsComponent,
    DeckCardFiltersComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule, ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    ClipboardModule,
    YoutubePlayerModule,
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    RouterModule.forChild(deckRoutes)
  ],
  providers: [
    AuthGuard,
    DeckService,
    FormatService,
    CategoryService,
    SubCategoryService,
    AttributeService,
    TypeService,
    LimitService,
    SelectedDeckResolve
  ]
})
export class DeckModule {}
