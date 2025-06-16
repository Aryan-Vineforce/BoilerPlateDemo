import { Component, Injector, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
  BedServiceProxy,
  CreateUpdateDto
} from '../../../shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../shared/app-component-base';

@Component({
  selector: 'app-create-bed-dialog',
  templateUrl: './create-bed-dialog.component.html',
  styleUrls: ['./create-bed-dialog.component.css']
})
export class CreateBedDialogComponent extends AppComponentBase {
  saving = false;
  bed = new CreateUpdateDto();
  @Output() onSave = new EventEmitter<any>();

  bedTypes = [
    { value: 0, name: 'ICU' },
    { value: 1, name: 'General' },
    { value: 2, name: 'Private' }
  ];

  bedStatuses = [
    { value: 0, name: 'Available' },
    { value: 1, name: 'Occupied' },
    { value: 2, name: 'Maintenance' }
  ];

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _bedservice: BedServiceProxy
  ) {
    super(injector);
  }

  save(): void {
    this.saving = true;

    this._bedservice
      .create(this.bed)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      });
  }
}
