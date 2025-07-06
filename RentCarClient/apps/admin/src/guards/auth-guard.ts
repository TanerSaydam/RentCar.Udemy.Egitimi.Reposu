import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Common } from '../services/common';

export const authGuard: CanActivateChildFn = (childRoute, state) => {
  const token = localStorage.getItem('response');
  const router = inject(Router);
  const common = inject(Common);

  if(!token){
    router.navigateByUrl('/login');
    return false;
  }

  try {
    const decode:any = jwtDecode(token);

    common.decode().id = decode['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    common.decode().email = decode['email'];
    common.decode().fullName = decode['fullName'];
    common.decode().fullNameWithEmail = decode['fullNameWithEmail'];
    common.decode().role = decode['role'];
    common.decode().permissions = JSON.parse(decode['permissions']);
    common.decode().branch = decode['branch'];

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
