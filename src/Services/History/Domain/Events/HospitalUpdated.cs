namespace Domain.Events
{
    public record HospitalUpdated(long HospitalId, IList<string> Rooms);
}
