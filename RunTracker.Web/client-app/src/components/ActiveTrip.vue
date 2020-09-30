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
import { Component, Prop, Vue } from 'vue-property-decorator';
import TripModel from './../models/trip.model'
import TripService from './../services/trip.service';

@Component
export default class ActiveTrip extends TripService {
    // @Prop() private currentTrip: TripModel | null = null;


    get isTripActive(): boolean {
        let currentTrip: TripModel|null = this.GetActiveTrip();
        let isTripActive: boolean = (currentTrip && currentTrip.timeEnd === null) ? true : false;
        return isTripActive;
    }
    
    startTrip() {
        let currentTrip: TripModel|null = this.GetActiveTrip();
        if(!currentTrip || currentTrip.timeEnd !== null) {
            console.log('start trip');

            this.StartNewTrip().then((trip:TripModel) => {
                // this.currentTrip = trip;
                
                // console.log(this.currentTrip);
            });

        }
    }

    endTrip() {
        this.EndCurrentTrip();
    }

    // created() {
    //     let currentTrip: TripModel|null = this.GetActiveTrip();
    // }
}
</script>