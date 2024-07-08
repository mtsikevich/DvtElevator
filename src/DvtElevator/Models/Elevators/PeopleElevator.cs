namespace DvtElevator.Models.Elevators;

public class PeopleElevator(
    Kg? maxPassengerWeightLimit,
    Floor highestFloor,
    short averageMillisecondsBetweenFloors = 1500)
    : ElevatorBase(maxPassengerWeightLimit, highestFloor, averageMillisecondsBetweenFloors);