import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgClass } from '@angular/common';
import Blank from 'apps/admin/src/components/blank/blank';
import { ProtectionPackageModel, initialProtectionPackageModel } from '@shared/lib/models/protection-package.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { HttpService } from '@shared/lib/services/http';
import { lastValueFrom } from 'rxjs';
import { NgxMaskDirective } from 'ngx-mask';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass,
    NgxMaskDirective
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class CreateProtectionPackage {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Güvence Paketleri',
      icon: 'bi-shield-check',
      url: '/protection-packages'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Paket Güncelle' : 'Paket Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      const res = await lastValueFrom(this.#http.getResource<ProtectionPackageModel>(`/rent/protection-packages/${this.id()}`));
      this.bredcrumbs.update(prev => [...prev, {
        title: res.data!.name,
        icon: 'bi-pen',
        url: `/protection-packages/edit/${this.id()}`,
        isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? { ...initialProtectionPackageModel });
  readonly loading = linkedSignal(() => this.result.isLoading());
  coveragesInput = '';

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/protection-packages/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }

  save(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);
    if (!this.id()) {
      this.#http.post<string>('/rent/protection-packages', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "success");
        this.#router.navigateByUrl("/protection-packages");
        this.loading.set(false);
      }, () => this.loading.set(false));
    } else {
      this.#http.put<string>('/rent/protection-packages', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "info");
        this.#router.navigateByUrl("/protection-packages");
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

  addCoverage() {
    if (this.coveragesInput.trim()) {
      this.data.update(prev => ({
        ...prev,
        coverages: [...prev.coverages, this.coveragesInput.trim()]
      }));
      this.coveragesInput = '';
    }
  }

  removeCoverage(index: number) {
    this.data.update(prev => ({
      ...prev,
      coverages: prev.coverages.filter((_, i) => i !== index)
    }));
  }
}