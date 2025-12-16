import { FilterUserProfile } from '../enums/filterUserProfile';

export interface UserFilter {
  searchTerm?: string;
  filterUserProfile: FilterUserProfile;
}
