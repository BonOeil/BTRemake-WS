import { Component, Input } from '@angular/core';

@Component({
  template: ''
})
export abstract class BasePropertyPanel {
  @Input() data: any;
  abstract readonly panelType: string;
}
