namespace DvtElevator.Models.Elevators;

public class ElevatorStatus: ICloneable
{
    public bool IsMoving { get; set; }
    public Direction Direction { get; set; }
    public Floor CurrentFloor { get; set; }
    public Kg MaxPassengerWeightLimit { get; set; }
    public Kg CurrentLoad { get; set; }
    public DoorState DoorState { get; set; }
    public Floor HighestFloor { get; set; }
    
    public object Clone()
    {
        return new ElevatorStatus
        {
            IsMoving = IsMoving,
            Direction = Direction,
            CurrentFloor = CurrentFloor,
            MaxPassengerWeightLimit = MaxPassengerWeightLimit,
            CurrentLoad = CurrentLoad,
            DoorState = DoorState,
            HighestFloor = HighestFloor
        };
    }
}