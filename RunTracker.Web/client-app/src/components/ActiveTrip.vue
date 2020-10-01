<template>
    <div>
        <p v-if="isTripActive">
            Active trip
            <button type="button" v-on:click="endTrip()">Finish the trip</button>
        </p>
        <div v-else>
            No active trip
            <button type="button" v-on:click="startTrip()">Start a new trip</button>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import TripModel from './../models/trip.model'
import TripService from './../services/trip.service';

@Component
export default class ActiveTrip extends TripService {
    // @Prop() private currentTrip: TripModel | null = null;

    // public isTripActive: string;

    // @Watch('isTripActive')

    private hasActiveTrip: boolean = false;

    public get isTripActive(): boolean {
        console.log(this.hasActiveTrip, 'isTripActive');
        return this.hasActiveTrip;

        // let currentTrip: TripModel|null = this.GetActiveTrip();
        // let isTripActive: boolean = (currentTrip && currentTrip.timeEnd === null) ? true : false;
        // console.log(isTripActive, 'isTripActive');
        // return isTripActive;
    }

    public set isTripActive(val: boolean) {
        this.hasActiveTrip = val;
        console.log(this.hasActiveTrip, 'isTripActive');
    }
    
    startTrip() {
        let currentTrip: TripModel|null = this.GetActiveTrip();
        if(!currentTrip || currentTrip.timeEnd !== null) {
            console.log('start trip');

            this.StartNewTrip().then((trip:TripModel) => {
                this.isTripActive = true;
                // this.currentTrip = trip;
                
                // console.log(this.currentTrip);
            });

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