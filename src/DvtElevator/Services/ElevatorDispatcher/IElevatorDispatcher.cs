using DvtElevator.Models.Elevators;

namespace DvtElevator.Services.ElevatorDispatcher;

public interface IElevatorDispatcher
{
    public Task<ElevatorBase> ElevatorPicker(List<ElevatorBase> elevators, byte targetFloor,
        int numberOfWaitingPassengers);
}