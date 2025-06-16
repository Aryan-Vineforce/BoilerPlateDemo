import {
  Component,
  EventEmitter,
  Injector,
  OnInit,
  Output,
  Input,
} from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";

import {
  AdmissionsServiceProxy,
  PatientServiceProxy,
  BedServiceProxy,
  CreateUpdateAdmissionDto,
  GetPatientDto,
  GetBedDto
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/app-component-base";
import * as moment from "moment";

@Component({
  selector: 'app-edit-admission-dialog',
  templateUrl: './edit-admission-dialog.component.html',
  styleUrls: ['./edit-admission-dialog.component.css']
})
export class EditAdmissionDialogComponent extends AppComponentBase implements OnInit {
  @Input() admission: CreateUpdateAdmissionDto = new CreateUpdateAdmissionDto();
  @Output() onUpdate = new EventEmitter<any>();

  saving = false;
  patients: GetPatientDto[] = [];
  beds: GetBedDto[] = [];

  selectedDate: string;
  selectedDatee: string;

  constructor(
    injector: Injector,
    private _addService: AdmissionsServiceProxy,
    private _patientService: PatientServiceProxy,
    private _bedService: BedServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.admission.admitDate) {
      this.selectedDate = moment(this.admission.admitDate).format('YYYY-MM-DD');
    }
    if (this.admission.dischargeDate) {
      this.selectedDatee = moment(this.admission.dischargeDate).format('YYYY-MM-DD');
    }

    this._patientService.getAll(undefined, undefined, undefined, undefined).subscribe(result => {
      this.patients = result.items;
    });

    this._bedService.getAll(undefined, undefined, undefined, undefined).subscribe(result => {
      this.beds = result.items;
    });
  }

  update(): void {
    this.admission.admitDate = moment(this.selectedDate);
    this.admission.dischargeDate = this.selectedDatee ? moment(this.selectedDatee) : undefined;

    this.saving = true;
    this._addService.update(this.admission).subscribe(
      () => {
        this.notify.info(this.l("UpdatedSuccessfully"));
        this.bsModalRef.hide();
        this.onUpdate.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
