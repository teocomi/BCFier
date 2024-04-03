import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddStringValueComponent } from './add-string-value.component';

describe('AddStringValueComponent', () => {
  let component: AddStringValueComponent;
  let fixture: ComponentFixture<AddStringValueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddStringValueComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddStringValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
