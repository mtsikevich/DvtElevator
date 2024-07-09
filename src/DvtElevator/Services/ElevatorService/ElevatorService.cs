using DvtElevator.Models.Elevators;
using DvtElevator.Services.ElevatorDispatcher;

namespace DvtElevator.Services.ElevatorService;

public class ElevatorService(
    IElevatorDispatcher elevatorDispatcher,
    byte buildingFloorCount = 10) : IElevatorService
{
    private readonly List<ElevatorBase> _elevators=[];

    /// <summary>
    /// Returns the number of floors the building has
    /// </summary>
    public byte BuildingFloorCount => buildingFloorCount;

    /// <summary>
    /// Used to add the elevator to the building
    /// </summary>
    /// <param name="elevator">The elevator to add to the building</param>
    /// <exception cref="ElevatorExceedsBuildingFloors">Thrown when the elevator being added is configured to have access to more buildings that the building has</exception>
    public void AddElevator(ElevatorBase elevator)
    {
        elevator.Number = _elevators.Count + 1;
        
        var elevatorExceedsBuildingFloors = elevator.ElevatorStatus.HighestFloor> buildingFloorCount;
        if (elevatorExceedsBuildingFloors)
            throw new ElevatorExceedsBuildingFloors();
        _elevators.Add(elevator);
    }
    
    /// <summary>
    /// Picks any of the elevators near the target floor 
    /// </summary>
    /// <param name="floor">The target floor</param>
    /// <param name="numberOfWaitingPassengers">Number of passengers requesting the elevator</param>
    /// <returns>Returns the nearest elevator</returns>
    /// <exception cref="BuildingHasNoElevators">Thrown when there are no elevators to pick from</exception>
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