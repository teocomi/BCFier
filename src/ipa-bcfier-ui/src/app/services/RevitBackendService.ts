import { BcfFile, BcfViewpoint, Settings } from '../../generated/models';
import { Observable, Subject, of } from 'rxjs';

export class RevitBackendService {
  private javascriptBridge: {
    sendDataToRevit: (
      data: string,
      callback: (jsonValue: string) => void
    ) => void;
  } | null = null;

  constructor() {
    const setupBridge = () => {
      this.javascriptBridge = (window as any).bcfierJavascriptBridge;
      if (!this.javascriptBridge) {
        setTimeout(setupBridge, 100);
      }
    };

    setupBridge();
  }

  importBcfFile(bcfFile: File): Observable<BcfFile> {
    throw new Error('Method not implemented.');
  }

  exportBcfFile(bcfFile: BcfFile): Observable<void> {
    throw new Error('Method not implemented.');
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
    throw new Error('Method not implemented.');
  }

  private sendCommand<T>(command: string, data: any): Observable<T> {
    const subject = new Subject<T>();
    this.javascriptBridge!.sendDataToRevit(
      JSON.stringify({
        command,
        data: JSON.stringify(data),
      }),
      (value) => {
        if (value) {
          subject.next(JSON.parse(value));
        } else {
          subject.next(null as T);
        }
        setTimeout(() => {
          subject.complete();
        }, 0);
      }
    );

    return subject;
  }
}
