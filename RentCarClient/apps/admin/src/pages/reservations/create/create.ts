import { DatePipe, NgClass, NgTemplateOutlet } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { CustomerModel, initialCustomerModel } from 'apps/admin/src/models/customer.model';
import { ODataModel } from 'apps/admin/src/models/odata.model';
import { initialReservation, ReservationModel } from 'apps/admin/src/models/reservation.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { FlexiPopupModule } from 'flexi-popup';
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
    NgTemplateOutlet
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
  readonly customerPopupData = signal<CustomerModel>({...initialCustomerModel});
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

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #date = inject(DatePipe);
  readonly #grid = inject(FlexiGridService);

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
        this.customerPopupData.update(prev => ({...prev, dateOfBirth: date, drivingLicenseIssuanceDate: date}));
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

  customerDataStateChange(state:StateModel){
    this.customerState.set(state);
  }

  selectCustomer(item:CustomerModel){
    this.selectedCustomer.set(item);
    this.data.update(prev => ({...prev, customerId: item.id}));
  }

  clearCustomer(){
    this.selectedCustomer.set(undefined);
    this.data.update(prev => ({...prev, customerId: ''}));
  }
}
