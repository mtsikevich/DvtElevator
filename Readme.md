## DVT Elevator Challenge

*Preview*
![dvt elevator](https://github.com/mtsikevich/DvtElevator/assets/29281731/5d455bbb-983b-49a1-b740-867eee8e26fe)

## How to start?

When you first launch the console app, you will be prompted to enter the number of floors the fictional building will have, and another prompt for building elevator count.

The inputs to the prompts will be used to create the service, and then start the REPL loop.

## How to use?

You will be prompted to enter the target floor:

> The floor you pick should not be greater than the number of building floors.

You will then be prompted to enter the number of waiting passengers

> The number passengers should be at least one for the input to be accepted.

Both prompts will not accept any negative inputs.

After submitting the prompt inputs, the elevator will return statuses asynchronously using an observable on a secondary thread, making it possible to see the elevator as it goes from floor to floor until it reaches the target floor.
The primary thread will wait for your key input, which will terminate the process if the pressed key is 'q'.

The console app will loop until you press the 'q' key, or close it via the red 'X' in the top right corner.

---

*Note*: The console tested on Linux Mint LMDE 6 "Faye"
