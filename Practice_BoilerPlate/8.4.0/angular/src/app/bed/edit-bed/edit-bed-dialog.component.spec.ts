import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditBedDialogComponent } from './edit-bed-dialog.component';

describe('EditBedDialogComponent', () => {
  let component: EditBedDialogComponent;
  let fixture: ComponentFixture<EditBedDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditBedDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditBedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
