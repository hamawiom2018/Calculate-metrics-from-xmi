import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XmiUploadComponent } from './xmi-upload.component';

describe('XmiUploadComponent', () => {
  let component: XmiUploadComponent;
  let fixture: ComponentFixture<XmiUploadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XmiUploadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XmiUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
