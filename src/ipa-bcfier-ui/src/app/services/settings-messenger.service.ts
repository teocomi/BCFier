import { AppConfigService } from './AppConfigService';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Settings } from '../../generated/models';

@Injectable({
  providedIn: 'root',
})
export class SettingsMessengerService {
  private settingsSource = new ReplaySubject<Settings>(1);
  settings = this.settingsSource.asObservable();

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) {
    if (appConfigService.getFrontendConfig().isInElectronMode) {
      this.http.get<Settings>('/api/settings').subscribe((settings) => {
        this.settingsSource.next(settings);
      });
    } else {
      throw new Error('Not implemented');
    }
  }

  saveSettings(settings: Settings): void {
    if (this.appConfigService.getFrontendConfig().isInElectronMode) {
      this.http.put('/api/settings', settings).subscribe(() => {
        this.settingsSource.next(settings);
      });
    } else {
      throw new Error('Not implemented');
    }
  }
}
