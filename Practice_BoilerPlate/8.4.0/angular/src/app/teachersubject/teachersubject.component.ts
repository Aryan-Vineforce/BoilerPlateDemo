import { Component, Injector } from "@angular/core";
import { finalize } from "rxjs/operators";
import { BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  GetSubjectDto,
  TeacherSubjectGetDto,
  TeacherSubjectServiceProxy,
} from "@shared/service-proxies/service-proxies";

import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";

import { CreateTeachersubjectDialogComponent } from "./create-subject/create-teachersubject-dialog.component";
import { EditTeacherSubjectDialogComponent } from "./edit-subject/edit-teachersubject-dialog.component";

class PagedDepartmentsRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  selector: "app-teachersubject",
  templateUrl: "./teachersubject.component.html",
  styleUrls: ["./teachersubject.component.css"],
})
export class TeachersubjectComponent extends PagedListingComponentBase<GetSubjectDto> {
  teachersubjects: TeacherSubjectGetDto[] = [];
  isActive: boolean | null;
  keyword = "";
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _teachersubjectservice: TeacherSubjectServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }
  createDepartment(): void {
    const createDialog = this._modalService.show(
      CreateTeachersubjectDialogComponent,
      { class: "modal-lg" }
    );
    (
      createDialog.content as CreateTeachersubjectDialogComponent
    ).onSave.subscribe(() => {
      this.refresh();
    });
  }

  editDepartment(subject: TeacherSubjectGetDto): void {
    const editDialog = this._modalService.show(
      EditTeacherSubjectDialogComponent,
      {
        class: "modal-lg",
        initialState: {
          subject: Object.assign({}, subject), // Clone data to prevent 2-way binding issues
        },
      }
    );

    (
      editDialog.content as EditTeacherSubjectDialogComponent
    ).onUpdate.subscribe(() => {
      this.refresh();
    });
  }

  clearFilters(): void {
    this.keyword = "";
    this.getDataPage(1);
  }

  protected list(
    request: PagedDepartmentsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._teachersubjectservice //api call
      .getAll(
        request.keyword,
        undefined,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe((result: any) => {
        this.teachersubjects = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  protected delete(subject: GetSubjectDto): void {
    debugger;
    abp.message.confirm(
      this.l("DeleteWarningMessage", subject.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._teachersubjectservice.delete(subject.id).subscribe(() => {
            abp.notify.success(this.l("SuccessfullyDeleted"));
            this.refresh();
          });
        }
      }
    );
  }
}
