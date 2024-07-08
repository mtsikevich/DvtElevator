## DVT Elevator Challenge

*Preview*
![dvt elevator](https://github.com/mtsikevich/DvtElevator/assets/29281731/674f31ba-415e-44d9-8059-b34c7adf6a02)
---
## How to start?
When you first launch the console app, you will be prompted to enter the number of floors the fictional building will have, and then start the REPL loop.

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


