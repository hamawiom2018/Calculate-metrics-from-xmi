import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ElementMetricComponent } from './element-metric.component';

describe('ElementMetricComponent', () => {
  let component: ElementMetricComponent;
  let fixture: ComponentFixture<ElementMetricComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ElementMetricComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ElementMetricComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
