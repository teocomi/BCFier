import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentsDetailComponent } from './comments-detail.component';

describe('CommentsDetailComponent', () => {
  let component: CommentsDetailComponent;
  let fixture: ComponentFixture<CommentsDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommentsDetailComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CommentsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
