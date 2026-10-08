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
- A rover without an instruction line (followed by another rover or by the end of the input) is rejected at parse time
  ("The rover on line N has no instructions"). A mission with only a plateau is still accepted.
- Lowercase headings and instructions are still rejected, but the message now says letters must be upper case.
- Instruction lines containing spaces (`M M`) are rejected with a message saying so.
- A line starting with `-` or `+` followed by a digit is a malformed coordinate line, not an unknown instruction.
- The first line of the input must be the plateau (a missing plateau used to fail only at execution time).
- Instructions before any rover line are rejected at parse time (they used to crash at execution time).
- Whitespace-only input is rejected as empty (previously only `null`/`""`).
- Only ASCII digits start a coordinate; coordinates are parsed culture-invariantly without sign or decimal separators.
- `Deploy` rejects an undefined `CompassDirection` value; it used to be accepted and silently normalized.

### More tolerant
- Whitespace-only lines are skipped (master threw "Empty line not allowed" for them).
- A UTF-8 byte order mark at the start of standard input is ignored, as it already was for files.
- Lone `\r` line endings are accepted.
- Repeated spaces and tabs between fields are accepted (`1  2   N`).

### Exceptions
- Moving off the plateau or onto an occupied square throws `InvalidOperationException` (was `ArgumentException` with a
  swapped message).
- Deploying outside the plateau throws `ArgumentOutOfRangeException` (param `position`); deploying onto an occupied
  square throws `InvalidOperationException`.
- Error messages: unknown instructions name the character, the line and the allowed letters; `Position` prints as `(x, y)`.

### Messages
- Malformed lines have specific messages instead of "Unknown instruction": the plateau must be "two non-negative whole
  numbers (X Y)", a rover position needs "X Y and a heading", a number beyond 2147483647 "is too large", spaces inside
  instructions are not allowed. All name the 1-based line.
- Execution errors name the rover and the line, e.g. `Rover 2 (line 4): (2, 6) is outside the plateau; it cannot move
  North from (2, 5).` The ".NET (Parameter 'x')" and "Actual value was" text never reaches the user.
- Moving next to `int.MaxValue` fails with "outside the plateau" instead of wrapping around to a negative coordinate.

### Console
- A mission can be read from a file argument or from redirected standard input; with neither, the built-in sample runs.
- Redirected standard input that is empty now exits with `1` ("Empty input.") instead of running the sample.
- Only the built-in sample is decorated with `Test Input:` / `Expected Output:`. Missions from a file or stdin print
  just the report on stdout. The sample's output is unchanged.
- Invalid input prints a message on stderr and exits with `1` (unreadable input: `2`) instead of an unhandled exception.
  Only `InvalidMissionException` is reported as an invalid mission; programming errors are no longer disguised as one.
- When a rover fails, the report lines of the rovers that finished are still printed on stdout (exit code `1`).
- An empty file name and a directory exit with `2` and say so; the argument is checked before anything is built.
- `--help` / `-h` print the usage on stdout and exit `0`.
- `Console.ReadKey()` is only called after a successful interactive run of the built-in sample (interactive runs still
  wait for a key); not after errors, `--help`, file runs or redirected input.

## Structure and API (breaking for any code that referenced the old types)
- New `InvalidMissionException` (Application) replaces `ArgumentException` for parse errors; it carries the
  `PartialReport`. Execution wraps domain exceptions into it. The domain exception types are unchanged.
- `AddMarsRover()` no longer registers an `IMissionInputProvider`; the host registers exactly one.
- Commands are internal (visible to the test project); `RoverRotatorCommand` is now `RotateRoverCommand`.
  `MissionContext.Plateau` is internal. `Plateau.Rovers` is a read-only view. `InstructionSet.With` reports a
  duplicate letter with a clear `ArgumentException`.
- `InputLine` has an `IsFirst` flag: the first non-blank line is the plateau by position.
- `MissionRunner.Run()` returns the plain report; the sample decoration is done by the console.
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
