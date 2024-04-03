import { TestBed } from '@angular/core/testing';

import { BcfFilesMessengerService } from './bcf-files-messenger.service';

describe('BcfFilesMessengerService', () => {
  let service: BcfFilesMessengerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BcfFilesMessengerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
