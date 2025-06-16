import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { HomeComponent } from "./home/home.component";
import { AboutComponent } from "./about/about.component";
import { UsersComponent } from "./users/users.component";
import { TenantsComponent } from "./tenants/tenants.component";
import { RolesComponent } from "app/roles/roles.component";
import { ChangePasswordComponent } from "./users/change-password/change-password.component";
import { StudentComponent } from "./student/student.component";
import { DepartmentComponent } from "./department/department.component";
import { EmployeeComponent } from "./employee/employee.component";
import { CourseComponent } from "./course/course.component";
import { TeacherComponent } from "./teacher/teacher.component";
import { AddressComponent } from "./address.component";
import { TeachersubjectComponent } from "./teachersubject/teachersubject.component";
import { SubjectComponent } from "./subject/subject.component";
import { EnrollmentComponent } from "./enrollment/enrollment.component";
import { FileuploadComponent } from "./fileupload/fileupload.component";
import { BedComponent } from "./bed/bed.component";
import { PatientComponent } from "./patient/patient.component";
import { AdmissionComponent } from "./admission/admission.component";
import { DealsServiceComponent } from "./deals-service/deals-service.component";

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "",
        component: AppComponent,
        children: [
          {
            path: "home",
            component: HomeComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "users",
            component: UsersComponent,
            data: { permission: "Pages.Users" },
            canActivate: [AppRouteGuard],
          },
          {
            path: "roles",
            component: RolesComponent,
            data: { permission: "Pages.Roles" },
            canActivate: [AppRouteGuard],
          },
          {
            path: "tenants",
            component: TenantsComponent,
            data: { permission: "Pages.Tenants" },
            canActivate: [AppRouteGuard],
          },
          {
            path: "about",
            component: AboutComponent,
            canActivate: [AppRouteGuard],
          },
          { path: "student", component: StudentComponent },
          { path: "department", component: DepartmentComponent },
          { path: "employee", component: EmployeeComponent },
          { path: "course", component: CourseComponent },
          { path: "teacher", component: TeacherComponent },
          { path: "address", component: AddressComponent },
          { path: "teachersubject", component: TeachersubjectComponent },
          { path: "subject", component: SubjectComponent },
          { path: "enrollment", component: EnrollmentComponent },
          { path: "fileupload", component: FileuploadComponent },
          { path: "bed", component: BedComponent },
          { path: "patient", component: PatientComponent },
          { path: "admission", component: AdmissionComponent },
          { path: "deals-service", component: DealsServiceComponent },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
