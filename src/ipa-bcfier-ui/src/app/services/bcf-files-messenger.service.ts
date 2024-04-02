import { BcfFile } from '../../generated/models';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BcfFilesMessengerService {
  private bcfFilesSubject: ReplaySubject<BcfFile[]> = new ReplaySubject<
    BcfFile[]
  >(1);
  public bcfFiles = this.bcfFilesSubject.asObservable();
  private currentBcfFiles: BcfFile[] = [];

  openBcfFile(bcfFile: BcfFile) {
    this.currentBcfFiles.push(bcfFile);
    this.bcfFilesSubject.next(this.currentBcfFiles);
  }

  closeBcfFile(bcfFile: BcfFile) {
    this.currentBcfFiles = this.currentBcfFiles.filter((f) => f !== bcfFile);
    this.bcfFilesSubject.next(this.currentBcfFiles);
  }
}
