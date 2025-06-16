import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDealDialogComponent } from './create-deal-dialog.component';

describe('CreateDealDialogComponent', () => {
  let component: CreateDealDialogComponent;
  let fixture: ComponentFixture<CreateDealDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateDealDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDealDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
