using DvtElevator.Models.Elevators;

namespace DvtElevator.Services.ElevatorDispatcher;

public class ElevatorDispatcher: IElevatorDispatcher
{
    /// <summary>
    /// Picks any of the elevators nearest to the target floor
    /// </summary>
    /// <param name="elevators">Elevator collection to pick from</param>
    /// <param name="targetFloor">The requested floor</param>
    /// <param name="numberOfWaitingPassengers">Number of passengers waiting for the elevator</param>
    /// <returns>Elevator nearest to the target floor</returns>
    /// <exception cref="ElevatorsNotAvailable">Thrown when no elevator has been found</exception>
    public Task<ElevatorBase> ElevatorPicker(List<ElevatorBase> elevators, byte targetFloor,
        int numberOfWaitingPassengers)
    {
        var totalWaitingWeight = Constants.AveragePersonWeight * numberOfWaitingPassengers;
        
        var fastestElevator = elevators
            .Where(e => totalWaitingWeight + e.ElevatorStatus.CurrentLoad <= e.ElevatorStatus.MaxPassengerWeightLimit 
                        && !e.ElevatorStatus.IsMoving
                        && e.ElevatorStatus.HighestFloor > targetFloor)
            .MinBy(e => Math.Abs(e.ElevatorStatus.CurrentFloor - targetFloor));

        if (fastestElevator == null)
            throw new ElevatorsNotAvailable();
        
        return Task.FromResult(fastestElevator);
    }
}