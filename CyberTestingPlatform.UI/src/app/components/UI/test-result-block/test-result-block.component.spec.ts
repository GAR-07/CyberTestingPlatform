import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestResultBlockComponent } from './test-result-block.component';

describe('TestResultBlockComponent', () => {
  let component: TestResultBlockComponent;
  let fixture: ComponentFixture<TestResultBlockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TestResultBlockComponent]
    });
    fixture = TestBed.createComponent(TestResultBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
