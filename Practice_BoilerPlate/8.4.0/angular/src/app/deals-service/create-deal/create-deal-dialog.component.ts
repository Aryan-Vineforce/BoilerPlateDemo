import { Component, Injector } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import { finalize } from 'rxjs/operators';
import { EventEmitter, Output } from '@angular/core';
import {
  CreateUpdateDealDto,
  CreateUpdateTaskDto,
  DealWithTasksServiceProxy,
} from '@shared/service-proxies/service-proxies';
import * as moment from '@node_modules/moment-timezone';

@Component({
  selector: 'app-create-deal-dialog',
  templateUrl: './create-deal-dialog.component.html',
  styleUrls: ['./create-deal-dialog.component.css'],
})
export class CreateDealDialogComponent extends AppComponentBase {
  saving = false;
  deal = new CreateUpdateDealDto();
  @Output() onSave = new EventEmitter<any>();

  selectedDealDate: Date;
  dateFrom: Date; // moment.Moment or Date
  toDate: Date;   // moment.Moment or Date

  tasks: CreateUpdateTaskDto[] = [];

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _dealService: DealWithTasksServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {

    this.deal.tasks = this.tasks;
  }

  addTask(): void {
    const newTask = new CreateUpdateTaskDto();
    newTask.title = '';
    newTask.description = '';
    newTask.dateFrom = null;
    newTask.toDate = null;
    this.tasks.push(newTask);
  }

  removeTask(index: number): void {
    this.tasks.splice(index, 1);
  }

  save(): void {
  this.saving = true;

  // Convert the main deal date
  this.deal.date = this.selectedDealDate ? moment(this.selectedDealDate) : null;

  // Convert each task's dates if they exist
  this.deal.tasks.forEach(task => {
    task.dateFrom = task.dateFrom ? moment(task.dateFrom) : null;
    task.toDate = task.toDate ? moment(task.toDate) : null;
  });

  this._dealService
    .createDealWithTasks(this.deal)
    .pipe(finalize(() => (this.saving = false)))
    .subscribe(() => {
      this.notify.info(this.l('SavedSuccessfully'));
      this.bsModalRef.hide();
      this.onSave.emit();
    });
}
}
