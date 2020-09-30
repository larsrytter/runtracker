export default interface TripModel {
    tripGuid: string;
    activityTypeId: number;
    timeStart: Date;
    timeEnd: Date|null;

}
