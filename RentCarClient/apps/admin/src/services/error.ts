import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FlexiToastService } from 'flexi-toast';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  readonly #toast = inject(FlexiToastService);

  handle(err: HttpErrorResponse) {
    console.log(err);
    const status = err.status;
    if (status === 403 || status === 422 || status === 500) {
      const messages = err.error.errorMessages;
      messages.forEach((val: string) => {
        this.#toast.showToast("Hata!", val, "error");
      });
    }
  }
}
