import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, of } from 'rxjs';
import { ErrorService } from '../services/error';
import { SKIP_ERROR_HANDLER } from '../app.config';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  if(req.context.get(SKIP_ERROR_HANDLER)){
    return next(req);
  }

  const error = inject(ErrorService);
  return next(req).pipe(
    catchError((err:HttpErrorResponse) => {
      error.handle(err);
      return of();
    })
  );
};
