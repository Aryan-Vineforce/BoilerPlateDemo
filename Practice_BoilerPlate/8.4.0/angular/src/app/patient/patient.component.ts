import { Component, Injector } from "@angular/core";
import { finalize } from "rxjs/operators";
import { BsModalService } from "ngx-bootstrap/modal";
import Swal from "sweetalert2";
import { appModuleAnimation } from "@shared/animations/routerTransition";
// import { CreateStudentDialogComponent } from './create-student/create-student-dialog.component';
// import { EditStudentDialogComponent } from './edit-student/edit-student-dialog.component';
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";
import {
  GetPatientDto,
  PatientServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { CreatePatientDialogComponent } from "./create-patient/create-patient-dialog.component";
import { EditPatientDialogComponent } from "./edit-patient/edit-patient-dialog.component";

class PagedPatientRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
  sorting: string;
}
import { ChartOptions } from "chart.js";
import * as XLSX from "xlsx";
import * as FileSaver from "file-saver";
@Component({
  selector: "app-patient",
  templateUrl: "./patient.component.html",
})
export class PatientComponent extends PagedListingComponentBase<GetPatientDto> {
  patients: GetPatientDto[] = [];
  keyword = "";
  isActive: boolean | null;
  advancedFiltersVisible = false;
  sorting = "name asc";

  //  Chart data
  public genderChartLabels: string[] = ["Male", "Female", "Other"];
  public genderChartData: any[] = [
    { data: [0, 0, 0], backgroundColor: ["#4E79A7", "#A0CBE8", "#F28E2B"] },
  ];
  public genderChartType: "pie" | "bar" | "line" = "pie";
  public genderChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
  };
  public diseaseChartLabels: string[] = ["Abc", "MonkeyPox", "ChickenPox"];
  public diseaseChartData: any[] = [
    { data: [0, 0, 0], backgroundColor: ["#4E79A7", "#A0CBE8", "#F28E2B"] },
  ];
  public diseaseChartType: "bar" = "bar";
  public diseaseChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
  };
  activeTab: string = "list";
  constructor(
    injector: Injector,
    private _patienservice: PatientServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  createStudent(): void {
    const createDialog = this._modalService.show(CreatePatientDialogComponent, {
      class: "modal-lg",
    });

    (createDialog.content as CreatePatientDialogComponent).onSave.subscribe(
      () => {
        this.refresh();
        this.showSuccessPopup();
      }
    );
  }

  editStudent(student: GetPatientDto): void {
    const editDialog = this._modalService.show(EditPatientDialogComponent, {
      class: "modal-lg",
      initialState: {
        patient: Object.assign({}, student), // Clone data to prevent 2-way binding issues
      },
    });

    (editDialog.content as EditPatientDialogComponent).onUpdate.subscribe(
      () => {
        this.refresh();
      }
    );
  }

  exportToExcel(): void {
    const worksheet = XLSX.utils.json_to_sheet(
      this.patients.map((p) => ({
        Name: p.name,
        Age: p.age,
        Gender: this.getGenderString(p.gender),
        Disease: p.disease,
        Doctor: p.doctor,
      }))
    );

    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Patients");

    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });

    const blob = new Blob([excelBuffer], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });

    FileSaver.saveAs(blob, `Patient_List_${new Date().getTime()}.xlsx`);
  }
  clearFilters(): void {
    this.keyword = "";
    this.isActive = undefined;
    this.getDataPage(1);
  }

  protected list(
    request: PagedPatientRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    debugger;
    request.keyword = this.keyword;
    request.isActive = this.isActive;
    request.sorting = this.sorting;

    this._patienservice
      .getAll(
        request.keyword,
        request.sorting,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe((result: any) => {
        this.patients = result.items;
        this.showPaging(result, pageNumber);
        this.updateChart();
      });
  }
  showSuccessPopup(): void {
    Swal.fire({
      icon: "success",
      title: "Success",
      text: "Student has been successfully created!",
      confirmButtonText: "Ok, got it!",
      customClass: {
        confirmButton: "btn btn-primary",
      },
      buttonsStyling: false,
    });
  }
  protected delete(student: GetPatientDto): void {
    abp.message.confirm(
      this.l("UserDeleteWarningMessage", student.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._patienservice.delete(student.id).subscribe(() => {
            abp.notify.success(this.l("SuccessfullyDeleted"));
            this.refresh();
          });
        }
      }
    );
  }
  getGenderString(gender: number): string {
    switch (gender) {
      case 0:
        return "Male";
      case 1:
        return "Female";
      case 2:
        return "Other";
      default:
        return "Unknown";
    }
  }
  changeSorting(field: string): void {
    const isAsc = this.sorting === `${field} asc`;
    this.sorting = isAsc ? `${field} desc` : `${field} asc`;
    this.refresh();
  }
  updateChart(): void {
    const genderCounts = { male: 0, female: 0, other: 0 };
    const diseaseCounts = { abc: 0, monkeypox: 0, chickenpox: 0 };

    this.patients.forEach((gender) => {
      if (gender.gender === 0) genderCounts.male++;
      else if (gender.gender === 1) genderCounts.female++;
      else if (gender.gender === 2) genderCounts.other++;
    });
    this.patients.forEach((p) => {
      if (p.disease?.toLowerCase() === "abc") diseaseCounts.abc++;
      else if (p.disease?.toLowerCase() === "monkeypox")
        diseaseCounts.monkeypox++;
      else if (p.disease?.toLowerCase() === "chickenpox")
        diseaseCounts.chickenpox++;
    });
    this.genderChartData = [
      {
        data: [genderCounts.male, genderCounts.female, genderCounts.other],
        backgroundColor: ["#4E79A7", "#A0CBE8", "#F28E2B"],
      },
    ];
    this.diseaseChartData = [
      {
        data: [
          diseaseCounts.abc,
          diseaseCounts.monkeypox,
          diseaseCounts.chickenpox,
        ],
        backgroundColor: ["#4E79A7", "#A0CBE8", "#F28E2B"],
      },
    ];
  }
}
