import { BackendService } from './BackendService';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Settings } from '../../generated/models';

@Injectable({
  providedIn: 'root',
})
export class SettingsMessengerService {
  private settingsSource = new ReplaySubject<Settings>(1);
  settings = this.settingsSource.asObservable();

  constructor(private backendService: BackendService) {
    backendService.getSettings().subscribe((settings) => {
      this.settingsSource.next(settings);
    });
  }

  saveSettings(settings: Settings): void {
    this.backendService.saveSettings(settings).subscribe(() => {
      this.settingsSource.next(settings);
    });
  }
}
