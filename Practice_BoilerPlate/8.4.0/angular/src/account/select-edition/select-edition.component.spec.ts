import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectEditionComponent } from './select-edition.component';

describe('SelectEditionComponent', () => {
  let component: SelectEditionComponent;
  let fixture: ComponentFixture<SelectEditionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectEditionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectEditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
