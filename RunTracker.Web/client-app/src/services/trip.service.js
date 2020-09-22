import { __decorate } from "tslib";
import Vue from 'vue';
import Component from 'vue-class-component';
let ActiveTripService = class ActiveTripService extends Vue {
    GetActiveTrip() {
        let activityTypeId = 1;
        const tmpTrip = {
            ActivityTypeId: activityTypeId,
            TimeStart: new Date(),
            TimeEnd: new Date(),
            TripGuid: 'weifuhwef7823ry238h'
        };
        return tmpTrip;
    }
    async StartNewTrip() {
        let activityTypeId = 1;
        const createTripModel = {
            ActivityTypeId: activityTypeId
        };
        console.log(createTripModel);
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "text/json" },
            body: JSON.stringify(createTripModel)
        };
        console.log(requestOptions);
        const url = '/trip/create';
        const response = await fetch(url, requestOptions);
        debugger;
        let trip = await response.json();
        return trip;
    }
};
ActiveTripService = __decorate([
    Component
], ActiveTripService);
export default ActiveTripService;
//# sourceMappingURL=trip.service.js.map