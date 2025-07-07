import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { CustomerModel, initialCustomerModel } from 'apps/admin/src/models/customer.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgxMaskDirective
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DatePipe]
})
export default class CreateCustomer {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Müşteriler',
      icon: 'bi-people',
      url: '/customers'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Müşteri Güncelle' : 'Müşteri Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      const res = await lastValueFrom(this.#http.getResource<CustomerModel>(`/rent/customers/${this.id()}`));
      this.bredcrumbs.update(prev => [...prev, {
        title: res.data!.fullName,
        icon: 'bi-pen',
        url: `/customers/edit/${this.id()}`,
        isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? { ...initialCustomerModel });
  readonly loading = linkedSignal(() => this.result.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #date = inject(DatePipe);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/customers/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
        const date = this.#date.transform("01.01.2000", "yyyy-MM-dd")!;
        this.data.update(prev => ({...prev, dateOfBirth: date, drivingLicenseIssuanceDate: date}));
      }
    });
  }

  save(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);
    if (!this.id()) {
      this.#http.post<string>('/rent/customers', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "success");
        this.#router.navigateByUrl("/customers");
        this.loading.set(false);
      }, () => this.loading.set(false));
    } else {
      this.#http.put<string>('/rent/customers', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "info");
        this.#router.navigateByUrl("/customers");
        this.loading.set(false);
      }, () => this.loading.set(false));
    }
  }

  changeStatus(status: boolean) {
    this.data.update(prev => ({
      ...prev,
      isActive: status
    }));
  }
}