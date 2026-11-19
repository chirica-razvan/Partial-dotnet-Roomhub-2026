using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Booking.NotFound", "Rezervarea nu a fost găsită.");

    public static readonly Error BookedByRequired = Error.Validation(
        "Booking.BookedByRequired", "Câmpul bookedBy este obligatoriu.");

    public static readonly Error EndBeforeStart = Error.Validation(
        "Booking.EndBeforeStart", "Sfârșitul rezervării trebuie să fie după început.");

    public static readonly Error InvalidAttendees = Error.Validation(
        "Booking.InvalidAttendees", "Numărul de participanți trebuie să fie strict pozitiv.");

    public static readonly Error AttendeesExceedCapacity = Error.Validation(
        "Booking.AttendeesExceedCapacity", "Sala nu are suficiente locuri.");

    public static readonly Error StartInPast = Error.Validation(
        "Booking.StartInPast", "Rezervarea nu poate începe în trecut.");

    public static readonly Error TooLong = Error.Validation(
        "Booking.TooLong", "O rezervare durează cel mult 4 ore.");

    public static readonly Error TooLongForLargeRoom = Error.Validation(
        "Booking.TooLongForLargeRoom", "Sălile cu peste 10 locuri se rezervă cel mult 2 ore.");

    public static readonly Error RoomOccupied = Error.Conflict(
        "Booking.RoomOccupied", "Sala este ocupată în acest interval.");

    public static readonly Error DailyLimitReached = Error.Conflict(
        "Booking.DailyLimitReached", "Un participant poate avea cel mult 2 rezervări active pe zi.");

    public static readonly Error AlreadyCancelled = Error.Conflict(
        "Booking.AlreadyCancelled", "Rezervarea este deja anulată.");

    public static readonly Error TooLateToCancel = Error.Conflict(
        "Booking.TooLateToCancel", "Rezervarea se poate anula doar cu cel puțin 60 de minute înainte de început.");
}
