import {
  Component,
  Injector,
  OnInit,
  Output,
  EventEmitter,
  Input,
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import {
  DealDto,
  TaskDto,
  DealWithTasksServiceProxy,
  CreateUpdateDealDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import * as moment from '@node_modules/moment-timezone';

@Component({
  selector: 'app-edit-deal-dialog',
  templateUrl: './edit-deal-dialog.component.html'
})
export class EditDealDialogComponent {
  saving = false;
  deal: DealDto = new DealDto();

  selectedDealDate: string;
  onSave: () => void;
    @Output() onUpdate = new EventEmitter<any>();
  constructor(
    public bsModalRef: BsModalRef,
    private _dealService: DealWithTasksServiceProxy
  ) {}

  ngOnInit(): void {
      if (this.deal.date) {
      this.selectedDealDate = moment(this.deal.date).format("YYYY-MM-DD");
    }
    if (this.deal.tasks) {
    this.deal.tasks.forEach((task) => {
      if (task.dateFrom) {
        task.dateFrom = moment(task.dateFrom).format("YYYY-MM-DD") as any;
      }
      if (task.toDate) {
        task.toDate = moment(task.toDate).format("YYYY-MM-DD") as any;
      }
    });
  }
  }

  save(): void {
    this.saving = true;
    this.deal.date = this.selectedDealDate as any;
    this.deal.date = moment(this.selectedDealDate);
    const input = new CreateUpdateDealDto();
    input.init(this.deal); // copy properties from deal to DTO

    this._dealService
      .updateDealWithTasks(this.deal.id, input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        abp.notify.success('Saved successfully');
        this.bsModalRef.hide();
        if (this.onSave) this.onSave();
      });
  }

  addTask(): void {
    const newTask = new TaskDto();
    newTask.taskNo = this.deal.tasks?.length + 1 || 1;
    if (!this.deal.tasks) {
      this.deal.tasks = [];
    }
    this.deal.tasks.push(newTask);
  }

  removeTask(index: number): void {
    if (this.deal.tasks && this.deal.tasks.length > index) {
      this.deal.tasks.splice(index, 1);
    }
  }
}
