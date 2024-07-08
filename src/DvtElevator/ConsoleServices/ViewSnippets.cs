namespace DvtElevator.ConsoleServices;

/// <summary>
/// Contains the view snippets to display on the console window 
/// </summary>
public static class ViewSnippets
{
    /// <summary>
    /// Displays the elevator status as it goes up or down the building
    /// </summary>
    /// <param name="elevatorService">The elevator service containing the elevators</param>
    /// <param name="targetFloor">The floor for which the elevator is being requested</param>
    /// <param name="numberOfPeople1">The number of people waiting for the elevator</param>
    /// <returns>The key that the user decided to press</returns>
    public static async Task<ConsoleKeyInfo> ViewRealtimeElevatorUpdatesAsync(ElevatorService elevatorService, byte targetFloor, byte numberOfPeople1)
    {
        ConsoleKeyInfo consoleKeyInfo = default;
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Hamburger)
            .SpinnerStyle(Style.Parse("gold3"))
            .StartAsync("Loading elevator status...", async ctx =>
            {
                try
                {
                    var elevator = await elevatorService.PickOptimalElevatorAsync(targetFloor, numberOfPeople1);
                    AnsiConsole.WriteLine($"Go wait for elevator {elevator!.Number}");
            
                    var elevatorObservable = elevator.ProcessElevatorRequestAsync(targetFloor);

                    using (elevatorObservable.Subscribe(es =>
                               {
                                   ctx.Status($"Elevator on floor [b purple]{es.CurrentFloor}[/]");
                               },
                               exception => { },
                               (() => { ctx.Status("Elevator arrived");})
                           ))
                    {
                        AnsiConsole.WriteLine("Press any key to continue, q to quit");
                        consoleKeyInfo = Console.ReadKey();
                    }
                }
                catch(BuildingHasNoElevators)
                {
                    Console.WriteLine("The building has not elevators.");
                }
            });
        return consoleKeyInfo;
    }
    
    /// <summary>
    /// Displays the application headings on the console window
    /// </summary>
    public static void DisplayHeading()
    {
        AnsiConsole.Write(new Markup( "[u]Writer[/]\n[i b]Tumelo Motsikelane[/]\n\n[b gold3]DVT[/]").Centered());
        AnsiConsole.Write(new FigletText("Elevator")
            .Centered()
            .Color(Color.Blue));
    }
}