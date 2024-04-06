import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { Injectable } from '@angular/core';
import { LoadingScreenComponent } from '../components/loading-screen/loading-screen.component';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private matDialogRef: MatDialogRef<LoadingScreenComponent> | null = null;

  constructor(private matDialog: MatDialog) {}

  public showLoadingScreen(): void {
    if (this.matDialogRef) {
      return;
    }

    this.matDialogRef = this.matDialog.open(LoadingScreenComponent, {
      disableClose: true,
    });
  }

  public hideLoadingScreen(): void {
    if (!this.matDialogRef) {
      return;
    }

    this.matDialogRef.close();
    this.matDialogRef = null;
  }
}
