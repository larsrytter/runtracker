import TripModel from '@/models/trip.model';
import CreateTripModel from '@/models/create-trip.model';
import Vue from 'vue';
import Component from 'vue-class-component'

@Component
export default class ActiveTripService extends Vue {

    public GetActiveTrip(): TripModel | null {
        let activityTypeId: number = 1;
        const tmpTrip: TripModel = {
            ActivityTypeId: activityTypeId,
            TimeStart: new Date(),
            TimeEnd: new Date(),
            TripGuid: 'weifuhwef7823ry238h'
        };
        return tmpTrip; 
    }

    public async StartNewTrip() {
        let activityTypeId: number = 1;
        const createTripModel: CreateTripModel = {
            activityTypeId: activityTypeId
        }
        
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "text/json" },
            body: JSON.stringify(createTripModel)
        };

        const url = '/trip/create';
        const response = await fetch(url, requestOptions);
        const trip: TripModel = await response.json();
        return trip;
    }
}
