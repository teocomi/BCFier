import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { BcfConversionService } from '../../services/BcfConverfsionService';
import { BcfFile } from '../../../generated/models';
import { BcfFilesMessengerService } from '../../services/bcf-files-messenger.service';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NotificationsService } from '../../services/notifications.service';
import { SettingsComponent } from '../settings/settings.component';
import { version } from '../../version';

@Component({
  selector: 'bcfier-top-menu',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, MatDialogModule],
  templateUrl: './top-menu.component.html',
  styleUrl: './top-menu.component.scss',
})
export class TopMenuComponent {
  version = version.version;

  constructor(
    private bcfConversionService: BcfConversionService,
    private notificationsService: NotificationsService,
    private bcfFilesMessengerService: BcfFilesMessengerService,
    private matDialog: MatDialog
  ) {}

  openBcf(): void {
    this.bcfConversionService.importBcfFile().subscribe({
      next: (bcfFile: BcfFile) => {
        this.bcfFilesMessengerService.openBcfFile(bcfFile);
      },
      error: (error) => {
        this.notificationsService.error('Error during BCF import.');
      },
    });
  }

  openSettings(): void {
    this.matDialog.open(SettingsComponent);
  }
}
