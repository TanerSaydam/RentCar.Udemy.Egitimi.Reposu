import { ChangeDetectionStrategy, Component, ElementRef, inject, signal, viewChild, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { FormValidateDirective } from 'form-validate-angular';
import { FlexiToastService } from 'flexi-toast';
import { HttpService } from 'apps/admin/src/services/http';

@Component({
  imports: [
    FormsModule,
    FormValidateDirective
  ],
  templateUrl: './login.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Login {
  readonly loading = signal<boolean>(false);
  readonly email = signal<string>("");

  readonly closeBtn = viewChild<ElementRef<HTMLButtonElement>>("modalCloseBtn");

  readonly #http = inject(HttpService);
  readonly #router = inject(Router);
  readonly #toast = inject(FlexiToastService);

  login(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);
    this.#http.post<string>("/rent/auth/login", form.value, (res) => {
        localStorage.setItem("response", res);
        this.#router.navigateByUrl("/");
        this.loading.set(false);
    }, () => this.loading.set(false));
  }

  forgotPassword(){
    this.#http.post<string>(`/rent/auth/forgot-password/${this.email()}`,{}, (res) => {
      this.#toast.showToast("Başarılı",res, "info");
      this.closeBtn()!.nativeElement.click();
    });
  }
}
