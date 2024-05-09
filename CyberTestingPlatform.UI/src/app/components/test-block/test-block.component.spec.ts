import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestBlockComponent } from './test-block.component';

describe('TestFormComponent', () => {
  let component: TestBlockComponent;
  let fixture: ComponentFixture<TestBlockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TestBlockComponent]
    });
    fixture = TestBed.createComponent(TestBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
