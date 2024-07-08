using DvtElevator.Models.Elevators;

namespace DvtElevator.Services.ElevatorDispatcher;

public class ElevatorDispatcher: IElevatorDispatcher
{
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