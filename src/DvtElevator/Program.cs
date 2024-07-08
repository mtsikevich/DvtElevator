using DvtElevator.ConsoleServices;

var service = Prompts.CreateElevatorService();
ViewSnippets.DisplayHeading();

do
{
    AnsiConsole.Write(new Rule());
    var requestedFloor = Prompts.FloorRequestPrompt(service);
    var numberOfPeople = Prompts.NumberOfWaitingPeoplePrompt();
    
    var k = await ViewSnippets.ViewRealtimeElevatorUpdatesAsync(service, requestedFloor, numberOfPeople);
    
    if(k.Key == ConsoleKey.Q)
        break;
    
} while (true);