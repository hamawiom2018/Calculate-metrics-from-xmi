import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDiagramDeletionComponent } from './confirm-diagram-deletion.component';

describe('ConfirmDiagramDeletionComponent', () => {
  let component: ConfirmDiagramDeletionComponent;
  let fixture: ComponentFixture<ConfirmDiagramDeletionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDiagramDeletionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDiagramDeletionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
