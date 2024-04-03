import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicDetailComponent } from './topic-detail.component';

describe('TopicDetailComponent', () => {
  let component: TopicDetailComponent;
  let fixture: ComponentFixture<TopicDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopicDetailComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopicDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
