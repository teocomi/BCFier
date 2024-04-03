import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BcfFileComponent } from './bcf-file.component';

describe('BcfFileComponent', () => {
  let component: BcfFileComponent;
  let fixture: ComponentFixture<BcfFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BcfFileComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BcfFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
