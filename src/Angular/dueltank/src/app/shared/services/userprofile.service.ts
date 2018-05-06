import {Injectable} from "@angular/core";
import {UserProfile} from "../models/userprofile";

@Injectable()
export class UserProfileService {
  private profile_key: string = "auth_profile"

  constructor() {}

  public setUserProfile(userProfile: UserProfile) : void {
    localStorage.setItem(this.profile_key, JSON.stringify(userProfile));
  }

  public getUserProfile() : UserProfile {
    var retrievedObject = localStorage.getItem(this.profile_key);
    return JSON.parse(retrievedObject);
  }

  public removeUserProfile() {
    localStorage.removeItem(this.profile_key);
  }
}
