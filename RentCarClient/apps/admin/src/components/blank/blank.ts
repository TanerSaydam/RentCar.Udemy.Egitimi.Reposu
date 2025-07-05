import { DatePipe, Location, NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, input, output, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EntityModel } from '../../models/entity.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'blank',
  imports: [
    NgClass,
    RouterLink,
    DatePipe,
    FormsModule
  ],
  templateUrl: './blank.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Blank {
  readonly pageIcon = input.required<string>();
  readonly pageTitle = input.required<string>();
  readonly pageDescription = input<string>('');
  readonly status = input<boolean>(true);
  readonly showStatus = input<boolean>(false);
  readonly showStatusCheckbox = input<boolean>(true);
  readonly showBackBtn = input<boolean>(true);
  readonly showEditBtn = input<boolean>(false);
  readonly editBtnUrl = input<string>("");
  readonly audit = input<EntityModel>();
  readonly showAduit = input<boolean>(false);

  readonly changeStatusEvent = output<boolean>();

  readonly #location = inject(Location);

  back(){
    this.#location.back();
  }

  changeStatus(event:any){
    const checked = event.target.checked;
    this.changeStatusEvent.emit(checked);
  }
}
