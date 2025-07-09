import { Component, OnInit, OnDestroy, ViewChild, inject } from '@angular/core';
import * as THREE from 'three';
import { GameScene } from '../../Engine/GameScene';
import { LocationService } from '../../Services/LocationService';
import { SignalRService } from '../../GameServer/SignalRService';

@Component({
  selector: 'app-planet',
  templateUrl: './planet.component.html',
  styleUrl: './planet.component.css'
})
export class PlanetComponent implements OnInit, OnDestroy {

  scene!: GameScene;
  renderHeigth!: number;
  renderWidth!: number;

  renderer!: THREE.WebGLRenderer;

  @ViewChild('scene_content') scene_content!: HTMLCanvasElement;

  private locationService = inject(LocationService);
  private signalRService = inject(SignalRService);

  ngOnInit() {

    this.renderHeigth = 200;
    this.renderWidth = 300;

    // Renderer
    const scene_content = (document.getElementById('scene_content') as HTMLCanvasElement);
    this.renderer = new THREE.WebGLRenderer({ antialias: true, canvas: scene_content });
    this.renderer.setSize(this.renderWidth, this.renderHeigth);

    // Scene
    this.scene = new GameScene(this.renderer, this.locationService, this.signalRService);

    // Click events
    scene_content.addEventListener('click', (event) => this.onMouseClick(event));

    // Rendering loop
    this.renderer.setAnimationLoop(() => this.animate());
  }

  ngOnDestroy(): void {
    this.scene.dispose();
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
}
