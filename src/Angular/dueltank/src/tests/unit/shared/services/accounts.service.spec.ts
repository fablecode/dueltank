import {AccountsService} from "../../../../app/shared/services/accounts.service";
import {UserProfile} from "../../../../app/shared/models/userprofile";
import {AppConfigService} from "../../../../app/shared/services/app-config.service";
import {AuthenticatedUser} from "../../../../app/shared/models/authentication/authenticateduser.model";
import {RegisterUser} from "../../../../app/shared/models/authentication/registeruser.model";

describe("AccountService", () => {

  let httpClientSpy: {get: jasmine.Spy, post: jasmine.Spy};
  let appConfigServiceSpy: jasmine.SpyObj<AppConfigService>;;
  let accountsService: AccountsService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj("HttpClient", ["get", "post"]);
    appConfigServiceSpy = jasmine.createSpyObj("AppConfigService", ["apiEndpoint"]);

    accountsService = new AccountsService(<any> httpClientSpy, appConfigServiceSpy);
  });


  it("Profile: should invoke HttpClient once", () => {
    let expectedProfile : UserProfile = new UserProfile();

    httpClientSpy.get.and.returnValue({ subscribe: () => { return new UserProfile()} });
    appConfigServiceSpy.apiEndpoint.and.returnValue("")

    accountsService.profile().subscribe(
      profile => expect(profile.valueOf).toEqual(expectedProfile, "expected profile"),
      fail
    );

    expect(httpClientSpy.get.calls.count()).toBe(1, "One call");
  });

  it("Register: should invoke HttpClient once", () => {
    let expectedUser : AuthenticatedUser = new AuthenticatedUser();

    httpClientSpy.post.and.returnValue({ subscribe: () => { return new AuthenticatedUser()} });
    appConfigServiceSpy.apiEndpoint.and.returnValue("")

    accountsService.register(new RegisterUser(), "").subscribe(
      profile => expect(profile.valueOf).toEqual(expectedUser, "expected user"),
      fail
    );

    expect(httpClientSpy.post.calls.count()).toBe(1, "One call");
  });

});
