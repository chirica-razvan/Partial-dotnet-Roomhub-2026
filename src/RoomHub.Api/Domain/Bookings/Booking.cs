using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

// Rich domain: rules R2–R8, R10, R11 live here. The current time comes in as a parameter (from TimeProvider).
public sealed class Booking
{
    public const int LargeRoomCapacity = 10;
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);
    public static readonly TimeSpan MaxDurationLargeRoom = TimeSpan.FromHours(2);
    public static readonly TimeSpan MinCancelNotice = TimeSpan.FromMinutes(60);

    private Booking()
    {
    }

    public Guid Id { get; private set; }

    public Guid RoomId { get; private set; }

    public string BookedBy { get; private set; } = string.Empty;

    public DateTime StartUtc { get; private set; }

    public DateTime EndUtc { get; private set; }

    public int Attendees { get; private set; }

    public BookingStatus Status { get; private set; }

    public static Result<Booking> Create(
        Guid roomId,
        int roomCapacity,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        Guid? id = null) // fixed only in tests (hard-coded GUIDs); otherwise generated
    {
        var error = Validate(roomCapacity, bookedBy, startUtc, endUtc, attendees, nowUtc);
        if (error is not null)
            return error;

        return new Booking
        {
            Id = id ?? Guid.NewGuid(),
            RoomId = roomId,
            BookedBy = bookedBy!.Trim(),
            StartUtc = startUtc,
            EndUtc = endUtc,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // R7/R8: only active bookings collide; half-open interval (10-12 and 12-14 do not overlap).
    public bool Overlaps(Booking other) =>
        Status == BookingStatus.Active
        && other.Status == BookingStatus.Active
        && StartUtc < other.EndUtc
        && other.StartUtc < EndUtc;

    // R8 + R11
    public Result<Unit> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        if (StartUtc - nowUtc < MinCancelNotice)
            return BookingErrors.TooLateToCancel;

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }

    // The first error found, or null.
    private static Error? Validate(
        int roomCapacity,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc)
    {
        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.BookedByRequired;

        // R3
        if (endUtc <= startUtc)
            return BookingErrors.EndBeforeStart;

        // R4
        if (attendees <= 0)
            return BookingErrors.InvalidAttendees;

        if (attendees > roomCapacity)
            return BookingErrors.AttendeesExceedCapacity;

        // R5
        if (startUtc < nowUtc)
            return BookingErrors.StartInPast;

        // R6 + R10: large rooms (> 10 seats) have a 2-hour limit, the others 4 hours
        if (roomCapacity > LargeRoomCapacity)
        {
            if (endUtc - startUtc > MaxDurationLargeRoom)
                return BookingErrors.TooLongForLargeRoom;
        }
        else if (endUtc - startUtc > MaxDuration)
        {
            return BookingErrors.TooLong;
        }

        return null;
    }
}
