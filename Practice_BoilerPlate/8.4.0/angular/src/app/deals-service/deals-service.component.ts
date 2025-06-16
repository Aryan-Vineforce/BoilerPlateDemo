import { Component, Injector, Output } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
  DealDto,
  DealWithTasksServiceProxy,
  TaskDto,
} from '@shared/service-proxies/service-proxies';
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from 'shared/paged-listing-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateDealDialogComponent } from './create-deal/create-deal-dialog.component';
import { BsModalService } from '@node_modules/ngx-bootstrap/modal';
import { EditDealDialogComponent } from './edit-deal/edit-deal-dialog.component';
import { EventEmitter } from 'stream';

class PagedDealsRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  selector: 'app-deals-service',
  templateUrl: './deals-service.component.html',
  styleUrls: ['./deals-service.component.css'],
  animations: [appModuleAnimation()],
})
export class DealsServiceComponent extends PagedListingComponentBase<DealDto> {
  dealsWithTasks: DealDto[] = [];
  task:TaskDto[]=[];
  tasks: any[] = [];
  keyword = '';
  sorting = 'dealName asc';
 
  constructor(
    injector: Injector,
    private _dealsService: DealWithTasksServiceProxy,
    private _modalService: BsModalService
  
  ) {
    super(injector);
  }

  protected list(
    request: PagedDealsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._dealsService
      .getDealWithTasks(
        request.keyword,
        this.sorting,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe((result) => {
        this.dealsWithTasks = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  changeSorting(field: string): void {
    const isAsc = this.sorting === `${field} asc`;
    this.sorting = isAsc ? `${field} desc` : `${field} asc`;
    this.refresh();
  }

  clearFilters(): void {
    this.keyword = '';
    this.getDataPage(1);
  }
  
createDeal()
{
  const createDialog = this._modalService.show(
           CreateDealDialogComponent,
         { class: 'modal-lg' }
      );
       (createDialog.content as CreateDealDialogComponent).onSave.subscribe(() => {
              this.refresh();
            });

}
editTask(deal: DealDto): void {
   const editDialog = this._modalService.show(
            EditDealDialogComponent,
           {
              class: 'modal-lg',
              initialState: {
              deal: Object.assign({}, deal) // Clone data to prevent 2-way binding issues
             },
            }
         );
        
           (editDialog.content as EditDealDialogComponent).onUpdate.subscribe(() => {
            this.refresh();
          });
}
deleteTask(task: TaskDto):void {
  abp.message.confirm(
    this.l("DeleteWarningMessage", task.taskNo),
    undefined,
    (result: boolean) => {
      if (result) {
        this._dealsService.deleteTask(task.id).subscribe(() => {
          abp.notify.success(this.l("SuccessfullyDeleted"));
          this.refresh();
        });
      }
    }
  );
}

  
protected delete(deal:DealDto): void {
   abp.message.confirm(
    this.l("DeleteWarningMessage", deal.dealName),
     undefined,
     (result: boolean) => {
      if (result) {
        this._dealsService.delete(deal.id).subscribe(() => {
          abp.notify.success(this.l("SuccessfullyDeleted"));
          this.refresh();
         });
      }
    }
   );
}
}
  