import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDealDialogComponent } from './edit-deal-dialog.component';

describe('EditDealDialogComponent', () => {
  let component: EditDealDialogComponent;
  let fixture: ComponentFixture<EditDealDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditDealDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditDealDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
