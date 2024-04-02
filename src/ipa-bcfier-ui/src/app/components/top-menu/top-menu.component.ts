import { BcfConversionService } from '../../services/BcfConverfsionService';
import { BcfFile } from '../../../generated/models';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'bcfier-top-menu',
  standalone: true,
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './top-menu.component.html',
  styleUrl: './top-menu.component.scss',
})
export class TopMenuComponent {
  constructor(private bcfConversionService: BcfConversionService) {}

  openBcf(): void {
    this.bcfConversionService.importBcfFile().subscribe({
      next: (bcfFile: BcfFile) => {
        console.log(bcfFile);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}
