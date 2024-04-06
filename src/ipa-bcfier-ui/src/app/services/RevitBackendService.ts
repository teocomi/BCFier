import { BcfFile, BcfViewpoint, Settings } from '../../generated/models';
import { NgZone, inject } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';

export class RevitBackendService {
  private javascriptBridge: {
    sendDataToRevit: (
      data: string,
      callback: (jsonValue: string) => void
    ) => void;
  } | null = null;

  private ngZone: NgZone = inject(NgZone);

  constructor() {
    const setupBridge = () => {
      this.javascriptBridge = (window as any).bcfierJavascriptBridge;
      if (!this.javascriptBridge) {
        setTimeout(setupBridge, 100);
      }
    };

    setupBridge();
  }

  importBcfFile(): Observable<BcfFile> {
    return this.sendCommand<BcfFile>('importBcfFile', null);
  }

  exportBcfFile(bcfFile: BcfFile): Observable<void> {
    return this.sendCommand<void>('exportBcfFile', bcfFile);
  }

  openDocumentation(): void {
    this.sendCommand<void>('openDocumentation', null);
  }

  getSettings(): Observable<Settings> {
    return this.sendCommand<Settings>('getSettings', null);
  }

  saveSettings(settings: Settings): Observable<void> {
    return this.sendCommand<void>('setSettings', settings);
  }

  addViewpoint(): Observable<BcfViewpoint | null> {
    return this.sendCommand<BcfViewpoint>('createViewpoint', null);
  }

  selectViewpoint(viewpoint: BcfViewpoint): void {
    this.sendCommand('showViewpoint', viewpoint);
  }

  private sendCommand<T>(command: string, data: any): Observable<T> {
    const subject = new Subject<T>();
    this.javascriptBridge!.sendDataToRevit(
      JSON.stringify({
        command,
        data: JSON.stringify(data),
      }),
      (value) => {
        this.ngZone.run(() => {
          if (value) {
            subject.next(JSON.parse(value));
          } else {
            subject.next(null as T);
          }

          setTimeout(() => {
            subject.complete();
          }, 0);
        });
      }
    );

    return subject;
  }
}
