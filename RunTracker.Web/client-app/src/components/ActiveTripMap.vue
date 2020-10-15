<template>
    <div id="map" class="map"></div>
</template>

<script lang="ts">
import TripExtendedModel from '@/models/trip-extended.model';
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import 'ol/ol.css';
import Map from 'ol/Map';
import OSM from 'ol/source/OSM';
import VectorSource from 'ol/source/Vector';
import ATTRIBUTION from 'ol/source/OSM';
import TileLayer from 'ol/layer/Tile';
import View from 'ol/View';
import WKT from 'ol/format/WKT';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import Geometry from 'ol/geom/Geometry';
// Problems with using exported types from ol/proj in typescript
const fromLonLat = (<any>require('ol/proj')).fromLonLat;


@Component
export default class ActiveTripMap extends Vue {
    @Prop() public trip: TripExtendedModel|null = null;
    @Prop() public center: [number, number]|null = null;

    private _map: Map|null = null;
    private _vectorLayer: VectorLayer;

    constructor() {
        super();

        console.log('create vector layer');
        this._vectorLayer = new VectorLayer({
            source: new VectorSource({
                features: [],
            }),
        });
        console.log(this._vectorLayer);
    }

    @Watch('trip', {immediate: true}) onTripUpdated() {
        if(this._vectorLayer) {
            let features = this._vectorLayer.getSource().getFeatures();

            features.map((feature: Feature) => {
                this._vectorLayer.getSource().removeFeature(feature);
            });

            const format = new WKT();
            if(this.trip && this.trip.wkt) {
                
                const feature: Feature = format.readFeature(this.trip.wkt, {
                    dataProjection: 'EPSG:4326',
                    featureProjection: 'EPSG:3857',
                });
                const geometry = feature.getGeometry();
                
                this._vectorLayer.getSource().addFeature(feature);
                if(this._map) {
                    if(this.center)
                    {
                        const centerTransformed = fromLonLat(this.center);
                        this._map.getView().setCenter(centerTransformed);
                    } else {
                        this._map.getView().fit(geometry.getExtent(), {padding: [170, 50, 30, 150]});
                    }
                }
                
            }
        }
        
    }

    public mounted() {
        this._vectorLayer = new VectorLayer({
            source: new VectorSource({
                features: [],
            }),
        });

        this._map = new Map({
            layers: [
                new TileLayer({
                    source: new OSM(),
                }) ],
            target: 'map',
            view: new View({
                center: [-6655.5402445057125, 6709968.258934638],
                zoom: 14,
            }),
        });

        if(this._vectorLayer !== null) {
            console.log('add vector layer', this._vectorLayer);
            this._map.addLayer(this._vectorLayer);
        }
        
    }
}

</script>
<style scoped>
      .map {
        width: 100%;
        height:50vh;
      }
</style>