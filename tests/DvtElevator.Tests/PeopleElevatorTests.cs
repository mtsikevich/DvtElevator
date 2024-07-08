using System.Reactive.Linq;
using DvtElevator.Models;
using DvtElevator.Models.Elevators;
using FluentAssertions;

namespace DvtElevator.Tests;

public class PeopleElevatorTests
{
    private ElevatorBase _elevator;

    public PeopleElevatorTests()
    {
        _elevator = new PeopleElevator(400, 4, 1500);
    }

    [Fact]
    public async Task ProcessElevatorRequestAsync_ShouldMoveElevatorToTargetFloor()
    {
        byte targetFloor = 2;

        var statuses = await _elevator.ProcessElevatorRequestAsync(targetFloor).ToArray();

        statuses.Last().CurrentFloor.Should().Be(targetFloor);
        statuses.Last().IsMoving.Should().BeFalse();
    }
    
    [Fact]
    public async Task ProcessElevatorRequestAsync_ShouldMoveElevatorInCorrectDirection()
    {
        byte targetFloor = 3;
        var statuses = await _elevator.ProcessElevatorRequestAsync(targetFloor).ToArray();

        var movingUp = statuses.Any(s => s.Direction == Direction.Up);
        var movingDown = statuses.Any(s => s.Direction == Direction.Down);

        movingUp.Should().BeTrue();
        movingDown.Should().BeFalse();
    }

    [Fact]
    public async Task ProcessElevatorRequestAsync_ShouldOpenDoorsAtEndOfJourney()
    {
        byte targetFloor = 2;
        var statuses = await _elevator.ProcessElevatorRequestAsync(targetFloor).ToArray();

        statuses.Last().DoorState.Should().Be(DoorState.Open);
    }
}