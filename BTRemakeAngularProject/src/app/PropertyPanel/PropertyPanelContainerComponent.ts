import { Component, OnInit, inject } from '@angular/core';
import { SelectionService } from '../../Services/SelectionService';
import { CommonModule } from '@angular/common';
import { MapLocationPropertiesComponent } from './maplocation-properties.component';
import { SelectionItem } from '../../Services/SelectionItem';
import { MapUnitPropertiesComponent } from './mapunit-properties.component';

@Component({
  selector: 'app-property-panel-container',
  standalone: true,
  imports: [CommonModule, MapLocationPropertiesComponent, MapUnitPropertiesComponent],
  template: `
    <div class="property-panel-container" *ngIf="selectedItem">
      <ng-container [ngSwitch]="selectedItem.type">
        <app-maplocation-properties
          *ngSwitchCase="'MapLocation'"
          [data]="selectedItem.data"
          (propertyChanged)="onPropertyChanged($event)">
        </app-maplocation-properties>

        <app-mapunit-properties
          *ngSwitchCase="'MapUnit'"
          [data]="selectedItem.data"
          (propertyChanged)="onPropertyChanged($event)">
        </app-mapunit-properties>
        
        <div *ngSwitchDefault class="no-panel">
          <h3>Type non supporté</h3>
          <p>Aucun panneau de propriétés disponible pour le type: {{ selectedItem.type }}</p>
        </div>
      </ng-container>
    </div>
    
    <div class="no-selection" *ngIf="!selectedItem">
      <h3>Aucune sélection</h3>
      <p>Sélectionnez un élément pour voir ses propriétés</p>
    </div>
  `,
  styles: [`
    .property-panel-container {
      width: 100%;
      height: 100%;
      overflow-y: auto;
    }
    .no-selection, .no-panel {
      padding: 32px;
      text-align: center;
      color: #6c757d;
    }
  `]
})
export class PropertyPanelContainerComponent implements OnInit {
  selectedItem: SelectionItem | null = null;

  private selectionService = inject(SelectionService);

  ngOnInit() {
    this.selectionService.selectedItem$.subscribe(item => {
      this.selectedItem = item;
    });
  }

  onPropertyChanged(event: { property: string, value: any }) {
    console.log('Propriété modifiée:', event);
    // Ici vous pouvez déclencher des actions globales comme la sauvegarde automatique
  }
}
