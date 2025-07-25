import { BasePropertyPanel } from "./BasePropertyPanel";
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-maplocation-properties',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="property-panel">
      <h3>Location properties</h3>
      
      <div class="property-section">
        <h4>Informations générales</h4>
        <div class="property-row">
          <label>Nom:</label>
          <input [(ngModel)]="data.name" (blur)="onPropertyChange('name', data.name)">
        </div>
        <div class="property-row">
          <label>Email:</label>
          <input [(ngModel)]="data.email" (blur)="onPropertyChange('email', data.email)">
        </div>
        <div class="property-row">
          <label>Rôle:</label>
          <select [(ngModel)]="data.role" (change)="onPropertyChange('role', data.role)">
            <option value="admin">Admin</option>
            <option value="user">User</option>
            <option value="guest">Guest</option>
          </select>
        </div>
      </div>

      <div class="property-row">
        <label>Position:</label>
        <div class="vector-input">
          <input type="number"
                 [(ngModel)]="data.position.latitude"
                 (change)="onPropertyChange('role', data.position.latitude)"
                 step="0.1">
          <input type="number"
                 [(ngModel)]="data.position.longitude"
                 (change)="onPropertyChange('role', data.position.longitude)"
                 step="0.1">
        </div>
      </div>

      <div class="property-section">
        <h4>Métadonnées</h4>
        <div class="property-row">
          <label>ID:</label>
          <span class="readonly">{{ data.id }}</span>
        </div>
        <div class="property-row">
          <label>Créé le:</label>
          <span class="readonly">{{ data.createdAt | date:'short' }}</span>
        </div>
      </div>

      <div class="property-actions">
        <button (click)="saveChanges()" [disabled]="!hasChanges">Sauvegarder</button>
        <button (click)="resetChanges()">Annuler</button>
      </div>
    </div>
  `,
  styles: [`
    .property-panel {
      padding: 16px;
      background: #f8f9fa;
      border-left: 1px solid #dee2e6;
    }
    .property-section {
      margin-bottom: 20px;
      padding: 12px;
      background: white;
      border-radius: 4px;
    }
    .property-row {
      display: flex;
      align-items: center;
      margin-bottom: 8px;
    }
    .property-row label {
      width: 80px;
      font-weight: bold;
      margin-right: 8px;
    }
    .property-row input, .property-row select {
      flex: 1;
      padding: 4px 8px;
      border: 1px solid #ddd;
      border-radius: 3px;
    }
    .readonly {
      color: #6c757d;
      font-style: italic;
    }
    .property-actions {
      margin-top: 16px;
    }
    .property-actions button {
      margin-right: 8px;
      padding: 8px 16px;
    }
  `]
})
export class MapLocationPropertiesComponent extends BasePropertyPanel implements OnInit {
  readonly panelType = 'MapLocation';
  @Output() propertyChanged = new EventEmitter<{ property: string, value: any }>();

  hasChanges = false;
  originalData: any;

  ngOnInit() {
    this.originalData = { ...this.data };
  }

  onPropertyChange(property: string, value: any) {
    this.hasChanges = true;
    this.propertyChanged.emit({ property, value });
  }

  saveChanges() {
    // Logique de sauvegarde
    console.log('Sauvegarde des modifications utilisateur:', this.data);
    this.hasChanges = false;
    this.originalData = { ...this.data };
  }

  resetChanges() {
    this.data = { ...this.originalData };
    this.hasChanges = false;
  }
}
