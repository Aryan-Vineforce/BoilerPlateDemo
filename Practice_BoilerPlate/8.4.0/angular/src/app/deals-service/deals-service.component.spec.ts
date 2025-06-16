import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DealsServiceComponent } from './deals-service.component';

describe('DealsServiceComponent', () => {
  let component: DealsServiceComponent;
  let fixture: ComponentFixture<DealsServiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DealsServiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DealsServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
