import { BcfFile, BcfViewpoint, Settings } from '../../generated/models';

import { Observable } from 'rxjs';

export class RevitBackendService {
  importBcfFile(bcfFile: File): Observable<BcfFile> {
    throw new Error('Method not implemented.');
  }
  exportBcfFile(bcfFile: BcfFile): Observable<void> {
    throw new Error('Method not implemented.');
  }
  openDocumentation(): void {
    throw new Error('Method not implemented.');
  }
  saveSettings(settings: Settings): Observable<void> {
    throw new Error('Method not implemented.');
  }
  addViewpoint(): Observable<BcfViewpoint | null> {
    throw new Error('Method not implemented.');
  }
}
