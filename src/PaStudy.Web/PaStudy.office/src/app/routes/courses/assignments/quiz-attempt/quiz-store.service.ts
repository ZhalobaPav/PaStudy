import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class QuizServiceStore {
  private _isAttemptActive = signal<boolean>(false);

  public isAttemptActive = this._isAttemptActive.asReadonly();

  public startAttemptLogic() {
    this._isAttemptActive.set(true);
  }

  public stopAttemptLogic() {
    this._isAttemptActive.set(false);
  }
}
