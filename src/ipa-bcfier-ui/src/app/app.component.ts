import { Component, ViewChild } from '@angular/core';
import { MatTabGroup, MatTabsModule } from '@angular/material/tabs';
import { Observable, take } from 'rxjs';

import { BackendService } from './services/BackendService';
import { BcfFile } from '../generated/models';
import { BcfFileComponent } from './components/bcf-file/bcf-file.component';
import { BcfFilesMessengerService } from './services/bcf-files-messenger.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NotificationsService } from './services/notifications.service';
import { TopMenuComponent } from './components/top-menu/top-menu.component';

@Component({
  selector: 'bcfier-root',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatTabsModule,
    TopMenuComponent,
    CommonModule,
    BcfFileComponent,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  bcfFiles: Observable<BcfFile[]>;
  @ViewChild(MatTabGroup) tabGroup: MatTabGroup | undefined;

  constructor(
    private bcfFilesMessengerService: BcfFilesMessengerService,
    private backendService: BackendService,
    private notificationsService: NotificationsService
  ) {
    this.bcfFiles = bcfFilesMessengerService.bcfFiles;

    bcfFilesMessengerService.bcfFileSaveRequested.subscribe(() => {
      this.bcfFiles.pipe(take(1)).subscribe((bcfFiles) => {
        if (!this.tabGroup || this.tabGroup.selectedIndex == null) {
          return;
        }

        const selectedIndex = this.tabGroup.selectedIndex;
        const bcfFileToSave = bcfFiles[selectedIndex];
        this.backendService.exportBcfFile(bcfFileToSave).subscribe(() => {
          this.notificationsService.success('BCF file saved successfully.');
        });
      });
    });

    bcfFilesMessengerService.bcfFileSelected.subscribe((bcfFile) => {
      bcfFilesMessengerService.bcfFiles.pipe(take(1)).subscribe((bcfFiles) => {
        if (!this.tabGroup) {
          return;
        }

        this.tabGroup.selectedIndex = bcfFiles.indexOf(bcfFile);
      });
    });
  }

  closeBcfFile(bcfFile: BcfFile): void {
    this.bcfFilesMessengerService.closeBcfFile(bcfFile);
  }
}
