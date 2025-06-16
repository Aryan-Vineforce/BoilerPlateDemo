import { Component, Injector } from "@angular/core";
import { finalize } from "rxjs/operators";
import { BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { CreateDepartmentDialogComponent } from './create-department/create-department-dialog.component';



import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";

import {
  GetDepartmentDto,
  DepartmentServiceProxy
} from "@shared/service-proxies/service-proxies";
import { EditDepartmentDialogComponent } from "./edit-department/edit-department-dialog.component";


class PagedDepartmentsRequestDto extends PagedRequestDto {
  keyword: string;
  sorting: string;
}

@Component({
  selector: "app-department",
  templateUrl: "./department.component.html",
  styleUrls: ["./department.component.css"],
  animations: [appModuleAnimation()]
})
export class DepartmentComponent extends PagedListingComponentBase<GetDepartmentDto> {
  departments: GetDepartmentDto[] = [];
  isActive: boolean | null; 
  keyword = "";
  sorting = "name asc";
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _departmentService: DepartmentServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  createDepartment(): void {
    const createDialog = this._modalService.show(
      CreateDepartmentDialogComponent,
      { class: 'modal-lg' }
    );
     (createDialog.content as CreateDepartmentDialogComponent).onSave.subscribe(() => {
          this.refresh();
        });
  }

  
  
    editDepartment(department: GetDepartmentDto): void {
      const editDialog = this._modalService.show(
        EditDepartmentDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            department: Object.assign({}, department) // Clone data to prevent 2-way binding issues
          },
        }
      );
    
      (editDialog.content as EditDepartmentDialogComponent).onUpdate.subscribe(() => {
        this.refresh();
      });
    }
    


  clearFilters(): void {
    this.keyword = "";
    this.sorting = "name asc";
    this.getDataPage(1);
  }

  protected list(
    request: PagedDepartmentsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.sorting = this.sorting;

    this._departmentService//api call
      .getAll(
        request.keyword,
        request.sorting, 
        request.skipCount,
        request.maxResultCount
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe((result: any) => {
        this.departments = result.items;
        this.showPaging(result, pageNumber);
      });
  }
  changeSorting(field: string): void {
    const isAsc = this.sorting === `${field} asc`;
    this.sorting = isAsc ? `${field} desc` : `${field} asc`;
    this.refresh();
  }

  protected delete(department: GetDepartmentDto): void {
    debugger
    abp.message.confirm(
      this.l("DeleteWarningMessage", department.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._departmentService.delete(department.id).subscribe(() => {
            abp.notify.success(this.l("SuccessfullyDeleted"));
            this.refresh();
          });
        }
      }
    );
  }
}
