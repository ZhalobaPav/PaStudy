export interface BaseResponse<T> {
  data?: T;
  succeeded?: boolean;
  message?: string;
  errors: string[];
}
