import { DatePipe, NgClass, NgTemplateOutlet } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, effect, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { BranchModel } from 'apps/admin/src/models/branch.model';
import { CustomerModel, initialCustomerModel } from 'apps/admin/src/models/customer.model';
import { ODataModel } from 'apps/admin/src/models/odata.model';
import { initialReservation, ReservationModel } from 'apps/admin/src/models/reservation.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { Common } from 'apps/admin/src/services/common';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { FlexiPopupModule } from 'flexi-popup';
import { FlexiSelectModule } from 'flexi-select';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgxMaskDirective,
    NgxMaskPipe,
    NgClass,
    FlexiPopupModule,
    FlexiGridModule,
    NgxMaskPipe,
    NgTemplateOutlet,
    FlexiSelectModule,
    DatePipe
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DatePipe]
})
export default class Create {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Rezervasyonlar',
      icon: 'bi-calendar-check',
      url: '/reservations'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Rezervasyon Güncelle' : 'Rezervasyon Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      if (!this.id()) return null;
      const res = await lastValueFrom(this.#http.getResource<ReservationModel>(`/rent/reservations/${this.id()}`));
      this.bredcrumbs.update(prev => [...prev, {
        title: res.data!.customer.fullName,
        icon: 'bi-pen',
        url: `/reservation/edit/${this.id()}`,
        isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? { ...initialReservation });
  readonly loading = linkedSignal(() => this.result.isLoading());
  isCustomerPopupVisible = false;
  readonly isCustomerPopupLoading = signal<boolean>(false);
  readonly customerPopupData = signal<CustomerModel>({ ...initialCustomerModel });
  readonly customerState = signal<StateModel>(new StateModel());
  readonly customersResult = httpResource<ODataModel<CustomerModel>>(() => {
    let endpoint = '/rent/odata/customers?count=true&';
    const part = this.#grid.getODataEndpoint(this.customerState());
    endpoint += part;
    return endpoint;
  });
  readonly customersData = computed(() => this.customersResult.value()?.value ?? []);
  readonly customersTotal = computed(() => this.customersResult.value()?.['@odata.count'] ?? 0);
  readonly customersLoading = computed(() => this.customersResult.isLoading());
  readonly selectedCustomer = signal<CustomerModel | undefined>(undefined);
  readonly branchesResult = httpResource<ODataModel<BranchModel>>(() => '/rent/odata/branches');
  readonly branchesData = computed(() => this.branchesResult.value()?.value ?? []);
  readonly branchesLoading = computed(() => this.branchesResult.isLoading());
  readonly isAdmin = computed(() => this.#common.decode().role === 'sys_admin');
  readonly timeData = signal<string[]>(
    Array.from({ length: 31 }, (_, i) => {
      const hour = 9 + Math.floor(i / 2);
      const minute = i % 2 === 0 ? "00" : "30";
      return `${hour.toString().padStart(2, "0")}:${minute}`;
    })
  );
  readonly branchName = linkedSignal(() => this.#common.decode().branch);

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #date = inject(DatePipe);
  readonly #grid = inject(FlexiGridService);
  readonly #common = inject(Common);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/reservation/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
        const date = this.#date.transform("01.01.2000", "yyyy-MM-dd")!;
        this.customerPopupData.update(prev => ({ ...prev, dateOfBirth: date, drivingLicenseIssuanceDate: date }));
        const now = this.#date.transform(new Date(), "yyyy-MM-dd")!;
        const tomorrowDate = new Date();
        tomorrowDate.setDate(tomorrowDate.getDate() + 1);
        const tomorrow = this.#date.transform(tomorrowDate, "yyyy-MM-dd")!;

        this.data.update(prev => ({
          ...prev,
          pickUpDate: now,
          deliveryDate: tomorrow
        }));

        this.calculateDayDifference();
      }
    });
  }

  save(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);
    if (!this.id()) {
      this.#http.post<string>('/rent/reservations', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "success");
        this.#router.navigateByUrl("/reservation");
        this.loading.set(false);
      }, () => this.loading.set(false));
    } else {
      this.#http.put<string>('/rent/reservations', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "info");
        this.#router.navigateByUrl("/reservation");
        this.loading.set(false);
      }, () => this.loading.set(false));
    }
  }

  saveCustomer(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);
    this.#http.post<string>('/rent/customers', this.customerPopupData(), res => {
      this.#toast.showToast("Başarılı", res, "success");
      this.loading.set(false);
    }, () => this.loading.set(false));
  }

  customerDataStateChange(state: StateModel) {
    this.customerState.set(state);
  }

  selectCustomer(item: CustomerModel) {
    this.selectedCustomer.set(item);
    this.data.update(prev => ({ ...prev, customerId: item.id }));
  }

  clearCustomer() {
    this.selectedCustomer.set(undefined);
    this.data.update(prev => ({ ...prev, customerId: '' }));
  }

  calculateDayDifference() {
    const pickUpDateTime = new Date(`${this.data().pickUpDate}T${this.data().pickUpTime}`);
    const deliveryDateTime = new Date(`${this.data().deliveryDate}T${this.data().deliveryTime}`);

    const diffMs = deliveryDateTime.getTime() - pickUpDateTime.getTime();

    if (diffMs <= 0){
      this.data.update(prev => ({...prev, totalDay: 0}));
      return;
    }

    const oneDayMs = 24 * 60 * 60 * 1000;

    const fullDays = Math.floor(diffMs / oneDayMs);
    const remainder = diffMs % oneDayMs;

    const totalDay = remainder > 0 ? fullDays + 1 : fullDays;
    this.data.update(prev => ({...prev, totalDay: totalDay}));
  }

  setLocation(id:any){
    const branch = this.branchesData().find(i => i.id == id)!;
    this.branchName.set(branch.name);
  }
}
