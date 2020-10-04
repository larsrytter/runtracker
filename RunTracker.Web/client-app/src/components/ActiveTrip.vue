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
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import TripModel from './../models/trip.model'
import TripService from './../services/trip.service';

@Component
export default class ActiveTrip extends TripService {

    private hasActiveTrip: boolean = false;
    public message = '';

    public get isTripActive(): boolean {
        return this.hasActiveTrip;

        // let currentTrip: TripModel|null = this.GetActiveTrip();
        // let isTripActive: boolean = (currentTrip && currentTrip.timeEnd === null) ? true : false;
        // console.log(isTripActive, 'isTripActive');
        // return isTripActive;
    }

    public set isTripActive(val: boolean) {
        this.hasActiveTrip = val;
    }

    public created() {
      this.GetActiveTrip().then((trip: TripModel|null) => {
        if (trip === null) {
            this.isTripActive = false;
        } else if (trip && trip.timeEnd === null) {
            this.isTripActive = true;
        } else {
            this.isTripActive = false;
        }
      });
    }
    
    async startTrip() {
        let currentTrip: TripModel|null = await this.GetActiveTrip();
        if(!currentTrip || currentTrip.timeEnd !== null) {
            console.log('start trip');

            const positionOrFalse = await this.ensureDeviceLocationEnabled();
            if (!positionOrFalse) {
                this.message = 'Could not get position of device. You need to allow position for app.'
            } else {
                this.StartNewTrip().then((trip:TripModel) => {
                    this.isTripActive = true;
                    // this.currentTrip = trip;
                    
                    // console.log(this.currentTrip);
                });
            }
        }
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
        this.EndCurrentTrip().then(() => {
            this.isTripActive = false;
        });
        
    }

    // created() {
    //     let currentTrip: TripModel|null = this.GetActiveTrip();
    // }
}
</script>