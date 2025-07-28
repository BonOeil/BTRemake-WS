import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaneManagement } from './planeManagement.component';

describe('PlanetComponent', () => {
  let component: PlaneManagement;
  let fixture: ComponentFixture<PlaneManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlaneManagement]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PlaneManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
