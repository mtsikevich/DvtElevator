using DvtElevator.Models.Elevators;

namespace DvtElevator.Services.ElevatorService;

public class ElevatorService(
    byte buildingFloorCount = 10) : IElevatorService
{
    private readonly List<ElevatorBase> _elevators=[];

    public byte BuildingFloorCount => buildingFloorCount;

    public void AddElevator(ElevatorBase elevator)
    {
        elevator.Number = _elevators.Count + 1;
        
        var elevatorExceedsBuildingFloors = elevator.ElevatorStatus.HighestFloor> buildingFloorCount;
        if (elevatorExceedsBuildingFloors)
            throw new ElevatorExceedsBuildingFloors();
        _elevators.Add(elevator);
    }
    
    public Task<ElevatorBase?> PickOptimalElevatorAsync(byte floor, byte numberOfWaitingPassengers)
    {
        if (_elevators.Count == 0)
            throw new BuildingHasNoElevators();

        return Task.FromResult(_elevators.FirstOrDefault());
    }
}