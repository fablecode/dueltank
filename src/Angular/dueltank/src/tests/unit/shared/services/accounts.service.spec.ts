import {AccountsService} from "../../../../app/shared/services/accounts.service";
import {UserProfile} from "../../../../app/shared/models/userprofile";
import {AppConfigService} from "../../../../app/shared/services/app-config.service";

describe("AccountService", () => {

  let httpClientSpy: {get: jasmine.Spy};
  let appConfigServiceSpy: jasmine.SpyObj<AppConfigService>;;
  let accountsService: AccountsService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj("HttpClient", ["get"]);
    appConfigServiceSpy = jasmine.createSpyObj("AppConfigService", ["apiEndpoint"]);

    accountsService = new AccountsService(<any> httpClientSpy, appConfigServiceSpy);
  });


  it("should invoke HttpClient once", () => {
    let expectedProfile : UserProfile = new UserProfile();

    httpClientSpy.get.and.returnValue({ subscribe: () => { return new UserProfile()} });
    appConfigServiceSpy.apiEndpoint.and.returnValue("")

    accountsService.profile().subscribe(
      profile => expect(profile.valueOf).toEqual(expectedProfile, "expected profile"),
      fail
    );

    expect(httpClientSpy.get.calls.count()).toBe(1, "One call");
  });
});
