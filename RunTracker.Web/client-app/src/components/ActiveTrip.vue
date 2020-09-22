<template>
    <div>
        <p v-if="isTripActive">Active trip</p>
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
        let isTripActive = (currentTrip !== null && currentTrip.TimeEnd === null);
        return isTripActive;
    }
    
    startTrip() {
        let currentTrip: TripModel|null = this.GetActiveTrip();
        if(!currentTrip || currentTrip.TimeEnd !== null) {
            console.log('start trip');

            this.StartNewTrip().then((trip:TripModel) => {
                // this.currentTrip = trip;
                
                // console.log(this.currentTrip);
            });

        }
    }

    // created() {
    //     let currentTrip: TripModel|null = this.GetActiveTrip();
    // }
}
</script>