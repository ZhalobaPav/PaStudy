export interface LoginModel {
  email: string;
  password: string;
}

export interface LoginResponse {
  succeeded: boolean;
  token: string;
  errors?: string[];
}
