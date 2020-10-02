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

    private hasActiveTrip: boolean = false;

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
          console.log('trip', trip);
          if(trip !== null) {
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