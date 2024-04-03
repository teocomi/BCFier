import { BcfFile } from '../../generated/models';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(private http: HttpClient) {}

  importBcfFile(): Observable<BcfFile> {
    return this.http.post<BcfFile>('/api/bcf-conversion/import', null);
  }

  exportBcfFile(bcfFile: BcfFile): Observable<void> {
    return this.http.post<void>('/api/bcf-conversion/export', bcfFile);
  }
}
