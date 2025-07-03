import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateChildFn = (childRoute, state) => {
  const token = localStorage.getItem('response');
  const router = inject(Router);
  if(!token){
    router.navigateByUrl('/login');
    return false;
  }

  try {
    const decode = jwtDecode(token);
    const now = new Date().getTime() / 1000;
    const exp = decode.exp ?? 0;

    if(exp <= now){
      router.navigateByUrl('/login');
      return false;
    }

    return true;
  } catch (error) {
    router.navigateByUrl('/login');
    return false;
  }
};
