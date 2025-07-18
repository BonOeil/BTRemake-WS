import * as THREE from 'three';
import { OrbitControls } from 'three-orbitcontrols-ts';
import { CoordinateConverter } from '../Business/CoordinateConverter';
import { MapLocation } from '../Models/MapLocation';
import { LocationService } from '../Services/LocationService';
import { Subject, Subscription } from 'rxjs';
import { SignalRService } from '../GameServer/SignalRService';
import { MapUnitsService } from '../Services/MapUnitsService';
import { MapUnit } from '../Models/MapUnit';
import { GPSPosition } from '../Models/GPSPosition';

export interface ObjectProperties {
  id: string;
  name: string;
  position: THREE.Vector3;
  rotation: THREE.Euler;
  scale: THREE.Vector3;
  material?: any;
  geometry?: any;
}

export class GameScene {

  scene!: THREE.Scene;

  earth!: THREE.Mesh;
  controls!: OrbitControls;
  renderer!: THREE.WebGLRenderer;
  camera!: THREE.PerspectiveCamera;

  private selectedObjectSubject = new Subject<object | null>();
  public selectedObject$ = this.selectedObjectSubject.asObservable();

  public selectedObject: THREE.Object3D | null = null;
  private originalMaterial: THREE.Material | null = null;

  private locationService: LocationService;
  private mapUnitsService: MapUnitsService;

  private signalRService: SignalRService;
  private stepSubscription!: Subscription;
  
  constructor(renderer: THREE.WebGLRenderer, locationService: LocationService, mapUnitsService: MapUnitsService, signalRService: SignalRService) {
    this.renderer = renderer;

    this.locationService = locationService;
    this.mapUnitsService = mapUnitsService;
    this.signalRService = signalRService;

    this.initScene();

    this.stepSubscription = this.signalRService.fullState$.subscribe(message => {
      if (message) {
        for (const unit of message.parameter.units) {
          const object3d: THREE.Mesh = this.scene.getObjectByName(unit.name) as THREE.Mesh;
          console.log(unit.position);
          const position = CoordinateConverter.GpsToWorldPosition(unit.position as GPSPosition);
          object3d.position.x = position.x;
          object3d.position.y = position.y;
          object3d.position.z = position.z;
        }
      }
    });
  }

  dispose(): void {
    if (this.stepSubscription) {
      this.stepSubscription.unsubscribe();
    }
  }

  initScene() {
    // Scene
    this.scene = new THREE.Scene();
    this.scene.add(new THREE.AxesHelper(5));
    this.earth = this.createPlanet();

    // Camera
    const renderSize = new THREE.Vector2();
    this.renderer.getSize(renderSize);
    this.camera = this.createCamera(renderSize.width / renderSize.height);

    this.earth.add(this.camera);

    // Lights
    this.setupLighting();

    // Fill scene
    this.createLocations();
    this.createUnits();

    // Controls
    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.update();
  }

  createCamera(renderRatio: number): THREE.PerspectiveCamera {
    const camera = new THREE.PerspectiveCamera(75, renderRatio, 0.1, 1000);
    camera.position.z = CoordinateConverter.EarthRadius + CoordinateConverter.EarthRadius * 0.5;

    return camera;
  }

  createUnits() {
    const material = new THREE.MeshBasicMaterial({ color: "#00FF00" });

    const locations = this.mapUnitsService.getUnits();
    locations.subscribe((value: MapUnit[]) => {
      value.forEach((unit: MapUnit) => {
        const geometry = new THREE.SphereGeometry(10);
        const position = CoordinateConverter.GpsToWorldPosition(unit.position);

        const unitVue = new THREE.Mesh(geometry, material);
        unitVue.name = unit.name;
        unitVue.position.x = position.x;
        unitVue.position.y = position.y;
        unitVue.position.z = position.z;

        unitVue.userData = unit;

        this.earth.add(unitVue);
      });
    });
  }

  createLocations() {
    const material = new THREE.MeshBasicMaterial({ color: "#FF0000" });

    const locations = this.locationService.getLocations();
    locations.subscribe((value: MapLocation[]) => {
      value.forEach((location: MapLocation) => {
        const geometry = new THREE.SphereGeometry(10);
        const position = CoordinateConverter.GpsToWorldPosition(location.position);

        const locationVue = new THREE.Mesh(geometry, material);
        locationVue.position.x = position.x;
        locationVue.position.y = position.y;
        locationVue.position.z = position.z;

        locationVue.userData = location;

        this.earth.add(locationVue);
      });
    });
  }

  createPlanet(): THREE.Mesh {
    const geometry = new THREE.SphereGeometry(CoordinateConverter.EarthRadius, 100, 100);
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
    // Ambient lightning
    const ambientLight = new THREE.AmbientLight(0x404040, 0.3);
    this.scene.add(ambientLight);

    // Directionnal light
    const directionalLight = new THREE.DirectionalLight(0xffffff, 1.2);
    directionalLight.position.set(5, 0, 5);
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
    this.earth.rotation.y += 0.001;

    this.controls.update();
  }

  clickEvent(intersects: THREE.Intersection<THREE.Object3D<THREE.Object3DEventMap>>[]) {
    if (intersects.length > 0) {
      this.selectObject(intersects[0].object);
    } else {
      this.deselectObject();
    }
  }

  private selectObject(object: THREE.Object3D) {
    // Deselect previous selected object
    this.deselectObject();

    if (object == this.earth)
      return;

    // Select current object
    this.selectedObject = object;

    // Change selected object material
    if (object instanceof THREE.Mesh) {
      this.originalMaterial = object.material;
      object.material = new THREE.MeshLambertMaterial({
        color: 0xffff00,
        transparent: true,
        opacity: 0.8
      });
    }

    // Create selected object data
    const properties: ObjectProperties = {
      id: object.uuid,
      name: object.name || 'Objet sans nom',
      position: object.position.clone(),
      rotation: object.rotation.clone(),
      scale: object.scale.clone(),
      material: this.originalMaterial,
      geometry: object instanceof THREE.Mesh ? object.geometry : null
    };

    // Trigger selected object display
    this.selectedObjectSubject.next(properties);
  }

  private deselectObject() {
    if (this.selectedObject && this.originalMaterial) {
      if (this.selectedObject instanceof THREE.Mesh) {
        this.selectedObject.material = this.originalMaterial;
      }
    }
    this.selectedObject = null;
    this.originalMaterial = null;
    this.selectedObjectSubject.next(null);
  }

}
