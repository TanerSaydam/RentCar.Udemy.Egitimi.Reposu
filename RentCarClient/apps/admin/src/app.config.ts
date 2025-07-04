import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { appRoutes } from './app.routes';
import { HttpContextToken, provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpInterceptor } from './interceptors/http-interceptor';
import { authInterceptor } from './interceptors/auth-interceptor';
import { errorInterceptor } from './interceptors/error-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(appRoutes),
    provideHttpClient(withInterceptors([
      httpInterceptor,
      authInterceptor,
      errorInterceptor
    ]))
  ],
};

export const SKIP_ERROR_HANDLER = new HttpContextToken<boolean>(() => false);
