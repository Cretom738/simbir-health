using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class FilterDoctorAccountsDto
    {
        [MaxLength(200)]
        public string? NameFilter { get; set; }

        [Range(0, int.MaxValue)]
        public int From { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int Count { get; set; } = int.MaxValue;
    }
}
