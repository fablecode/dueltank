import {UserProfile} from "../userprofile";

export class AuthenticatedUser {
  token: string;
  user: UserProfile;
}
