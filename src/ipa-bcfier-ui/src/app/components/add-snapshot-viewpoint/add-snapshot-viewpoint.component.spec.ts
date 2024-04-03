import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSnapshotViewpointComponent } from './add-snapshot-viewpoint.component';

describe('AddSnapshotViewpointComponent', () => {
  let component: AddSnapshotViewpointComponent;
  let fixture: ComponentFixture<AddSnapshotViewpointComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddSnapshotViewpointComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddSnapshotViewpointComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
