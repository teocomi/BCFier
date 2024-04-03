import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';

import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { SettingsMessengerService } from '../../services/settings-messenger.service';

@Component({
  selector: 'bcfier-settings',
  standalone: true,
  imports: [MatDialogModule, FormsModule, MatInputModule, MatButtonModule],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss',
})
export class SettingsComponent implements OnInit, OnDestroy {
  constructor(
    private dialogRef: MatDialogRef<SettingsComponent>,
    private settingsMessengerService: SettingsMessengerService
  ) {}

  username: string = '';

  private destroyed$ = new Subject<void>();

  ngOnInit(): void {
    this.settingsMessengerService.settings
      .pipe(takeUntil(this.destroyed$))
      .subscribe((settings) => {
        this.username = settings.username;
      });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  save(): void {
    this.settingsMessengerService.saveSettings({ username: this.username });
    this.close();
  }

  close(): void {
    this.dialogRef.close();
  }
}
