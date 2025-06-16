import { Component, Injector, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';

import { finalize } from 'rxjs/operators';
import { CourseServiceProxy, EnrollmentCreateUpdateDto, EnrollmentServiceProxy, GetCourseDto, GetStudentDto, StudentServiceProxy } from '../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'app-create-enrollment-dialog',
  templateUrl: './create-enrollment-dialog.component.html',
  styleUrls: ['./create-enrollment-dialog.component.css'],
  
})
export class CreateEnrollmentDialogComponent  extends AppComponentBase implements OnInit {
  saving = false;
  courses: GetCourseDto[] = [];
  students: GetStudentDto[] = [];
  enrollment: EnrollmentCreateUpdateDto = new EnrollmentCreateUpdateDto();



  @Output() onSave = new EventEmitter<void>();

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
    this.enrollment.courseIds = [];
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

  save(): void {
    this.saving = true;

    this._enrollmentService
      .create(this.enrollment)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      });
  }
}

