export interface FetchOptions {
  take: number;
  skip: number;
  filters: any;
  sortColumn: string;
  sortOrder: SortOrder;
}

export enum SortOrder {
  ACCENDING = 'Accending',
  DESCENDING = 'Descending',
}
