import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseBlockComponent } from './course-block.component';

describe('CourseBlockComponent', () => {
  let component: CourseBlockComponent;
  let fixture: ComponentFixture<CourseBlockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CourseBlockComponent]
    });
    fixture = TestBed.createComponent(CourseBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
