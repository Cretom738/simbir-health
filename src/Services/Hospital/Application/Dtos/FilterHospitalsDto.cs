using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class FilterHospitalsDto
    {
        [Range(0, int.MaxValue)]
        public int From { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int Count { get; set; } = int.MaxValue;
    }
}
