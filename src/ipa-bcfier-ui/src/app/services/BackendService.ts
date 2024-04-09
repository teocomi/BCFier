import { BcfFile, BcfViewpoint, Settings } from '../../generated/models';
import { Observable, Subject, catchError, of, tap } from 'rxjs';

import { AddSnapshotViewpointComponent } from '../components/add-snapshot-viewpoint/add-snapshot-viewpoint.component';
import { AppConfigService } from './AppConfigService';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoadingService } from './loading.service';
import { MatDialog } from '@angular/material/dialog';
import { NotificationsService } from './notifications.service';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(
    private http: HttpClient,
    private matDialog: MatDialog,
    private appConfigService: AppConfigService,
    private loadingService: LoadingService,
    private notificationsService: NotificationsService
  ) {}

  importBcfFile(): Observable<BcfFile> {
    return this.http.post<BcfFile>('/api/bcf-conversion/import', null);
  }

  mergeBcfFile(): Observable<BcfFile> {
    return this.http.post<BcfFile>('/api/bcf-conversion/merge', null);
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
    if (this.appConfigService.getFrontendConfig().isConnectedToRevit) {
      this.loadingService.showLoadingScreen();
      return this.http.post<BcfViewpoint>('/api/viewpoints', null).pipe(
        tap(() => this.loadingService.hideLoadingScreen()),
        catchError(() => {
          this.loadingService.hideLoadingScreen();
          this.notificationsService.error('Failed to add viewpoint.');
          return of(null);
        })
      );
    } else {
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

  selectViewpoint(viewpoint: BcfViewpoint): void {
    if (this.appConfigService.getFrontendConfig().isConnectedToRevit) {
      this.loadingService.showLoadingScreen();
      this.http.post('/api/viewpoints/visualization', viewpoint).subscribe({
        next: () => this.loadingService.hideLoadingScreen(),
        error: () => {
          this.loadingService.hideLoadingScreen();
          this.notificationsService.error('Failed to select viewpoint.');
        },
      });
    } else {
      // Not doing anything in the standalone version
    }
  }
}
