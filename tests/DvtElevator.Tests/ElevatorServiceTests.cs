using DvtElevator.Exceptions;
using DvtElevator.Models.Elevators;
using DvtElevator.Services.ElevatorService;
using FluentAssertions;

namespace DvtElevator.Tests;


public class ElevatorServiceTests
{

    [Fact]
    public void AddElevator_ShouldRaiseElevatorExceedsBuildingFloorsWhenTheElevatorExceedsBuildingFloors()
    {
        var sut = new ElevatorService(4);
        var peopleElevator = new PeopleElevator(null, highestFloor: 20);

        var act = () => sut.AddElevator(peopleElevator);

        act.Should().Throw<ElevatorExceedsBuildingFloors>();
    }
    
    [Fact]
    public async Task PickOptimalElevatorAsync_ShouldRaiseBuildingHasNoElevatorsWhenThereAreNoElevators()
    {
        var sut = new ElevatorService(4);
        var act = async () => await sut.PickOptimalElevatorAsync(2,2);

        await act.Should().ThrowAsync<BuildingHasNoElevators>();
    }
    
    [Fact]
    public async Task PickOptimalElevatorAsync_ShouldReturnTheFirstElevatorItFinds()
    {
        var peopleElevator = new PeopleElevator(null, highestFloor: 10);
        var sut = new ElevatorService(10);
        sut.AddElevator(peopleElevator);

        var elevator = await sut.PickOptimalElevatorAsync(2, 2);
        
        elevator.Should().BeSameAs(peopleElevator);
    }
}
