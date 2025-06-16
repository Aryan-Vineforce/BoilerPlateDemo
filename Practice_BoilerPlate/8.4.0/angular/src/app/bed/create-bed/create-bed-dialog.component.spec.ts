import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBedDialogComponent } from './create-bed-dialog.component';

describe('CreateBedDialogComponent', () => {
  let component: CreateBedDialogComponent;
  let fixture: ComponentFixture<CreateBedDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateBedDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateBedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
