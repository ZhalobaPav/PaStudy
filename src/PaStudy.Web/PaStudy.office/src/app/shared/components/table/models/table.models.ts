export type FetchOptions<TFilter extends object> = BaseFetchOptions & TFilter;

export interface TableConfig {
  headers: Header[];
}

export interface Header {
  name: string;
  label: string;
  width: number;
  component: any;
  componentInputs?: any;
  canSort?: boolean;
}

export interface BaseFetchOptions {
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
