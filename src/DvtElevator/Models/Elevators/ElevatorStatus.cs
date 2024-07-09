namespace DvtElevator.Models.Elevators;

/// <summary>
/// Elevator status data type
/// </summary>
public class ElevatorStatus: ICloneable
{
    public bool IsMoving { get; set; }
    public Direction Direction { get; set; }
    public Floor CurrentFloor { get; set; }
    public Kg MaxPassengerWeightLimit { get; set; }
    public Kg CurrentLoad { get; set; }
    public DoorState DoorState { get; set; }
    public Floor HighestFloor { get; set; }
    
    /// <summary>
    /// Create a deep clone of the object.
    /// </summary>
    /// <returns>Deep clone of elevator status object.</returns>
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