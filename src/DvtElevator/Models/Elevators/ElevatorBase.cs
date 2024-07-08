using System.Reactive.Linq;

namespace DvtElevator.Models.Elevators;

public abstract class ElevatorBase(Kg? maxPassengerWeightLimit, Floor highestFloor, short averageMillisecondsBetweenFloors)
{
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
    
    public int? Number { get; set; }
    protected virtual TimeSpan AverageTimeToGetIntoElevator => TimeSpan.FromSeconds(2);
    
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

    private void UpdateMovementAndDoorStatus(Direction elevatorDirection, DoorState elevatorDoorState, bool isMoving)
    {
        ElevatorStatus.Direction = elevatorDirection;
        ElevatorStatus.DoorState = elevatorDoorState;
        ElevatorStatus.IsMoving = isMoving;
    }
}