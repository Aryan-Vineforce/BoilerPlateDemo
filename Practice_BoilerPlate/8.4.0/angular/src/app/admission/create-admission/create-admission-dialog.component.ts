import { Component,Injector } from '@angular/core';
import { AddressServiceProxy, AdmissionsServiceProxy, BedServiceProxy, CreateUpdateAdmissionDto, CreateUpdateEmployeDto,DepartmentCreateUpdateDto, GetAddressDto, GetAdmissionDto, GetBedDto, GetDepartmentDto, GetPatientDto, PatientServiceProxy } from '@shared/service-proxies/service-proxies';
import { EmployeeServiceProxy,DepartmentServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import {  EventEmitter, Output } from "@angular/core";
import { BsModalRef } from 'ngx-bootstrap/modal';


import {
  PagedListingComponentBase,
  PagedRequestDto,

} from "shared/paged-listing-component-base";
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from '@node_modules/moment-timezone';
// import { CreateEmployeeDialogComponent } from './create-employee/create-employee-dialog.component';
// import { EditEmployeeDialogComponent } from './edit-employee/edit-employee-dialog.component';

class PagedEmployeeRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  selector: 'app-create-admission-dialog',
  templateUrl: './create-admission-dialog.component.html',
  styleUrls: ['./create-admission-dialog.component.css']
})
export class CreateAdmissionDialogComponent extends AppComponentBase{
saving = false;

  admission = new CreateUpdateAdmissionDto ();
  patients: GetPatientDto[] = [];
  beds: GetBedDto[] = [];
  @Output() onSave = new EventEmitter<any>();
  selectedDate: Date; // Make sure DepartmentDto has name, code, description
  selectedDatee: Date; // Make sure DepartmentDto has name, code, description
  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _pService:PatientServiceProxy,
    private _addService:AdmissionsServiceProxy,
    private _bedservice :BedServiceProxy
  ) {
    super(injector);
  }
    ngOnInit() {
     this.loadPatients();
     this.loadBeds();
    this.admission.dischargeDate=moment(this.selectedDatee);
    this.admission.admitDate=moment(this.selectedDate); // Call loadDepartments to fetch the list of departments
  }
  loadPatients(): void {
    this._pService.getAll(undefined, undefined, undefined, undefined)
      .subscribe((result) => {
        this.patients = result.items;
      });
  }
  loadBeds():void{
    this._bedservice.getAll(undefined,undefined,undefined,undefined)
    .subscribe((result) => {
      this.beds = result.items;
    });
}
  
  // loadBeds(): void {
  //   this._addService.getAll(undefined, undefined, undefined, undefined)
  //     .subscribe((result) => {
  //       this.beds = result.items;
  //     });
  // }
  // loadDepartments(): void {
  //   this._addService.getAll(undefined,undefined,undefined,undefined)  // Fetch all departments from your API
  //     .subscribe((result) => {
  //       this. result.items;  // Assuming 'items' is the list of departments
  //     });
  // }


  save(): void {
    this.saving = true;
     this.admission.dischargeDate=moment(this.selectedDatee);
     this.admission.admitDate=moment(this.selectedDate);
    this._addService
      .create(this.admission)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit(); 
      });
  }
}



