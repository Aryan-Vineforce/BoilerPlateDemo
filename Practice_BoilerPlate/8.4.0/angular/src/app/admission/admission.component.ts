import { Component, Injector } from "@angular/core";
import { finalize } from "rxjs/operators";
import { BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { GetAdmissionDto, GetSubjectDto } from '@shared/service-proxies/service-proxies';

import {
  PagedListingComponentBase,
  PagedRequestDto,
  
} from "shared/paged-listing-component-base";

import {

AdmissionsServiceProxy
} from "@shared/service-proxies/service-proxies";
import { CreateAdmissionDialogComponent } from "./create-admission/create-admission-dialog.component";
import { EditAdmissionDialogComponent } from "./edit-admission/edit-admission-dialog.component";
// import { CreateSubjectDialogComponent } from "./create-subject/create-subject-dialog.component";

// import { EditSubjectDialogComponent } from "./edit-subject/edit-subject-dialog.component";
class PagedDepartmentsRequestDto extends PagedRequestDto {
  keyword: string;
}
@Component({
  selector: 'app-admission',
  templateUrl: './admission.component.html',
  styleUrls: ['./admission.component.css']
})
export class AdmissionComponent extends PagedListingComponentBase<GetSubjectDto> {
 adds: GetAdmissionDto[] = [];
  isActive: boolean | null; 
  keyword = "";
  sorting = "name asc";
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _addservice: AdmissionsServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }
    createDepartment(): void {
       const createDialog = this._modalService.show(
         CreateAdmissionDialogComponent,
       { class: 'modal-lg' }
    );
     (createDialog.content as CreateAdmissionDialogComponent).onSave.subscribe(() => {
            this.refresh();
          });
    }
  
    
    
      editDepartment(subject: GetAdmissionDto): void {
      const editDialog = this._modalService.show(
          EditAdmissionDialogComponent,
         {
           class: 'modal-lg',
            initialState: {
           admission: Object.assign({}, subject) // Clone data to prevent 2-way binding issues
          },
          }
       );
      
        (editDialog.content as EditAdmissionDialogComponent).onUpdate.subscribe(() => {
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
      this.getDataPage(1);
    }
  
    protected list(
      request: PagedDepartmentsRequestDto,
      pageNumber: number,
      finishedCallback: Function
    ): void {
      request.keyword = this.keyword;
  
      this._addservice//api call
        .getAll(
          request.keyword,
      this.sorting,
          request.skipCount,
          request.maxResultCount
        )
        .pipe(finalize(() => finishedCallback()))
        .subscribe((result: any) => {
          this.adds = result.items;
          this.showPaging(result, pageNumber);
        });
    }
  
    protected delete(subject: GetSubjectDto): void {
      debugger
      abp.message.confirm(
        this.l("DeleteWarningMessage", subject.name),
        undefined,
        (result: boolean) => {
          if (result) {
            this._addservice.delete(subject.id).subscribe(() => {
              abp.notify.success(this.l("SuccessfullyDeleted"));
              this.refresh();
            });
          }
        }
      );
    }
  }
  

