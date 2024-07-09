using System.Reactive.Linq;

namespace DvtElevator.Models.Elevators;

/// <summary>
/// The base elevator 
/// </summary>
/// <param name="maxPassengerWeightLimit">The maximum elevator weight limit</param>
/// <param name="highestFloor">The highest floor the elevator will have access to</param>
/// <param name="averageMillisecondsBetweenFloors">The average time between floor (elevator speed)</param>
public abstract class ElevatorBase(Kg? maxPassengerWeightLimit, Floor highestFloor, short averageMillisecondsBetweenFloors)
{
    /// <summary>
    /// Elevator status
    /// </summary>
    public ElevatorStatus ElevatorStatus { get;} = new()
    {
        IsMoving = false,
        Direction = Direction.None,
        DoorState = DoorState.Closed,
        CurrentFloor = 0,
        HighestFloor = highestFloor,
        CurrentLoad = 0,
        MaxPassengerWeightLimit = maxPassengerWeightLimit ?? Constants.MaxWeightForPeopleElevator
    };
    
    /// <summary>
    /// The elevator number (assigned by the elevator service), e.g. 1, 2, 3 ...
    /// </summary>
    public int? Number { get; set; }
    
    /// <summary>
    /// The time it takes the passenger to enter the elevator
    /// </summary>
    protected virtual TimeSpan AverageTimeToGetIntoElevator => TimeSpan.FromSeconds(2);
    
    /// <summary>
    /// Process the elevator request from the prompt
    /// </summary>
    /// <param name="targetFloor">The requested floor</param>
    /// <returns>Observable that will show the changing elevator state in realtime</returns>
    public virtual IObservable<ElevatorStatus> ProcessElevatorRequestAsync(byte targetFloor)
    {
        var initialFloor = ElevatorStatus.CurrentFloor;
        
        return Observable.Create<ElevatorStatus>(async observer =>
        {
            if (ElevatorStatus.CurrentFloor != targetFloor)
            {
                var requestedOnUpperFloor = initialFloor <= targetFloor;
                UpdateMovementAndDoorStatus(requestedOnUpperFloor ? Direction.Up : Direction.Down, DoorState.Closed, true);
            
                var increment = requestedOnUpperFloor ? 1 : -1;
                for(int i = initialFloor; increment > 0 ? i <= targetFloor : i >= targetFloor; i += increment)
                {
                    ElevatorStatus.CurrentFloor = (Floor)i;
                    observer.OnNext((ElevatorStatus)ElevatorStatus.Clone());
                    await Task.Delay(averageMillisecondsBetweenFloors);
                }
            }

            UpdateMovementAndDoorStatus(Direction.None, DoorState.Open, false);
            observer.OnNext((ElevatorStatus)ElevatorStatus.Clone());
            observer.OnCompleted();
        });
    }

    /// <summary>
    /// Updates the state of the elevator properties
    /// </summary>
    /// <param name="elevatorDirection">The direction of the elevator</param>
    /// <param name="elevatorDoorState">The door state (open or close) of the elevator</param>
    /// <param name="isMoving">Indicates whether the elevator is moving or not</param>
    private void UpdateMovementAndDoorStatus(Direction elevatorDirection, DoorState elevatorDoorState, bool isMoving)
    {
        ElevatorStatus.Direction = elevatorDirection;
        ElevatorStatus.DoorState = elevatorDoorState;
        ElevatorStatus.IsMoving = isMoving;
    }
}