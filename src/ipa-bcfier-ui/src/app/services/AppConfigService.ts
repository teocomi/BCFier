import { FrontendConfig } from '../../generated/models';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AppConfigService {
  private defaultFrontendConfig: FrontendConfig = {
    isInElectronMode: false,
    isConnectedToRevit: false,
  };

  getFrontendConfig(): FrontendConfig {
    return (
      ((window as any)['ipaBcfierFrontendConfig'] as FrontendConfig) ||
      this.defaultFrontendConfig
    );
  }
}
