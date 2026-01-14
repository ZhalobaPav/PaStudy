export class StorageService {
  static setItem(key: string, value: any) {
    if (value === null || value === undefined) {
      localStorage.removeItem(key);
    } else {
      localStorage.setItem(key, value);
    }
  }

  static removeItem(key: string) {
    localStorage.removeItem(key);
  }
}
