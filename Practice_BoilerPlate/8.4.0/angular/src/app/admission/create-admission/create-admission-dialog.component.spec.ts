import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAdmissionDialogComponent } from './create-admission-dialog.component';

describe('CreateAdmissionDialogComponent', () => {
  let component: CreateAdmissionDialogComponent;
  let fixture: ComponentFixture<CreateAdmissionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateAdmissionDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateAdmissionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
