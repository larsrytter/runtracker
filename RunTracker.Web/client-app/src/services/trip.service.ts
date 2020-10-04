import TripModel from '@/models/trip.model';
import CreateTripModel from '@/models/create-trip.model';
import Vue from 'vue';
import Component from 'vue-class-component'
import AddTripTickModel from '@/models/add-trip-tick.model';
// import { interval, Subscription } from 'rxjs';

@Component
export default class ActiveTripService extends Vue {

    private _currentTrip: TripModel | null = null;
    private _tripTickInterval: any;
    private _tripTickIntervalDuration: number = 4000;

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
        this.StartTripTicksInterval();
        return this._currentTrip;
    }

    public StartTripTicksInterval() {
        console.log('StartTripTicksInterval');
        this._tripTickInterval = window.setInterval( () => {

            this.getPosition().then(position => {
                const lat = position.coords.latitude;
                const long = position.coords.longitude;
                const altitude = position.coords.altitude;
                const timestamp: number = position.timestamp;
                console.log('Add triptick ' + timestamp);
                this.AddTripTick(lat, long).then((promise) => {
                    console.log('triptick added');
                });
            });
        }, 3000);
        // }, this._tripTickIntervalDuration);
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
        this._currentTrip = await response.json() as TripModel;
    }

    public async EndCurrentTrip() {
        window.clearInterval(this._tripTickInterval);
        
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
}
