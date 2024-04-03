import { AppConfigService } from './services/AppConfigService';
import { ApplicationConfig } from '@angular/core';
import { BackendService } from './services/BackendService';
import { IMAGE_CONFIG } from '@angular/common';
import { RevitBackendService } from './services/RevitBackendService';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import { routes } from './app.routes';

const frontendConfigService = new AppConfigService();

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(),
    {
      provide: BackendService,
      useClass: frontendConfigService.getFrontendConfig().isInElectronMode
        ? BackendService
        : RevitBackendService,
    },
    {
      provide: IMAGE_CONFIG,
      useValue: {
        disableImageSizeWarning: true,
        disableImageLazyLoadWarning: true,
      },
    },
    provideToastr({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      closeButton: true,
    }),
  ],
};
