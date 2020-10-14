import TripModel from '@/models/trip.model';
import CreateTripModel from '@/models/create-trip.model';
import Vue from 'vue';
import Component from 'vue-class-component'
import AddTripTickModel from '@/models/add-trip-tick.model';
import TripExtendedModel from '@/models/trip-extended.model';
// import { interval, Subscription } from 'rxjs';

@Component
export default class ActiveTripService extends Vue {

    private _currentTrip: TripModel | null = null;

    public async GetActiveTrip(): Promise<TripModel | null> {
        // let activityTypeId: number = 1;
        // const tmpTrip: TripModel = {
        //     ActivityTypeId: activityTypeId,
        //     TimeStart: new Date(),
        //     TimeEnd: new Date(),
        //     TripGuid: 'weifuhwef7823ry238h'
        // };
        // return tmpTrip; 

        const url = '/trip/opentrips';
        const requestOptions = {
            method: 'GET'
        };
        const response = await fetch(url, requestOptions);
        let trips: TripModel[] = await response.json() as TripModel[];
        console.log(response, 'response');
        if(trips && trips.length > 0) {
            this._currentTrip = trips[0];
        }

        return this._currentTrip;
    }

    public async StartNewTrip() {
        let activityTypeId: number = 1;
        const createTripModel: CreateTripModel = {
            activityTypeId: activityTypeId
        }
        
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(createTripModel)
        };

        const url = '/trip/create';
        const response = await fetch(url, requestOptions);
        this._currentTrip = await response.json() as TripModel;
        return this._currentTrip;
    }

    public async AddTripTick(lat: number, long: number) {
        const tripTick: AddTripTickModel = {
            lat: lat,
            long: long
        };
        const tripGuid = this._currentTrip ? this._currentTrip.tripGuid : null;
        if (!tripGuid)
        {
            return;
        }
        const url = `/trip/${tripGuid}/ticks/add`;
        
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(tripTick)
        };

        const response = await fetch(url, requestOptions);
        // this._currentTrip = await response.json() as TripModel;
        return;
    }

    public async EndCurrentTrip() {
        
        const tripGuid = this._currentTrip ? this._currentTrip.tripGuid : null;
        if (!tripGuid)
        {
            return;
        }
        const url = `/trip/${tripGuid}/close`;
        const requestOptions = {
            method: "GET",
        };
        const response = await fetch(url, requestOptions);
        if (response.ok) {
            this._currentTrip = await response.json() as TripModel;
        }
    }

    public getPosition(): Promise<Position> {
        return new Promise(function(resolve, reject) {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
    }

    public async getTripExtended(tripGuid: string): Promise<TripExtendedModel|null> {
        let tripExtended: TripExtendedModel|null = null;
        const url = `/trip/${tripGuid}/extended`;
        const requestOptions = {
            method: "GET",
        };
        const response = await fetch(url, requestOptions);
        if (response.ok) {
            tripExtended = await response.json() as TripExtendedModel;
        }
        return tripExtended;
    }
}
