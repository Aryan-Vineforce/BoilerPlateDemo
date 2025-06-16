import { Component, Injector, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
  BedServiceProxy,
  CreateUpdateDto
} from '../../../shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../shared/app-component-base';

@Component({
  selector: 'app-edit-bed-dialog',
  templateUrl: './edit-bed-dialog.component.html',
  styleUrls: ['./edit-bed-dialog.component.css']
})
export class EditBedDialogComponent extends AppComponentBase {
  saving = false;
  bed = new CreateUpdateDto();
  @Output() onUpdate = new EventEmitter<any>();

  bedTypes = [
    { value: 0, name: 'General' },
    { value: 1, name: 'ICU' },
    { value: 2, name: 'SemiICU' },
    { value: 3, name: 'Isolation' }
  ];

  bedStatuses = [
    { value: 0, name: 'Available' },
    { value: 1, name: 'Occupied' },
    { value: 2, name: 'Blocked' }
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
      .update(this.bed)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onUpdate.emit();
      });
  }
}
