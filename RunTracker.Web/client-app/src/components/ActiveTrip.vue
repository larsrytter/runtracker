<template>
    <div>
        <div v-if="isTripActive">
            <p>Active trip</p>
            <button type="button" v-on:click="endTrip()">Finish the trip</button>
        </div>
        <div v-else>
            No active trip
            <button type="button" v-on:click="startTrip()">Start a new trip</button>
        </div>
        <div>
            {{ message }}
        </div>
        <div>
            <active-trip-map
            v-bind:trip="tripExtended"
            v-bind:center="position" />
        </div>
        
    </div>
</template>

<script lang="ts">
import TripExtendedModel from '@/models/trip-extended.model';
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import TripModel from './../models/trip.model'
import TripService from './../services/trip.service';
import ActiveTripMap from './ActiveTripMap.vue';
import 'wakelock-lazy-polyfill/dist/wakelock-polyfill.umd';

@Component({
    components: {
        ActiveTripMap
    }
})
export default class ActiveTrip extends TripService {

    private hasActiveTrip: boolean = false;
    private _tripTickInterval: any;
    private activeTripGuid: string = '';
    private _wakeLockObj: any;

    public tripExtended: TripExtendedModel|null = null;
    public position: [number, number]|null = null;

    public message = '';

    public get isTripActive(): boolean {
        return this.hasActiveTrip;
    }

    public set isTripActive(val: boolean) {
        this.hasActiveTrip = val;
    }

    public created() {
      this.GetActiveTrip().then((trip: TripModel|null) => {
        if (trip === null) {
            this.isTripActive = false;
            this.activeTripGuid = '';
        } else if (trip && trip.timeEnd === null) {
            this.isTripActive = true;
            this.activeTripGuid = trip.tripGuid;
            this.StartTripTicksInterval();
        } else {
            this.isTripActive = false;
            this.activeTripGuid = '';
        }
      });
    }
    
    async startTrip() {
        let currentTrip: TripModel|null = await this.GetActiveTrip();
        if(!currentTrip || currentTrip.timeEnd !== null) {

            const positionOrFalse = await this.ensureDeviceLocationEnabled();
            if (!positionOrFalse) {
                this.message = 'Could not get position of device. You need to allow position for app.'
            } else {
                this.StartNewTrip().then((trip:TripModel) => {
                    this.isTripActive = true;
                    this.activeTripGuid = trip.tripGuid;

                    this.StartTripTicksInterval();
                });
            }
        }
    }

    public StartTripTicksInterval() {

        // Request wakeLock to prevent screen from locking and making app pause/hibernate
        // TODO: Can this be solved using service-worker instead?
        if ('wakeLock' in navigator) {
            (navigator as any).wakeLock.request('screen').then((lock: any) => {
                this._wakeLockObj = lock;
            }).catch(() => {
                // cannot get screen wake lock
            })
        } else {
            // console.log('Not supported 😐');
        }
        
        this._tripTickInterval = window.setInterval( () => {
            this.getPosition().then(position => {
                const lat = position.coords.latitude;
                const long = position.coords.longitude;
                const altitude = position.coords.altitude;
                const timestamp: number = position.timestamp;
                this.position = [long, lat];
                console.log('Add triptick ' + timestamp);
                this.AddTripTick(lat, long).then((promise) => {
                    // console.log('triptick added');
                    console.log(this.activeTripGuid, 'this.activeTripGuid');
                    if(this.activeTripGuid) {
                        this.getTripExtended(this.activeTripGuid).then((tripExtended: TripExtendedModel|null) => {
                            this.tripExtended = tripExtended;
                        });
                    }
                    
                });
            });
        }, 3200);
        // }, this._tripTickIntervalDuration);
    }



    async ensureDeviceLocationEnabled(): Promise<Position | false> {
        try {
            const position: Position = await this.getPosition();
            return position;
        } catch(error) {
            if (error.code == error.PERMISSION_DENIED) {
                console.warn('Cannot get position - permission denied. ');
            } else {
                console.warn('Cannot get position from device.');
            }
            return false;
        }
    }

    endTrip() {
        window.clearInterval(this._tripTickInterval);
        this.EndCurrentTrip().then(() => {
            this.isTripActive = false;

            if (this._wakeLockObj) {
                this._wakeLockObj.release();
                this._wakeLockObj = null;
            }
        });
        
    }
}
</script>