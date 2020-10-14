export default interface TripExtendedModel {
    tripGuid: string;
    activityTypeId: number;
    timeStart: Date;
    timeEnd: Date|null;
    wkt: string;
}
