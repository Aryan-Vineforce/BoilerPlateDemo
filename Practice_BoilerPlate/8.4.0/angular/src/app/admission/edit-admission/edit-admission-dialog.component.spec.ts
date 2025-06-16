import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAdmissionDialogComponent } from './edit-admission-dialog.component';

describe('EditAdmissionDialogComponent', () => {
  let component: EditAdmissionDialogComponent;
  let fixture: ComponentFixture<EditAdmissionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditAdmissionDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditAdmissionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
