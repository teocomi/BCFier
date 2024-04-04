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
    throw new Error('Method not implemented.');
  }

  getSettings(): Observable<Settings> {
    return this.sendCommand<Settings>('getSettings', null);
  }
  saveSettings(settings: Settings): Observable<void> {
    throw new Error('Method not implemented.');
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
        subject.next(JSON.parse(value));
        setTimeout(() => {
          subject.complete();
        }, 0);
      }
    );

    return subject;
  }
}
