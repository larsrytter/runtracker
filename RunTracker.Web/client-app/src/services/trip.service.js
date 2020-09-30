import { __decorate } from "tslib";
import Vue from 'vue';
import Component from 'vue-class-component';
// import { interval, Subscription } from 'rxjs';
let ActiveTripService = class ActiveTripService extends Vue {
    constructor() {
        super(...arguments);
        this._currentTrip = null;
        this._tripTickIntervalDuration = 2000;
    }
    GetActiveTrip() {
        // let activityTypeId: number = 1;
        // const tmpTrip: TripModel = {
        //     ActivityTypeId: activityTypeId,
        //     TimeStart: new Date(),
        //     TimeEnd: new Date(),
        //     TripGuid: 'weifuhwef7823ry238h'
        // };
        // return tmpTrip; 
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
        this.StartTripTicksInterval();
        return this._currentTrip;
    }
    StartTripTicksInterval() {
        this._tripTickInterval = setInterval(async () => {
            navigator.geolocation.getCurrentPosition(async (position) => {
                const lat = position.coords.latitude;
                const long = position.coords.longitude;
                const altitude = position.coords.altitude;
                const timestamp = position.timestamp;
                await this.AddTripTick(lat, long);
            });
        }, this._tripTickIntervalDuration);
    }
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
        debugger;
        const response = await fetch(url, requestOptions);
        this._currentTrip = await response.json();
    }
    async EndTrip() {
        clearInterval(this._tripTickInterval);
        //TODO: End trip in API
    }
};
ActiveTripService = __decorate([
    Component
], ActiveTripService);
export default ActiveTripService;
//# sourceMappingURL=trip.service.js.map