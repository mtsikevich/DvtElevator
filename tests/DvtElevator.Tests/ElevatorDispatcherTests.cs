using DvtElevator.Exceptions;
using DvtElevator.Models.Elevators;
using DvtElevator.Services.ElevatorDispatcher;
using FluentAssertions;

namespace DvtElevator.Tests;

public class ElevatorDispatcherTests
{
    [Fact]
    public async Task ElevatorPicker_ShouldRaiseElevatorsNotAvailableException()
    {
        var sut = new ElevatorDispatcher();
        var peopleElevator = new PeopleElevator(null, highestFloor: 10);
        peopleElevator.ElevatorStatus.IsMoving = true;
        var elevators = new List<ElevatorBase> { peopleElevator };
        byte targetFloor = 5;
        int numberOfWaitingPassengers = 3;
        
        var act = () => sut.ElevatorPicker(elevators,targetFloor,numberOfWaitingPassengers);

        await act.Should().ThrowAsync<ElevatorsNotAvailable>();
    }

    [Fact] 
    public async Task ElevatorPicker_ShouldUseTheElevatorAlreadyOnTheFloorIfThereIsAny()
    {
        byte targetFloor = 4;
        var sut = new ElevatorDispatcher();
        var elevator1 = new PeopleElevator(null, highestFloor: 10);
        var elevator2 = new PeopleElevator(null, highestFloor: 10);
        elevator2.ElevatorStatus.CurrentFloor = targetFloor;
        var elevator3 = new PeopleElevator(null, highestFloor: 10);

        var elevators = new List<ElevatorBase> { elevator1, elevator2, elevator3 };
        var selectedElevator = await sut.ElevatorPicker(elevators, targetFloor, 3);

        selectedElevator.Should().BeSameAs(elevator2);
    }
    
    [Fact]
    public async Task ElevatorPicker_ShouldPickTheElevatorOnTheNextFloor()
    {
        byte targetFloor = 4;
        var sut = new ElevatorDispatcher();
        var elevator1 = new PeopleElevator(null, highestFloor: 10);
        var elevator2 = new PeopleElevator(null, highestFloor: 10);
        elevator2.ElevatorStatus.CurrentFloor = (byte)(targetFloor+1);
        var elevator3 = new PeopleElevator(null, highestFloor: 10);

        var elevators = new List<ElevatorBase> { elevator1, elevator2, elevator3 };
        var selectedElevator = await sut.ElevatorPicker(elevators, targetFloor, 3);

        selectedElevator.Should().BeSameAs(elevator2);
    }
    
    [Fact]
    public async Task ElevatorPicker_ShouldPickTheElevatorOnThePreviousFloor()
    {
        byte targetFloor = 4;
        var sut = new ElevatorDispatcher();
        var elevator1 = new PeopleElevator(null, highestFloor: 10);
        var elevator2 = new PeopleElevator(null, highestFloor: 10);
        elevator2.ElevatorStatus.CurrentFloor = (byte)(targetFloor-1);
        var elevator3 = new PeopleElevator(null, highestFloor: 10);

        var elevators = new List<ElevatorBase> { elevator1, elevator2, elevator3 };
        var selectedElevator = await sut.ElevatorPicker(elevators, targetFloor, 3);

        selectedElevator.Should().BeSameAs(elevator2);
    }
}