import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LectureBlockComponent } from './lecture-block.component';

describe('LectureFormComponent', () => {
  let component: LectureBlockComponent;
  let fixture: ComponentFixture<LectureBlockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LectureBlockComponent]
    });
    fixture = TestBed.createComponent(LectureBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
