import { __decorate } from "tslib";
import Vue from 'vue';
import Component from 'vue-class-component';
// import { interval, Subscription } from 'rxjs';
let ActiveTripService = class ActiveTripService extends Vue {
    constructor() {
        super(...arguments);
        this._currentTrip = null;
    }
    // private _tripTickInterval: any;
    // private _tripTickIntervalDuration: number = 4000;
    async GetActiveTrip() {
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
        let trips = await response.json();
        console.log(response, 'response');
        if (trips && trips.length > 0) {
            this._currentTrip = trips[0];
        }
        return this._currentTrip;
    }
    async StartNewTrip() {
        let activityTypeId = 1;
        const createTripModel = {
            activityTypeId: activityTypeId
        };
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(createTripModel)
        };
        const url = '/trip/create';
        const response = await fetch(url, requestOptions);
        this._currentTrip = await response.json();
        // this.StartTripTicksInterval();
        return this._currentTrip;
    }
    // public StartTripTicksInterval() {
    //     this._tripTickInterval = window.setInterval( () => {
    //         this.getPosition().then(position => {
    //             const lat = position.coords.latitude;
    //             const long = position.coords.longitude;
    //             const altitude = position.coords.altitude;
    //             const timestamp: number = position.timestamp;
    //             console.log('Add triptick ' + timestamp);
    //             this.AddTripTick(lat, long).then((promise) => {
    //                 console.log('triptick added');
    //             });
    //         });
    //     }, 3500);
    //     // }, this._tripTickIntervalDuration);
    // }
    async AddTripTick(lat, long) {
        const tripTick = {
            lat: lat,
            long: long
        };
        const tripGuid = this._currentTrip ? this._currentTrip.tripGuid : null;
        if (!tripGuid) {
            return;
        }
        const url = `/trip/${tripGuid}/ticks/add`;
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(tripTick)
        };
        const response = await fetch(url, requestOptions);
        this._currentTrip = await response.json();
    }
    async EndCurrentTrip() {
        // window.clearInterval(this._tripTickInterval);
        const tripGuid = this._currentTrip ? this._currentTrip.tripGuid : null;
        if (!tripGuid) {
            return;
        }
        const url = `/trip/${tripGuid}/close`;
        const requestOptions = {
            method: "GET",
        };
        const response = await fetch(url, requestOptions);
        if (response.ok) {
            this._currentTrip = await response.json();
        }
    }
    getPosition() {
        return new Promise(function (resolve, reject) {
            navigator.geolocation.getCurrentPosition(resolve, reject);
        });
    }
};
ActiveTripService = __decorate([
    Component
], ActiveTripService);
export default ActiveTripService;
//# sourceMappingURL=trip.service.js.map