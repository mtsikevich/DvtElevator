using DvtElevator.Models.Elevators;
using DvtElevator.Services.ElevatorDispatcher;

namespace DvtElevator.ConsoleServices;

public static class Prompts
{
    /// <summary>
    /// Function to build the ElevatorService:
    /// Prompts the number of floors the fictional/imaginary building has
    /// </summary>
    /// <returns>Returns the Created ElevatorService</returns>
    public static ElevatorService CreateElevatorService()
    {
        var buildingFloorCount = NumberOfFloorsPrompt();
        var buildingElevatorCount = NumberOfElevatorsPrompt();
        
        IElevatorDispatcher dispatcherService = new ElevatorDispatcher();
        var elevatorService = new ElevatorService(dispatcherService,buildingFloorCount);

        foreach (var _ in Enumerable.Range(1,buildingElevatorCount))
        {
            try
            {
                elevatorService.AddElevator(new PeopleElevator(Constants.MaxWeightForPeopleElevator,buildingFloorCount));
            }
            catch (ElevatorExceedsBuildingFloors)
            {
                Console.WriteLine("The elevator has access to more floors that the building has.\nYou will have to restart the app with a fixed number");
            }
        }
            
        return elevatorService;
    }

    /// <summary>
    /// Prompt function for getting the number of floors for the fictional/imaginary building
    /// </summary>
    /// <returns>Returns the number of floors for the building captured through the prompt</returns>
    private static byte NumberOfFloorsPrompt()
    {
        return (byte)TextPrompt("Number of floors for the building: ", (Func<int, ValidationResult>?)NumberOfFloorInElevatorValidation);
        
        ValidationResult NumberOfFloorInElevatorValidation(int numberOfPeole)
        {
            return numberOfPeole switch
            {
                < 0 => ValidationResult.Error("[red]You cannot have < 1 floor(s)[/]"),
                > 100 => ValidationResult.Error($"[red]You cannot have more than 100 floors[/]"),
                _ => ValidationResult.Success()
            };
        }
    }
    
    /// <summary>
    /// Prompt function for getting the number of elevators for the fictional/imaginary building
    /// </summary>
    /// <returns>Returns the number of floors for the building captured through the prompt</returns>
    private static byte NumberOfElevatorsPrompt()
    {
        return (byte)TextPrompt("Building elevator count: ", (Func<int, ValidationResult>?)NumberOfElevatorsInTheBuildingValidation);
        
        ValidationResult NumberOfElevatorsInTheBuildingValidation(int numberOfElevators)
        {
            return numberOfElevators switch
            {
                < 0 => ValidationResult.Error("[red]You cannot have < 1 elevator(s)[/]"),
                > 10 => ValidationResult.Error($"[red]You cannot have more than 10 elevators[/]"),
                _ => ValidationResult.Success()
            };
        }
    }
    
    /// <summary>
    /// Prompt function for getting the floor
    /// </summary>
    /// <param name="elevatorService">The elevator service holding building data</param>
    /// <returns>Returns the floor number captured through the prompt</returns>
    public static byte FloorRequestPrompt(ElevatorService elevatorService)
    {
        return (byte)TextPrompt("Select Floor: ", (Func<int, ValidationResult>?)FloorSelectionValidation);
        
        ValidationResult FloorSelectionValidation(int floor)
        {
            return floor switch
            {
                < 0 => ValidationResult.Error("[red]Floor cannot be less than [/]"),
                _ when floor > elevatorService.BuildingFloorCount => ValidationResult.Error($"[red]You cannot request a floor beyond {elevatorService.BuildingFloorCount} (The last building floor)[/]"),
                _ => ValidationResult.Success()
            };
        }
    }
    
    /// <summary>
    /// Prompt function for getting the number of people waiting for the elevator
    /// </summary>
    /// <returns>Returns the number of waiting people captured from the prompt</returns>
    public static byte NumberOfWaitingPeoplePrompt()
    {
        return (byte)TextPrompt("Number of people waiting: ", (Func<int, ValidationResult>?)NumberOfPeopleInElevatorValidation);
        
        ValidationResult NumberOfPeopleInElevatorValidation(int numberOfPeole)
        {
            return numberOfPeole switch
            {
                < 0 => ValidationResult.Error("[red]You cannot request the elevator for < 1 person [/]"),
                > 10 => ValidationResult.Error($"[red]You cannot have more than 10 people in the elevator[/]"),
                _ => ValidationResult.Success()
            };
        }
    }

    
    /// <summary>
    /// Generic text prompt function 
    /// </summary>
    /// <param name="textPrompt">Prompt text</param>
    /// <param name="validationFunction">validation function</param>
    /// <typeparam name="T">Data Type of data being collected</typeparam>
    /// <returns>Returns text prompt object of type T</returns>
    private static T TextPrompt<T>(string textPrompt, Func<T,ValidationResult> validationFunction)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<T>(textPrompt)
                .ValidationErrorMessage("[red]Please enter a valid floor number[/]")
                .Validate(validationFunction));
    }
}