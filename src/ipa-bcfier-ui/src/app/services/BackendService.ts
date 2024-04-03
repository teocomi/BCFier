import { BcfFile, BcfViewpoint, Settings } from '../../generated/models';
import { Observable, Subject } from 'rxjs';

import { AddSnapshotViewpointComponent } from '../components/add-snapshot-viewpoint/add-snapshot-viewpoint.component';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(private http: HttpClient, private matDialog: MatDialog) {}

  importBcfFile(): Observable<BcfFile> {
    return this.http.post<BcfFile>('/api/bcf-conversion/import', null);
  }

  exportBcfFile(bcfFile: BcfFile): Observable<void> {
    return this.http.post<void>('/api/bcf-conversion/export', bcfFile);
  }

  openDocumentation(): void {
    this.http.post('/api/documentation', null).subscribe(() => {
      /* Not doing anything with the result */
    });
  }

  getSettings(): Observable<Settings> {
    return this.http.get<Settings>('/api/settings');
  }

  saveSettings(settings: Settings): Observable<void> {
    const subject = new Subject<void>();
    this.http.put('/api/settings', settings).subscribe(() => {
      subject.next();
      setTimeout(() => {
        subject.complete();
      }, 0);
    });

    return subject.asObservable();
  }

  addViewpoint(): Observable<BcfViewpoint | null> {
    const subject = new Subject<BcfViewpoint | null>();
    this.matDialog
      .open(AddSnapshotViewpointComponent)
      .afterClosed()
      .subscribe((viewpoint) => {
        if (viewpoint) {
          subject.next(viewpoint);
        } else {
          subject.next(null);
        }
        setTimeout(() => {
          subject.complete();
        }, 0);
      });

    return subject.asObservable();
  }
}
