import {RouterModule, Routes} from "@angular/router";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {NgModule} from "@angular/core";
import {ModalModule, PaginationModule, TabsModule} from "ngx-bootstrap";
import {DeckListPage} from "./pages/deck-list/deck-list.page";
import {DeckViewPage} from "./pages/deck-view/deck-view.page";
import {CardNameToUrlPipe} from "../../shared/pipes/cardNameToUrl.pipe";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BrowserModule} from "@angular/platform-browser";
import {DeckViewCardSearchComponent} from "./components/deck-view-card-search/deck-view-card-search.component";
import {DeckViewFormOptionsComponent} from "./components/deck-view-form-options/deck-view-form-options.component";
import {DeckService} from "../../shared/services/deck.service";
import {SharedModule} from "../shared/shared.module";
import {ClipboardModule} from "ngx-clipboard";
import {YoutubePlayerModule } from 'ngx-youtube-player';
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
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {DeckCardSearchResultService} from "./services/deck-card-search-result.service";
import {ToastrModule} from "ngx-toastr";
import {DeckCurrentCardTextService} from "./services/deck-current-card-text.service";
import {RulingService} from "../../shared/services/ruling.service";
import {TipService} from "../../shared/services/tip.service";
import {DeckTypesComponent} from "./components/deck-types/deck-types.component";
import {MainDeckComponent} from "./components/main-deck/main-deck.component";
import {ExtraDeckComponent} from "./components/extra-deck/extra-deck.component";
import {SideDeckComponent} from "./components/side-deck/side-deck.component";
import {DndModule} from "ng2-dnd";
import {MainDeckService} from "./services/main-deck.service";
import {ExtraDeckService} from "./services/extra-deck.service";
import {SideDeckService} from "./services/side-deck.service";
import {DeckNewPage} from "./pages/deck-new/deck-new.page";
import {DeckNewCardSearchComponent} from "./components/deck-new-card-search/deck-new-card-search.component";
import {DeckInfoService} from "./services/deck-info.service";
import {DeckEditorPage} from "./pages/deck-editor/deck-editor.page";
import {DeckEditCardSearchComponent} from "./components/deck-edit-card-search/deck-edit-card-search.component";

const deckRoutes: Routes = [
  {path: "decks", component: DeckListPage},
  {
    path: "deck", 
    children: [
      { path: ":id/:name", component: DeckViewPage},
      { path: "new", component: DeckNewPage},
      { path: "editor/:id/:name", component: DeckEditorPage}
    ]
  }

];

@NgModule({
  declarations: [
    CardNameToUrlPipe,
    DeckListPage,
    DeckViewPage,
    DeckNewPage,
    DeckViewCardSearchComponent,
    DeckNewCardSearchComponent,
    DeckViewFormOptionsComponent,
    DeckCardFiltersComponent,
    DeckCardSearchResultComponent,
    DeckTypesComponent,
    MainDeckComponent,
    ExtraDeckComponent,
    SideDeckComponent,
    DeckEditorPage,
    DeckEditCardSearchComponent
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
    InfiniteScrollModule,
    RouterModule,
    PaginationModule.forRoot(),
    DndModule.forRoot(),
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
    CardSearchService,
    TipService,
    RulingService,
    MainDeckService,
    ExtraDeckService,
    SideDeckService,
    DeckInfoService
  ]
})
export class DeckModule {}
