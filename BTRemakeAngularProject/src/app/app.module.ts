import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PlanetComponent } from './planet/planet.component';
import { PropertyPanelContainerComponent } from './PropertyPanel/PropertyPanelContainerComponent';
import { PlaneManagement } from './PlaneManagement/planeManagement.component';

@NgModule({
  declarations: [
    AppComponent,
    PlanetComponent,
    PlaneManagement
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    PropertyPanelContainerComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
