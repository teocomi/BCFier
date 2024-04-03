import { TestBed } from '@angular/core/testing';

import { SettingsMessengerService } from './settings-messenger.service';

describe('SettingsMessengerService', () => {
  let service: SettingsMessengerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SettingsMessengerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
