import {Component, OnInit, TemplateRef} from "@angular/core";
import {UserProfileService} from "../../../../shared/services/userprofile.service";
import {UserProfile} from "../../../../shared/models/userprofile";
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {DeleteDeckResult, UserDecksService} from "../../services/user-decks.service";
import {Subscription} from "rxjs";
import {tap} from "rxjs/operators";
import {DeckSearchResult} from "../../../../shared/models/deck-search-result";
import {Deck} from "../../../../shared/models/deck";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {BsModalRef, BsModalService} from "ngx-bootstrap";
import {animate, group, style, transition, trigger} from "@angular/animations";

@Component({
  templateUrl: "./user-deck-list.page.html",
  animations: [
    trigger('itemAnim', [
      transition(':enter', [
        style({ transform: 'translateY(-20%)' }),
        animate(500)
      ]),
      transition(':leave', [
        group([
          animate('0.5s ease', style({ transform: 'translateY(-20%)', 'height':'0px' })),
          animate('0.5s 0.2s ease', style({ opacity: 0 }))
        ])
      ])
    ])
  ]
})
export class UserDeckListPage implements OnInit {
  public user: UserProfile;
  public modalRef: BsModalRef;
  public totalDecks: number;
  public decks: Deck[];
  public isLoading: boolean = true;
  public isDeletingDeck: boolean = false;

  public maxSize: number = 10;
  public currentPage = 1;
  public pageSize: number = 7;
  public rotate = true;

  // form
  public searchForm : FormGroup;
  public searchTermControl: FormControl = new FormControl(null);
  public searchTextControlSubscription: Subscription;

  // Subscriptions
  private subscriptions: Subscription[] = [];
  private deckToDeleteId: number;

  constructor
  (
    private userProfileService: UserProfileService,
    private fb: FormBuilder,
    private userDecksService : UserDecksService,
    private configuration: AppConfigService,
    private modalService: BsModalService
  ) {
    this.user = userProfileService.getUserProfile();
  }

  openModal(template: TemplateRef<any>, deckId: number) {
    this.deckToDeleteId = deckId;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public search(page: number, searchText: string = null) : void {
    this.userDecksService
      .authenticatedUserDecks(searchText, this.pageSize, page)
      .pipe(
        tap(() => { this.isLoading = false;})
      )
      .subscribe((deckSearchResult: DeckSearchResult) => {
        this.totalDecks = deckSearchResult.totalDecks;
        this.decks = deckSearchResult.decks;
      });
  }

  ngOnInit(): void {
    this.searchForm = this.fb.group({
      searchTerm: this.searchTermControl
    });

    this.searchTextControlSubscription = this.searchTermControl
      .valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe((newValue: string) => {
        this.search(1, newValue)
      });

    this.search(1);

    this.subscriptions = [...this.subscriptions, this.searchTextControlSubscription]
  }

  public pageChanged(event: any): void {
    this.isLoading = true;
    this.search(event.page);
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  public confirmDelete(): void {
    this.isDeletingDeck = true;

    this.userDecksService
      .deleteDeckById(this.deckToDeleteId)
      .pipe(
        tap(() => this.isDeletingDeck = false)
      )
      .subscribe((deleteDeckResult: DeleteDeckResult) => {
        const deckIndex = this.decks.findIndex(deck => deck.id === deleteDeckResult.id);
        this.decks.splice(deckIndex, 1);
        this.modalRef.hide();
      });
  }

  public declineDelete(): void {
    this.modalRef.hide();
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
