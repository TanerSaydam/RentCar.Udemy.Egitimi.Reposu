import { Component, ViewEncapsulation, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  templateUrl: './unavailable.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink
  ]
})
export default class UnavailablePage {}