namespace Application.Dtos
{
    public class FilterTimetablesDto
    {
        public DateTime From { get; set; } = DateTime.MinValue;

        public DateTime To { get; set; } = DateTime.MaxValue;
    }
}
