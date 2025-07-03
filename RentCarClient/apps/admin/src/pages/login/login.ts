import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Result } from '../../models/result.model';
import { Router } from '@angular/router';
import { FormValidateDirective } from 'form-validate-angular';

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
  readonly #http = inject(HttpClient);
  readonly #router = inject(Router);

  login(form: NgForm) {
    if (!form.valid) return;

    this.#http.post<Result<string>>("/rent/auth/login", form.value).subscribe(res => {
      localStorage.setItem("response",res.data!);
      this.#router.navigateByUrl("/");
    });
  }
}
