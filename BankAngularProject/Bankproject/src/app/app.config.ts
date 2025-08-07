import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { InterceptorTokenService } from './interceptor/interceptotoken.service';
 
export const API_CONFIG = {
  apiUrl: 'http://localhost:5296/',
}
export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),
        provideHttpClient(withInterceptorsFromDi()),
        {
          provide:HTTP_INTERCEPTORS,
          useClass:InterceptorTokenService,
          multi:true

        }

  ]
};
