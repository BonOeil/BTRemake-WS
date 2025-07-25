import { BasePropertyPanel } from "./BasePropertyPanel";
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-maplocation-properties',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './maplocation-properties.component.html',
  styleUrl: './maplocation-properties.component.css'
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
