namespace DvtElevator.Exceptions;

public class BuildingHasNoElevators() : Exception(string.Empty);
public class ElevatorExceedsBuildingFloors() : Exception(string.Empty);
public class ElevatorsNotAvailable(): Exception(string.Empty);