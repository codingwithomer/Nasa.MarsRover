# Changelog

Changes on `refactor/hardening-and-cleanup` relative to `master`. Everything that changes observable behavior is listed;
the sample mission still produces exactly the output in the README.

## Behavior changes

### Fixed
- **Rover collisions were never detected.** `Position` was a class compared by reference, so `IsPositionFree` was always
  true and two rovers could share a square. `Position` is now an immutable value type.
- `ArgumentException` was constructed with its arguments swapped (message in `ParamName`) in every throw site.
- Parse errors reported 0-based line numbers that drifted when the input had blank lines or CRLF endings. They are now
  1-based and refer to the original text.
- The report used a hardcoded `\r\n` for rover lines but the platform newline elsewhere; it uses `Environment.NewLine`
  everywhere (the parser test failed on macOS/Linux because of this).
- Using a plateau before it was initialized threw `NullReferenceException`; a second initialization threw a bare
  `System.Exception`.

### Stricter
- The first line of the input must be the plateau (a missing plateau used to fail only at execution time).
- Instructions before any rover line are rejected at parse time (they used to crash at execution time).
- Whitespace-only input is rejected as empty (previously only `null`/`""`).
- Only ASCII digits start a coordinate; coordinates are parsed culture-invariantly without sign or decimal separators.
- `Deploy` rejects an undefined `CompassDirection` value; it used to be accepted and silently normalized.

### More tolerant
- Repeated spaces and tabs between fields are accepted (`1  2   N`).

### Exceptions
- Moving off the plateau or onto an occupied square throws `InvalidOperationException` (was `ArgumentException` with a
  swapped message).
- Deploying outside the plateau throws `ArgumentOutOfRangeException` (param `position`); deploying onto an occupied
  square throws `InvalidOperationException`.
- Error messages: unknown instructions name the character, the line and the allowed letters; `Position` prints as `(x, y)`.

### Console
- A mission can be read from a file argument or from redirected standard input; with neither, the built-in sample runs.
- Invalid input prints a message on stderr and exits with `1` (unreadable input: `2`) instead of an unhandled exception.
- `Console.ReadKey()` is skipped when input is redirected.

## Structure and API (breaking for any code that referenced the old types)
- Target framework `netcoreapp2.1` -> `net10.0`; packages updated; unused `Microsoft.Extensions.Configuration*` removed;
  warnings are errors (`Directory.Build.props`).
- Projects renamed: `Entities` -> `Domain`, `BLL` -> `Application`; namespaces follow.
- `Plateau` takes its size in the constructor and creates rovers via `Deploy`; `ILocation`, `IPlateau` and `IRover` are
  gone (`ITerrain` stays). `MarsRover` no longer formats text and has an internal constructor.
- `ICommand.Execute(MissionContext)` returns `void`; the print command is gone and the report is built from the plateau.
  `LocationInitializeCommand` -> `DefinePlateauCommand`, `RoverCreationCommand` -> `DeployRoverCommand`.
- `ICommandCenter`/`CommandCenter` -> `IMissionExecutor`/`MissionExecutor` (`ExecuteCommands` -> `Execute`); input and
  report headers are no longer its job.
- `CommandParser` takes `IEnumerable<ILineParser>` and returns `IReadOnlyList<ICommand>`; instruction letters come from
  `IInstructionSet`.
- The static `Ioc` service locator is replaced by `AddMarsRover()` and constructor injection.
- `Rotation` and `CompassDirection` have no numeric values; the N/E/S/W text mapping lives in Application.
