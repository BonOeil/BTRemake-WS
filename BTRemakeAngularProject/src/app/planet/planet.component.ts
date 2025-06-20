import { Component } from '@angular/core';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls'

@Component({
  selector: 'app-planet',
  templateUrl: './planet.component.html',
  styleUrl: './planet.component.css'
})
export class PlanetComponent {
}

const planetRadius = 100;

const scene = new THREE.Scene();
scene.add(new THREE.AxesHelper(5))

const renderer = new THREE.WebGLRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

const camera = createCamera();
const planet = createPlanet();

const controls = new OrbitControls(camera, renderer.domElement)
controls.update()

// Add light
setupLighting();


function createCamera() {
  const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
  camera.position.z = planetRadius + planetRadius * 0.5;

  return camera;
}

function createPlanet() {
  const geometry = new THREE.SphereGeometry(planetRadius, 100, 100);
  const texture = new THREE.TextureLoader().load("assets/textures/earth-diffuse.jpg");
  texture.wrapS = THREE.RepeatWrapping;
  texture.wrapT = THREE.RepeatWrapping;
  const material = new THREE.MeshPhongMaterial({
    map: texture
  });

  const planet = new THREE.Mesh(geometry, material);

  scene.add(planet);

  return planet;
}

function setupLighting() {
  // Lumière ambiante faible
  const ambientLight = new THREE.AmbientLight(0x404040, 0.3);
  scene.add(ambientLight);

  // Lumière directionnelle (soleil)
  const directionalLight = new THREE.DirectionalLight(0xffffff, 1.2);
  directionalLight.position.set(5, 3, 5);
  directionalLight.castShadow = true;

  // Configuration des ombres
  directionalLight.shadow.mapSize.width = 2048;
  directionalLight.shadow.mapSize.height = 2048;
  directionalLight.shadow.camera.near = 0.5;
  directionalLight.shadow.camera.far = 50;
  directionalLight.shadow.camera.left = -10;
  directionalLight.shadow.camera.right = 10;
  directionalLight.shadow.camera.top = 10;
  directionalLight.shadow.camera.bottom = -10;

  scene.add(directionalLight);

  // Lumière ponctuelle pour les reflets
  const pointLight = new THREE.PointLight(0xffffff, 0.5, 100);
  pointLight.position.set(-5, -3, -5);
  scene.add(pointLight);
}

function animate() {
  requestAnimationFrame(animate);

  planet.rotation.y += 0.001;

  controls.update()

  renderer.render(scene, camera);
}

animate();
