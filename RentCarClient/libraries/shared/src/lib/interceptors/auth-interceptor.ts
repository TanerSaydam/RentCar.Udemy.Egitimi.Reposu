import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem("response");
  const clone = req.clone({
    headers: new HttpHeaders({
      "Authorization": token ? "Bearer " + token : ""
    })
  });
  return next(clone);
};
