# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 | src/RoomHub.Api/Services/BookingService.cs | reading the files | Ar trebui injectat TimeProvider si facut un check pentru endUtc>GetUtcNow() si startUtc>GetUtcNow() |
| R6 | src/RoomHub.Api/Services/BookingService.cs | reading the files | Ar trebui verificat ca endUtc-startUtc nu depaseste 4 ore |
