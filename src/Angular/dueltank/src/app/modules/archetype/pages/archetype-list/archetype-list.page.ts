import {Component, OnDestroy, OnInit, ViewEncapsulation} from "@angular/core";
import {Archetype} from "../../../../shared/models/archetype";
import {ArchetypeService} from "../../services/archetype.service";
import {ArchetypeSearchResult} from "../../../../shared/models/archetype-search-result";
import {Subscription} from "rxjs";
import {tap} from "rxjs/operators";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";

@Component({
  templateUrl: "./archetype-list.page.html",
  styleUrls: ["./archetype-list.page.css"],
  encapsulation: ViewEncapsulation.None
})
export class ArchetypeListPage implements OnInit, OnDestroy {
  public archetypes: Archetype[];
  public searchText: string;

  // form
  public searchForm : FormGroup;
  public searchTermControl: FormControl = new FormControl(null);
  public searchTextControlSubscription: Subscription;


  public maxSize: number = 10;
  public currentPage = 1;
  public pageSize: number = 12;
  public rotate = true;

  public isLoading = true;
  public totalArchetypes: number;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor
  (
    private archetypeService: ArchetypeService,
    private configuration: AppConfigService,
    private fb: FormBuilder
  ){
    this.searchForm = this.fb.group({
      searchTerm: this.searchTermControl
    });
  }

  ngOnInit(): void {
    this.searchTextControlSubscription = this.searchTermControl.valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe((newValue: string) => {
        this.search(1, newValue)
      });

    this.search(1);

    this.subscriptions = [...this.subscriptions, this.searchTextControlSubscription]
  }

  public search(page: number, searchText: string = null) : void {
    this.archetypeService
      .search(searchText, this.pageSize, page)
      .pipe(
        tap(() => { this.isLoading = false;})
      )
      .subscribe((archetypeSearchResult: ArchetypeSearchResult) => {
        this.totalArchetypes = archetypeSearchResult.totalArchetypes;
        this.archetypes = archetypeSearchResult.archetypes;
      });
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  public pageChanged(event: any): void {
    this.isLoading = true;
    this.search(event.page)

  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
