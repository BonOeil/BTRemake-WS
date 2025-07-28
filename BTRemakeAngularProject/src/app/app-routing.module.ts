import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PlanetComponent } from './planet/planet.component';
import { PlaneManagement } from './PlaneManagement/planeManagement.component';

const routes: Routes = [
  { path: 'planet', component: PlanetComponent },
  { path: 'planes', component: PlaneManagement },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
