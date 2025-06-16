import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.StudentServiceProxy ,
        ApiServiceProxies.BedServiceProxy ,
        ApiServiceProxies.SubejctServiceProxy ,
        ApiServiceProxies.DealWithTasksServiceProxy ,
        ApiServiceProxies.BedStatusServiceProxy ,
        ApiServiceProxies.AdmissionsServiceProxy ,
        ApiServiceProxies.PatientServiceProxy ,
        ApiServiceProxies.TeacherSubjectServiceProxy ,
        ApiServiceProxies.EnrollmentServiceProxy ,
        ApiServiceProxies.TeacherServiceProxy,
        ApiServiceProxies.AddressServiceProxy,
        ApiServiceProxies.EmployeeServiceProxy,
        ApiServiceProxies.DepartmentServiceProxy,
        ApiServiceProxies.FileServiceProxy,
        ApiServiceProxies.CourseServiceProxy,
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
