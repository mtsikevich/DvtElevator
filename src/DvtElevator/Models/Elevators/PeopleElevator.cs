namespace DvtElevator.Models.Elevators;

/// <summary>
/// Elevator for carrying People (Human Passengers)
/// </summary>
/// <param name="maxPassengerWeightLimit">Maximum passenger weight limit</param>
/// <param name="highestFloor">The highest floor the elevator has access to</param>
/// <param name="averageMillisecondsBetweenFloors">The average time (in milliseconds) between floors. The elevator speed</param>
public class PeopleElevator(
    Kg? maxPassengerWeightLimit,
    Floor highestFloor,
    short averageMillisecondsBetweenFloors = 1500)
    : ElevatorBase(maxPassengerWeightLimit, highestFloor, averageMillisecondsBetweenFloors);