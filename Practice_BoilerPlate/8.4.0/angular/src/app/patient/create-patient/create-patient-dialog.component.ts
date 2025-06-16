import { Component, EventEmitter, Injector, Output } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";

import {
  PatientServiceProxy,
  CreateUpdatePatientDto,
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/app-component-base";
import * as moment from "moment";


@Component({
  selector: 'app-create-patient-dialog',
  templateUrl: './create-patient-dialog.component.html',
  styleUrls: ['./create-patient-dialog.component.css']
})
export class CreatePatientDialogComponent extends AppComponentBase {
  saving = false;
  patient = new CreateUpdatePatientDto();

  @Output() onSave = new EventEmitter<any>();
  selectedDate: Date;
  constructor(
    injector: Injector,
    private _patientservice: PatientServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  //
  save(): void {
    console.log("Student DTO:", this.patient);

    this.saving = true;
    

    this._patientservice.create(this.patient).subscribe(
      () => {
        this.notify.info(this.l("SavedSuccessfully"));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
