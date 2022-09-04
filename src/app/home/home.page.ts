import { Component, ElementRef, ViewChild } from '@angular/core';
import { Geolocation } from '@capacitor/geolocation';
import { Socket } from 'ngx-socket-io';
import { AngularFireAuth } from '@angular/fire/compat/auth';

declare let google;

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild('map') mapElement: ElementRef;
  private map: any;
  private center: any;
  //private myPositionMarker: any;
  private clientsMarkers: Array<any> = [];
  user = null;

  constructor(private afAuth: AngularFireAuth, private socket: Socket) {
    this.anonLogin();
  }

  ionViewDidEnter() {
    Geolocation.getCurrentPosition({enableHighAccuracy: true}).then((resp) => {
      this.center = {
        latitude: resp.coords.latitude,
        longitude: resp.coords.longitude,
        title: this.user.uid
      };
      this.renderMap();
      this.socket.emit("set_position", this.center);
    });
  }

  anonLogin() {
    this.afAuth.signInAnonymously().then(res => {
      this.user = res.user;
    });
  }

  listenPositionUser() {
    Geolocation.watchPosition({enableHighAccuracy: true}, (position) => {
      if (position) {
        this.center.latitude = position.coords.latitude;
        this.center.longitude = position.coords.longitude;
        const position2 = new google.maps.LatLng(this.center.latitude, this.center.longitude);
        this.map.setCenter(position2);
        //this.myPositionMarker.setPosition(position2);
        this.socket.emit("set_position", this.center);
      }
    });
  }

  listenClientsConnected() {
    this.socket.on("clients_update", clients => {
      clients.forEach(client => {
        let index = this.clientsMarkers.map(item => item.ref).indexOf(client.ref);
        if (index == -1) {
          const latLng = new google.maps.LatLng(client.location.latitude, client.location.longitude);
          let marker = new google.maps.Marker({
            map: this.map,
            animation: google.maps.Animation.DROP,
            position: latLng,
            title: client.location.title
          });
          this.clientsMarkers.push({
            ref: client.ref,
            marker: marker
          })
        } else {
          const latLng = new google.maps.LatLng(client.location.latitude, client.location.longitude);
          this.clientsMarkers[index].marker.setPosition(latLng);
        }
      });
    });
  }

  renderMap() {
    const mapOptions = {
      center: [this.center.latitude, this.center.longitude],
      zoom: 16,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    this.map = new google.maps.Map(this.mapElement.nativeElement, mapOptions);
    const latLng = new google.maps.LatLng(this.center.latitude, this.center.longitude);
    /*
    this.myPositionMarker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: latLng
    });
    */
    setTimeout(() => {
      this.listenPositionUser();
      this.listenClientsConnected();
    }, 1000)
  }

}
