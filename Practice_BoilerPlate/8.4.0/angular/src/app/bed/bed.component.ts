import { Component, Injector } from "@angular/core";
import { finalize } from "rxjs/operators";
import { BsModalService } from "ngx-bootstrap/modal";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "shared/paged-listing-component-base";
import {
  GetBedDto,
  BedServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { CreateBedDialogComponent } from "./create-bed/create-bed-dialog.component";
import { EditBedDialogComponent } from "./edit-bed/edit-bed-dialog.component";

// ✅ Chart.js support
import { ChartType } from "chart.js";
import { ChartOptions } from "chart.js";

class PagedBedRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
  sorting: string;
}

@Component({
  selector: "app-bed",
  templateUrl: "./bed.component.html",
  styleUrls: ["./bed.component.css"],
  animations: [appModuleAnimation()],
})
export class BedComponent extends PagedListingComponentBase<GetBedDto> {
  beds: GetBedDto[] = [];
  keyword = "";
  isActive: boolean | null = null;
  advancedFiltersVisible = false;
  sorting = "bedNumber asc";

  //  Chart data
  public bedStatusChartLabels: string[] = ["Available", "Occupied", "Blocked"];
  public bedStatusChartData: any[] = [
    { data: [0, 0, 0], backgroundColor: ["#36A2EB", "#FF6384", "#FFCE56"] },
  ];
  public bedStatusChartType: "pie" | "bar" | "line" = "pie";
  public bedStatusChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
  };

  // Tab control
  activeTab: string = "list";

  //type
  public bedTypeChartLabels: string[] = ["ICU", "General", "SemiICU"];
  public bedTypeChartData: any[] = [
    { data: [0, 0, 0], backgroundColor: ["#36A2EB", "#FF6384", "#FFCE56"] },
  ];
  public bedTypeChartType: "pie" | "bar" | "line" = "pie";
  public bedTypeChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
  };



  constructor(
    injector: Injector,
    private _bedService: BedServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  createBed(): void {
    const createDialog = this._modalService.show(CreateBedDialogComponent, {
      class: "modal-lg",
    });
    (createDialog.content as CreateBedDialogComponent).onSave.subscribe(() => {
      this.refresh();
    });
  }

  editBed(bed: GetBedDto): void {
    const editDialog = this._modalService.show(EditBedDialogComponent, {
      class: "modal-lg",
      initialState: { bed: Object.assign({}, bed) },
    });
    (editDialog.content as EditBedDialogComponent).onUpdate.subscribe(() => {
      this.refresh();
    });
  }

  clearFilters(): void {
    this.keyword = "";
    this.isActive = null;
    this.getDataPage(1);
  }

  changeSorting(column: string): void {
    const [currentColumn, currentDirection] = this.sorting.split(" ");
    if (currentColumn === column) {
      this.sorting = `${column} ${currentDirection === "asc" ? "desc" : "asc"}`;
    } else {
      this.sorting = `${column} asc`;
    }
    this.getDataPage(1);
  }

  refresh(): void {
    this.getDataPage(this.pageNumber);
  }

  protected list(
    request: PagedBedRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = this.isActive;
    request.sorting = this.sorting;

    this._bedService
      .getAll(request.keyword, request.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => finishedCallback()))
      .subscribe((result) => {
        this.beds = result.items;
        this.showPaging(result, pageNumber);
        this.updateChart();
      });
  }

  
  protected delete(bed: GetBedDto): void {
    abp.message.confirm(
      this.l("DeleteWarningMessage", bed.id),
      undefined,
      (result: boolean) => {
        if (result) {
          this._bedService.delete(bed.id).subscribe(() => {
            abp.notify.success(this.l("SuccessfullyDeleted"));
            this.refresh();
          });
        }
      }
    );
  }

  updateChart(): void {
    const statusCounts = { available: 0, occupied: 0, blocked: 0 };
    const typeCounts = { icu: 0, general: 0, semiIcu: 0 };

    this.beds.forEach((bed) => {
      if (bed.status === 0) statusCounts.available++;
      else if (bed.status === 1) statusCounts.occupied++;
      else if (bed.status === 2) statusCounts.blocked++;
    });
    this.beds.forEach((bed) => {
      if (bed.type === 0) typeCounts.icu++;
      else if (bed.type === 1) typeCounts.general++;
      else if (bed.type === 2) typeCounts.semiIcu++;
    });

    this.bedStatusChartData = [
      {
        data: [
          statusCounts.available,
          statusCounts.occupied,
          statusCounts.blocked,
        ],
        backgroundColor: ["#36A2EB", "#FF6384", "#FFCE56"],
      },
    ];
    this.bedTypeChartData = [
      {
        data: [typeCounts.icu, typeCounts.general, typeCounts.semiIcu],
        backgroundColor: ["#36A2EB", "#FF6384", "#FFCE56"],
      },
    ];
  }

  getTypeString(type: number): string {
    switch (type) {
      case 0:
        return "General";
      case 1:
        return "ICU";
      case 2:
        return "SemiICU";
      case 3:
        return "Isolation";
      default:
        return "Unknown";
    }
  }

  gettypeString(status: number): string {
    switch (status) {
      case 0:
        return "Available";
      case 1:
        return "Occupied";
      case 2:
        return "Blocked";
      default:
        return "Unknown";
    }
  }
}
