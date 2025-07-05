import { NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { RoleModel, initialRole } from 'apps/admin/src/models/role.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass,
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Create {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Roller',
      icon: 'bi-buildings',
      url: '/roles'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Rol Güncelle' : 'Rol Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(this.#http.getResource<RoleModel>(`/rent/roles/${this.id()}`));

      this.bredcrumbs.update(prev => [...prev, {
          title: res.data!.name,
          icon: 'bi-pen',
          url: `/roles/edit/${this.id()}`,
          isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? {...initialRole});
  readonly loading = linkedSignal(() => this.result.isLoading());

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
          url: '/roles/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    })
  }

  save(form: NgForm){
    if(!form.valid) return;

    if(!this.id()){
      this.loading.set(true);
      this.#http.post<string>('/rent/roles', this.data(), (res) => {
        this.#toast.showToast("Başarılı",res,"success");
        this.#router.navigateByUrl("/roles");
        this.loading.set(false);
      },() => this.loading.set(false));
    }else{
      this.loading.set(true);
      this.#http.put<string>('/rent/roles', this.data(), (res) => {
        this.#toast.showToast("Başarılı",res,"info");
        this.#router.navigateByUrl("/roles");
        this.loading.set(false);
      },() => this.loading.set(false));
    }
  }

  changeStatus(status:boolean){
    this.data.update(prev => ({
      ...prev,
      isActive: status
    }));
  }
}
