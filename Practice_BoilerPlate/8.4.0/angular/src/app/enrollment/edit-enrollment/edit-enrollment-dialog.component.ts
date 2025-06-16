import { Component, Injector, OnInit, Output, EventEmitter,Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';

import { finalize } from 'rxjs/operators';
import { CourseServiceProxy, EnrollmentCreateUpdateDto, EnrollmentServiceProxy, GetCourseDto, GetStudentDto, StudentServiceProxy } from '../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'app-edit-enrollment-dialog',
  templateUrl: './edit-enrollment-dialog.component.html',
  styleUrls: ['./edit-enrollment-dialog.component.css']
})
export class EditEnrollmentDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  courses: GetCourseDto[] = [];
  students: GetStudentDto  [] = [];
  enrollment: EnrollmentCreateUpdateDto = new EnrollmentCreateUpdateDto();
@Input() enrollments: any;
  @Output() onUpdate = new EventEmitter<void>();

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _courseService: CourseServiceProxy,
    private _studentService: StudentServiceProxy,
    private _enrollmentService: EnrollmentServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loadCourses();
    this.loadStudents();
    this.enrollment.courseIds = this.enrollment.courseIds || [];
  }

  loadCourses(): void {
    this._courseService.getAll('', undefined, 0, 1000).subscribe(result => {
      this.courses = result.items;
    });
  }

  loadStudents(): void {
    this._studentService.getAll('', undefined, 0, 1000).subscribe(result => {
      this.students = result.items;
    });
  }
  update(): void {
    this.saving = true;

    this._enrollmentService
      .update(this.enrollment)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.info(this.l('UpdatedSuccessfully'));
        this.bsModalRef.hide();
        this.onUpdate.emit();
      });
  }
}

