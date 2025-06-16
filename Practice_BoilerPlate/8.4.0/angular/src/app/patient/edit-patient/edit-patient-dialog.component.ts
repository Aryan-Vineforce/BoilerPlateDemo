import {
  Component,
  EventEmitter,
  Injector,
  OnInit,
  Output,
} from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import {
  PatientServiceProxy,
  CreateUpdatePatientDto,
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/app-component-base";


@Component({
  selector: 'app-edit-patient-dialog',
  templateUrl: './edit-patient-dialog.component.html',
  styleUrls: ['./edit-patient-dialog.component.css']
})
export class EditPatientDialogComponent  extends AppComponentBase
  implements OnInit
{
  saving = false;
  patient = new CreateUpdatePatientDto();
  @Output() onUpdate = new EventEmitter<any>();
  selectedDate: string;
  sphone:string;
  selectedGender: import("c:/Users/Vineforce/source/repo/BoilerPlateDemo//Practice_BoilerPlate/8.4.0/angular/src/shared/service-proxies/service-proxies").Gender;
  constructor(
    injector: Injector,
    private _patientService: PatientServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }
  ngOnInit(): void {
   debugger
      // this.selectedDate = (this.student.dob ).toDate(); 
     
      this.selectedGender = this.patient.gender; // Moment → Date
      
      }
  update(): void {
    this.patient.gender = this.selectedGender;
     // from string to moment

    this.saving = true;
   
    console.log("clicked");

    console.log("Updated Student DTO:", this.patient);

    this._patientService.update(this.patient).subscribe(
      () => {
        this.notify.info(this.l("UpdatedSuccessfully"));
        this.bsModalRef.hide();
        this.onUpdate.emit(); // Refresh parent list
      },
      () => {
        this.saving = false;
      }
    );
  }
}