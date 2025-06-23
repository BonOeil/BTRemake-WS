import { Component, OnInit, ViewChild } from '@angular/core';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls'

@Component({
  selector: 'app-planet',
  templateUrl: './planet.component.html',
  styleUrl: './planet.component.css'
})
export class PlanetComponent implements OnInit {

  scene!: THREE.Scene;
  renderHeigth!: number;
  renderWidth!: number;

  planetRadius!: number;

  planet!: THREE.Mesh;
  controls!: OrbitControls;
  renderer!: THREE.WebGLRenderer;
  camera!: THREE.PerspectiveCamera;

  @ViewChild('scene_content') scene_content!: HTMLCanvasElement;

  constructor() {
  }

  ngOnInit() {

    this.scene = new THREE.Scene();
    this.scene.add(new THREE.AxesHelper(5));

    this.renderHeigth = 200;
    this.renderWidth = 300;

    this.planetRadius = 300;

    const scene_content = (document.getElementById('scene_content') as HTMLCanvasElement);
    this.renderer = new THREE.WebGLRenderer({ antialias: true, canvas: scene_content });
    this.renderer.setSize(this.renderWidth, this.renderHeigth);

    this.camera = this.createCamera();
    this.planet = this.createPlanet();

    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.update();

    // Add light
    this.setupLighting();

    this.renderer.setAnimationLoop(() => this.animate());
  }

  createCamera(): THREE.PerspectiveCamera {
    const camera = new THREE.PerspectiveCamera(75, this.renderWidth / this.renderHeigth, 0.1, 1000);
    camera.position.z = this.planetRadius + this.planetRadius * 0.5;

    return camera;
  }

  createPlanet() : THREE.Mesh {
    const geometry = new THREE.SphereGeometry(this.planetRadius, 100, 100);
    const texture = new THREE.TextureLoader().load("assets/textures/earth-diffuse.jpg");
    texture.wrapS = THREE.RepeatWrapping;
    texture.wrapT = THREE.RepeatWrapping;
    const material = new THREE.MeshPhongMaterial({
      map: texture
    });

    const planet = new THREE.Mesh(geometry, material);
    this.scene.add(planet);

    return planet;
  }

  setupLighting() {
    // Ambiant lightning
    const ambientLight = new THREE.AmbientLight(0x404040, 0.3);
    this.scene.add(ambientLight);

    // Directionnal light
    const directionalLight = new THREE.DirectionalLight(0xffffff, 1.2);
    directionalLight.position.set(5, 3, 5);
    directionalLight.castShadow = true;

    // Shadows
    directionalLight.shadow.mapSize.width = 2048;
    directionalLight.shadow.mapSize.height = 2048;
    directionalLight.shadow.camera.near = 0.5;
    directionalLight.shadow.camera.far = 50;
    directionalLight.shadow.camera.left = -10;
    directionalLight.shadow.camera.right = 10;
    directionalLight.shadow.camera.top = 10;
    directionalLight.shadow.camera.bottom = -10;

    this.scene.add(directionalLight);

    // Point light
    const pointLight = new THREE.PointLight(0xffffff, 0.5, 100);
    pointLight.position.set(-5, -3, -5);
    this.scene.add(pointLight);
  }

  animate() {
    this.planet.rotation.y += 0.001;

    this.controls.update()

    this.renderer.render(this.scene, this.camera);
  }
}
