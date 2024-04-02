import { BcfFile } from '../generated/models';
import { BcfFileComponent } from './components/bcf-file/bcf-file.component';
import { BcfFilesMessengerService } from './services/bcf-files-messenger.service';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Observable } from 'rxjs';
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

  constructor(private bcfFilesMessengerService: BcfFilesMessengerService) {
    this.bcfFiles = bcfFilesMessengerService.bcfFiles;
  }

  closeBcfFile(bcfFile: BcfFile): void {
    this.bcfFilesMessengerService.closeBcfFile(bcfFile);
  }
}
