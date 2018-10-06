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
import {DeckCardFilterService} from "./services/deck-card-filter.service";
import {CardSearchService} from "../../shared/services/cardSearch.service";
import {DeckCardSearchResultComponent} from "./components/deck-card-search-result/deck-card-search-result.component";
import {DndListModule} from "ngx-drag-and-drop-lists";
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {DeckCardSearchResultService} from "./services/deck-card-search-result.service";
import {ToastrModule} from "ngx-toastr";
import {DeckCurrentCardTextComponent} from "./components/deck-current-card-text/deck-current-card-text.component";
import {DeckCurrentCardTextService} from "./services/deck-current-card-text.service";

const deckRoutes: Routes = [
  {
    path: "decks",
    children: [
      { path: '', component: DeckListPage },
      { path: ":id/:name", component: DeckViewPage}
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
    DeckCardFiltersComponent,
    DeckCardSearchResultComponent,
    DeckCurrentCardTextComponent
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
    DndListModule,
    InfiniteScrollModule,
    ToastrModule.forRoot(),
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
    DeckCardFilterService,
    DeckCardSearchResultService,
    DeckCurrentCardTextService,
    CardSearchService
  ]
})
export class DeckModule {}
