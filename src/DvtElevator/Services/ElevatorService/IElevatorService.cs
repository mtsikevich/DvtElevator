using DvtElevator.Models.Elevators;

namespace DvtElevator.Services.ElevatorService;

public interface IElevatorService
{
    void AddElevator(ElevatorBase elevator);
    Task<ElevatorBase> PickOptimalElevatorAsync(byte floor, byte numberOfWaitingPassengers);
}