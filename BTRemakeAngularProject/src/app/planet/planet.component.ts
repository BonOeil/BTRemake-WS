import { Component, OnInit, ViewChild, inject } from '@angular/core';
import * as THREE from 'three';
import { GameScene } from '../../Engine/GameScene';
import { LocationService } from '../../Services/LocationService';

@Component({
  selector: 'app-planet',
  templateUrl: './planet.component.html',
  styleUrl: './planet.component.css'
})
export class PlanetComponent implements OnInit {

  scene!: GameScene;
  renderHeigth!: number;
  renderWidth!: number;

  renderer!: THREE.WebGLRenderer;

  @ViewChild('scene_content') scene_content!: HTMLCanvasElement;

  private locationService = inject(LocationService);

  ngOnInit() {

    this.renderHeigth = 200;
    this.renderWidth = 300;

    const scene_content = (document.getElementById('scene_content') as HTMLCanvasElement);
    this.renderer = new THREE.WebGLRenderer({ antialias: true, canvas: scene_content });
    this.renderer.setSize(this.renderWidth, this.renderHeigth);

    this.scene = new GameScene(this.renderer, this.locationService);

    this.renderer.setAnimationLoop(() => this.animate());
  }

  animate() {
    this.scene.animate();

    this.renderer.render(this.scene.scene, this.scene.camera);
  }
}
