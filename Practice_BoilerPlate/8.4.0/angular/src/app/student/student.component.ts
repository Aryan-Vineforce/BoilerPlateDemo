import { Component, Injector } from "@angular/core";
import { finalize } from "rxjs/operators";
import { BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { CreateStudentDialogComponent } from './create-student/create-student-dialog.component';
import { EditStudentDialogComponent } from './edit-student/edit-student-dialog.component';



import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";
import {
  GetStudentDto,
  StudentServiceProxy,
} from "@shared/service-proxies/service-proxies";

class PagedStudentsRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  selector: "app-student",
  templateUrl: "./student.component.html",
  styleUrls: ["./student.component.css"],
  animations: [appModuleAnimation()],
})
export class StudentComponent extends PagedListingComponentBase<GetStudentDto> {
  students: GetStudentDto[] = [];
  keyword = "";
  sorting= "name asc"
  isActive: boolean | null;
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _studentService: StudentServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  createStudent(): void {
    const createDialog = this._modalService.show(
      CreateStudentDialogComponent,
      {
        class: 'modal-lg',
      }
    );
  
    (createDialog.content as CreateStudentDialogComponent).onSave.subscribe(() => {
      this.refresh();
    });
  }
  

  editStudent(student: GetStudentDto): void {
    const editDialog = this._modalService.show(
      EditStudentDialogComponent,
      {
        class: 'modal-lg',
        initialState: {
          student: Object.assign({}, student) // Clone data to prevent 2-way binding issues
        },
      }
    );
  
    (editDialog.content as EditStudentDialogComponent).onUpdate.subscribe(() => {
      this.refresh();
    });
  }
  
  changeSorting(field: string): void {
    const isAsc = this.sorting === `${field} asc`;
    this.sorting = isAsc ? `${field} desc` : `${field} asc`;
    this.refresh();
  }

  clearFilters(): void {
    this.keyword = "";
    this.isActive = undefined;
    this.getDataPage(1);
  }

  protected list(
    
    request: PagedStudentsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    debugger
    request.keyword = this.keyword;
    request.isActive = this.isActive;

    this._studentService
      .getAll(
        request.keyword,
      this.sorting,              
      request.skipCount,
      request.maxResultCount
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe((result: any) => {
        this.students = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  protected delete(student: GetStudentDto): void {
    abp.message.confirm(
      this.l("UserDeleteWarningMessage", student.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._studentService.delete(student.id).subscribe(() => {
            abp.notify.success(this.l("SuccessfullyDeleted"));
            this.refresh();
          });
        }
      }
    );
  }
  getGenderString(gender: number): string {
    switch (gender) {
      case 0: return 'Male';
      case 1: return 'Female';
      case 2: return 'Other';
      default: return 'Unknown';
    }
}
}
