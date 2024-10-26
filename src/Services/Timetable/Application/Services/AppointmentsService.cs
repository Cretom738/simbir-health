using Application.Dtos;
using Application.Helpers;
using AutoMapper;
using Domain;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using System.Security.Claims;

namespace Application.Services
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly ClaimsPrincipal _claimsPrincipal;

        private long CurrentAccountId => long.Parse(
            _claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        private IList<Role> CurrentRoles => _claimsPrincipal.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => Enum.Parse<Role>(c.Value))
            .ToList();

        public AppointmentsService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ClaimsPrincipal claimsPrincipal)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;

            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(long timetableId, CreateAppointmentDto dto)
        {
            if (!DateTimeRoundingHelper.IsRounded(dto.Time))
            {
                throw new BadRequestException("appointment.wrong_minutes_or_seconds");
            }

            Timetable? timetable = await _unitOfWork.Timetables.GetByIdWithAppointmentsAsync(timetableId);

            if (timetable == null)
            {
                throw new NotFoundException("timetable.not_found");
            }

            if (timetable.From > dto.Time || dto.Time > timetable.To)
            {
                throw new BadRequestException("appointment.out_of_timetable_range");
            }

            if (timetable.Appointments.Any(a => a.Time == dto.Time))
            {
                throw new ConflictException("appointment.already_reserved");
            }

            Appointment appointment = new Appointment
            {
                Time = dto.Time,
                TimetableId = timetableId,
                PatientId = CurrentAccountId
            };

            _unitOfWork.Appointments.Add(appointment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<DateTime>> GetAvaliableAppointmentsListByAsync(long timetableId)
        {
            Timetable? timetable = await _unitOfWork.Timetables.GetByIdWithAppointmentsAsync(timetableId);

            if (timetable == null)
            {
                throw new NotFoundException("timetable.not_found");
            }

            DateTime currentAppointmentDateTime = timetable.From;

            IList<DateTime> avaliableApointments = [currentAppointmentDateTime];

            while (currentAppointmentDateTime < timetable.To)
            {
                currentAppointmentDateTime = currentAppointmentDateTime.AddMinutes(30);

                avaliableApointments.Add(currentAppointmentDateTime);
            }

            IList<DateTime> reservedAppointments = timetable.Appointments
                .Select(t => t.Time)
                .ToList();

            return avaliableApointments
                .Except(reservedAppointments)
                .ToList();
        }

        public async Task DeleteAppointmentByIdAsync(long id)
        {
            Appointment? appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null
                || !CurrentRoles.Contains(Role.Admin)
                && !CurrentRoles.Contains(Role.Manager)
                && appointment.PatientId != CurrentAccountId)
            {
                throw new NotFoundException("appointment.not_found");
            }

            _unitOfWork.Appointments.Remove(appointment);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
