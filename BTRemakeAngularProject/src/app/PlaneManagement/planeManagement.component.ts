import { Component, OnInit, inject } from '@angular/core';
import { PlanesService } from '../../Services/PlanesService';
import { Plane } from '../../Models/Plane';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-planet',
  templateUrl: './planeManagement.component.html',
  styleUrl: './planeManagement.component.css'
})
export class PlaneManagement implements OnInit {
  private planesService = inject(PlanesService);
  private fb = inject(FormBuilder);
  editForm: FormGroup;
  isEditing = false;
  isAddingNew = false;
  loading = false;
  error: string | null = null;

  data: Plane[] = [];
  selectedItem: Plane | null = null;

  constructor(
  ) {
    this.editForm = this.fb.group({
      id: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.minLength(2)]],
      maxSpeed: ['', [Validators.min(0)]],
      maxAltitude: ['', [Validators.min(0)]],
    });
  }

  ngOnInit() {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.planesService.getPlanes().subscribe({
      next: (data) => {
        this.data = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Erreur lors du chargement des données';
        this.loading = false;
        console.error('Erreur:', error);
      }
    });
  }

  selectItem(item: Plane): void {
    this.selectedItem = item;
    this.isEditing = true;
    this.isAddingNew = false;

    // Pré-remplir le formulaire avec les données existantes
    this.editForm.patchValue({
      id: item.id,
      name: item.name,
      maxSpeed: item.maxSpeed,
      maxAltitude: item.maxAltitude,
    });
  }

  // Préparer l'ajout d'une nouvelle ligne
  addNewItem(): void {
    this.selectedItem = null;
    this.isEditing = true;
    this.isAddingNew = true;

    // Réinitialiser le formulaire
    this.editForm.reset();
    this.editForm.patchValue({
      name: "NO_NAME",
      maxSpeed: 300,
      maxAltitude: 5000,
    });
  }

  // Sauvegarder les modifications
  saveItem(): void {
    if (this.editForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    const formData = this.editForm.value;
    this.loading = true;

    if (this.isAddingNew) {
      // Créer un nouvel élément
      this.planesService.createPlane(formData).subscribe({
        next: (newItem) => {
          this.data.push(newItem);
          this.cancelEdit();
          this.loading = false;
        },
        error: (error) => {
          this.error = 'Erreur lors de la création';
          this.loading = false;
          console.error('Erreur:', error);
        }
      });
    } else if (this.selectedItem?.id) {
      // Mettre à jour l'élément existant
      this.planesService.updatePlane(formData).subscribe({
        next: (updatedItem) => {
          const index = this.data.findIndex(item => item.id === this.selectedItem?.id);
          if (index !== -1) {
            this.data[index] = updatedItem;
          }
          this.cancelEdit();
          this.loading = false;
        },
        error: (error) => {
          this.error = 'Erreur lors de la mise à jour';
          this.loading = false;
          console.error('Erreur:', error);
        }
      });
    }
  }

  // Annuler l'édition
  cancelEdit(): void {
    this.isEditing = false;
    this.isAddingNew = false;
    this.selectedItem = null;
    this.editForm.reset();
    this.error = null;
  }

  // Supprimer un élément
  deleteItem(item: Plane): void {
    if (!item.id || !confirm('Êtes-vous sûr de vouloir supprimer cet élément ?')) {
      return;
    }

    this.loading = true;
    this.planesService.deletePlane(item.id).subscribe({
      next: () => {
        this.data = this.data.filter(d => d.id !== item.id);
        if (this.selectedItem?.id === item.id) {
          this.cancelEdit();
        }
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Erreur lors de la suppression';
        this.loading = false;
        console.error('Erreur:', error);
      }
    });
  }

  // Marquer tous les champs comme touchés pour afficher les erreurs
  private markFormGroupTouched(): void {
    Object.keys(this.editForm.controls).forEach(key => {
      this.editForm.get(key)?.markAsTouched();
    });
  }

  // Vérifier si un champ a une erreur
  hasError(fieldName: string): boolean {
    const field = this.editForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  // Obtenir le message d'erreur pour un champ
  getErrorMessage(fieldName: string): string {
    const field = this.editForm.get(fieldName);
    if (!field || !field.errors || !field.touched) return '';

    if (field.errors['required']) return `${fieldName} est requis`;
    if (field.errors['minlength']) return `${fieldName} trop court`;

    return 'Erreur de validation';
  }
}
