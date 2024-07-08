using DvtElevator.Exceptions;
using DvtElevator.Models.Elevators;
using DvtElevator.Services.ElevatorDispatcher;
using DvtElevator.Services.ElevatorService;
using FakeItEasy;
using FluentAssertions;

namespace DvtElevator.Tests;


public class ElevatorServiceTests
{

    [Fact]
    public void AddElevator_ShouldRaiseElevatorExceedsBuildingFloorsWhenTheElevatorExceedsBuildingFloors()
    {
        var fakeDispatcher = A.Fake<IElevatorDispatcher>();
        var sut = new ElevatorService(fakeDispatcher,4);
        var peopleElevator = new PeopleElevator(null, highestFloor: 20);

        var act = () => sut.AddElevator(peopleElevator);

        act.Should().Throw<ElevatorExceedsBuildingFloors>();
    }
    
    [Fact]
    public async Task PickOptimalElevatorAsync_ShouldRaiseBuildingHasNoElevatorsWhenThereAreNoElevators()
    {
        var fakeDispatcher = A.Fake<IElevatorDispatcher>();
        var sut = new ElevatorService(fakeDispatcher,4);
        var act = async () => await sut.PickOptimalElevatorAsync(2,2);

        await act.Should().ThrowAsync<BuildingHasNoElevators>();
    }
    
    [Fact]
    public async Task PickOptimalElevatorAsync_ShouldReturnTheFirstElevatorItFinds()
    {
        var peopleElevator = new PeopleElevator(null, highestFloor: 10);
        
        var fakeDispatcher = A.Fake<IElevatorDispatcher>();
        A.CallTo(() => fakeDispatcher.ElevatorPicker(A<List<ElevatorBase>>._, A<byte>._, A<int>._))
            .Returns(Task.FromResult<ElevatorBase>(peopleElevator));
        
        var sut = new ElevatorService(fakeDispatcher,10);
        sut.AddElevator(peopleElevator);

        var elevator = await sut.PickOptimalElevatorAsync(2, 2);

       elevator.Should().BeOfType<PeopleElevator>();
       elevator.Should().NotBeNull();
    }
}
