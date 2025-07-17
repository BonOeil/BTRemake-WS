import { Component, OnInit, OnDestroy, ViewChild, inject } from '@angular/core';
import * as THREE from 'three';
import { GameScene } from '../../Engine/GameScene';
import { LocationService } from '../../Services/LocationService';
import { SignalRService } from '../../GameServer/SignalRService';
import { MapUnitsService } from '../../Services/MapUnitsService';

@Component({
  selector: 'app-planet',
  templateUrl: './planet.component.html',
  styleUrl: './planet.component.css'
})
export class PlanetComponent implements OnInit, OnDestroy {

  public scene!: GameScene;

  renderer!: THREE.WebGLRenderer;

  @ViewChild('scene_content') scene_content!: HTMLCanvasElement;

  private locationService = inject(LocationService);
  private mapUnitsService = inject(MapUnitsService);
  private signalRService = inject(SignalRService);

  ngOnInit() {
    // Renderer
    this.scene_content = (document.getElementById('scene_content') as HTMLCanvasElement);
    this.renderer = new THREE.WebGLRenderer({ antialias: true, canvas: this.scene_content });

    // Scene
    this.scene = new GameScene(this.renderer, this.locationService, this.mapUnitsService, this.signalRService);

    this.resize();
    window.addEventListener('resize', () => this.resize());

    // Click events
    this.scene_content.addEventListener('click', (event) => this.onMouseClick(event));

    // Rendering loop
    this.renderer.setAnimationLoop(() => this.animate());
  }

  ngOnDestroy(): void {
    this.scene.dispose();
  }

  private resize() {
    const conteneur = document.getElementById('scene-container');

    const rect = conteneur?.getBoundingClientRect() ?? new DOMRect(200, 150);
    this.scene_content.width = rect.width;
    this.scene_content.height = rect.height;

    this.scene.camera.aspect = rect.width / rect.height;
    this.scene.camera.updateProjectionMatrix();

    this.renderer.setSize(rect.width, rect.height);
  }

  private onMouseClick(event: MouseEvent) {
    const rect = this.renderer.domElement.getBoundingClientRect();

    // Mouse coordinates
    const mouse = new THREE.Vector2();
    mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
    mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

    // Raycast
    const raycaster = new THREE.Raycaster();
    raycaster.setFromCamera(mouse, this.scene.camera);

    // Get intecsection objects
    const intersects = raycaster.intersectObjects(this.scene.scene.children, true);

    // Act on scene
    this.scene.clickEvent(intersects);
  }

  animate() {
    this.scene.animate();

    this.renderer.render(this.scene.scene, this.scene.camera);
  }

  updateObjectPosition() {
    console.log('Position mise à jour:', this.scene.selectedObject?.position);
  }
}
