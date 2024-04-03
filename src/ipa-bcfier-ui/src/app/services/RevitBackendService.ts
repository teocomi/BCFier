import { BcfFile } from '../../generated/models';
import { Observable } from 'rxjs';

export class RevitBackendService {
  importBcfFile(bcfFile: File): Observable<BcfFile> {
    throw new Error('Method not implemented.');
  }
  exportBcfFile(bcfFile: BcfFile): Observable<void> {
    throw new Error('Method not implemented.');
  }
}
