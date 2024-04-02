import { AppConfigService } from './services/AppConfigService';
import { ApplicationConfig } from '@angular/core';
import { BcfConversionService } from './services/BcfConverfsionService';
import { RevitBcfConversionService } from './services/RevitBcfConversionService';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import { routes } from './app.routes';

const frontendConfigService = new AppConfigService();

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideAnimationsAsync(), provideHttpClient(), {
    provide: BcfConversionService,
    useClass: frontendConfigService.getFrontendConfig().isInElectronMode
    ? BcfConversionService
    : RevitBcfConversionService
  },
  provideToastr({
    positionClass: 'toast-bottom-right',
    preventDuplicates: true,
    closeButton: true
  })]
};
