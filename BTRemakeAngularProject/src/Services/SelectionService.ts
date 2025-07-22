import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SelectionItem } from './SelectionItem';

@Injectable({
  providedIn: 'root'
})
export class SelectionService {
  private selectedItemSubject = new BehaviorSubject<SelectionItem | null>(null);
  public selectedItem$: Observable<SelectionItem | null> = this.selectedItemSubject.asObservable();

  selectItem(item: SelectionItem) {
    this.selectedItemSubject.next(item);
  }

  clearSelection() {
    this.selectedItemSubject.next(null);
  }

  getCurrentSelection(): SelectionItem | null {
    return this.selectedItemSubject.value;
  }
}
