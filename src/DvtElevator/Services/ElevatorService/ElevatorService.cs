using DvtElevator.Models.Elevators;
using DvtElevator.Services.ElevatorDispatcher;

namespace DvtElevator.Services.ElevatorService;

public class ElevatorService(
    IElevatorDispatcher elevatorDispatcher,
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
    
    public Task<ElevatorBase> PickOptimalElevatorAsync(byte floor, byte numberOfWaitingPassengers)
    {
        Task<ElevatorBase> pickedElevator = default!;
        try
        {
            if (_elevators.Count == 0)
                throw new BuildingHasNoElevators();

            pickedElevator = elevatorDispatcher.ElevatorPicker(_elevators, floor, numberOfWaitingPassengers);
        }
        catch (ElevatorsNotAvailable)
        {
            Console.WriteLine();
        }

        return pickedElevator;
    }
}