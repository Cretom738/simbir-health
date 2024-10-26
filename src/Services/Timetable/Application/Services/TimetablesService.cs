using Application.Dtos;
using Application.Helpers;
using AutoMapper;
using Domain;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class TimetablesService : ITimetablesService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IRequester _requester;

        public TimetablesService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IRequester requester)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;

            _requester = requester;
        }

        public async Task<TimetableDto> CreateTimetableAsync(CreateTimetableDto dto)
        {
            CheckIfTimePeriodValid(dto.From, dto.To);

            AccountExistanceResult accountExistanceResult = await _requester
                .GetResponse<CheckAccountExistance, AccountExistanceResult>(new CheckAccountExistance(dto.DoctorId));

            if (!accountExistanceResult.Roles.Contains(Role.Doctor))
            {
                throw new NotFoundException("timetable.doctor_not_found");
            }

            HospitalExistanceResult hospitalExistanceResult = await _requester
                .GetResponse<CheckHospitalExistance, HospitalExistanceResult>(new CheckHospitalExistance(dto.HospitalId));

            if (!hospitalExistanceResult.Rooms.Contains(dto.Room))
            {
                throw new NotFoundException("timetable.hospital_or_room_not_found");
            }

            Timetable timetable = _mapper.Map<Timetable>(dto);

            _unitOfWork.Timetables.Add(timetable);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TimetableDto>(timetable);
        }

        public async Task<IEnumerable<TimetableDto>> GetTimetablesListByDoctorIdAsync(long doctorId, FilterTimetablesDto dto)
        {
            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByDoctorIdAsync(doctorId, dto.From, dto.To);

            return _mapper.Map<IEnumerable<TimetableDto>>(timetables);
        }

        public async Task<IEnumerable<TimetableDto>> GetTimetablesListByHospitalIdAsync(long hospitalId, FilterTimetablesDto dto)
        {
            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByHospitalIdAsync(hospitalId, dto.From, dto.To);

            return _mapper.Map<IEnumerable<TimetableDto>>(timetables);
        }

        public async Task<IEnumerable<TimetableDto>> GetTimetablesListByHospitalRoomAsync(
            long hospitalId, string room, FilterTimetablesDto dto)
        {
            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByHospitalRoomAsync(hospitalId, room, dto.From, dto.To);

            return _mapper.Map<IEnumerable<TimetableDto>>(timetables);
        }

        public async Task UpdateTimetableByIdAsync(long id, UpdateTimetableDto dto)
        {
            CheckIfTimePeriodValid(dto.From, dto.To);

            AccountExistanceResult accountExistanceResult = await _requester
                .GetResponse<CheckAccountExistance, AccountExistanceResult>(new CheckAccountExistance(dto.DoctorId));

            if (!accountExistanceResult.Roles.Contains(Role.Doctor))
            {
                throw new NotFoundException("timetable.doctor_not_found");
            }

            HospitalExistanceResult hospitalExistanceResult = await _requester
                .GetResponse<CheckHospitalExistance, HospitalExistanceResult>(new CheckHospitalExistance(dto.HospitalId));

            if (!hospitalExistanceResult.Rooms.Contains(dto.Room))
            {
                throw new NotFoundException("timetable.hospital_or_room_not_found");
            }

            Timetable? timetable = await _unitOfWork.Timetables.GetByIdWithAppointmentsAsync(id);

            if (timetable == null)
            {
                throw new NotFoundException("timetable.not_found");
            }

            if (timetable.Appointments.Count != 0)
            {
                throw new ConflictException("timetable.has_reserved_appointments");
            }

            _mapper.Map(dto, timetable);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTimetablesByDoctorIdAsync(long doctorId)
        {
            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByDoctorIdAsync(doctorId);

            foreach (Timetable timetable in timetables)
            {
                _unitOfWork.Timetables.Remove(timetable);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTimetablesByHospitalIdAsync(long hospitalId)
        {
            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByHospitalIdAsync(hospitalId);

            foreach (Timetable timetable in timetables)
            {
                _unitOfWork.Timetables.Remove(timetable);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTimetableByIdAsync(long id)
        {
            Timetable? timetable = await _unitOfWork.Timetables.GetByIdAsync(id);

            if (timetable == null)
            {
                throw new NotFoundException("timetable.not_found");
            }

            _unitOfWork.Timetables.Remove(timetable);

            await _unitOfWork.SaveChangesAsync();
        }

        private void CheckIfTimePeriodValid(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new BadRequestException("timetable.from_more_than_to");
            }

            if ((to - from) > TimeSpan.FromHours(12))
            {
                throw new BadRequestException("timetable.time_period_more_than_twelve_hours");
            }

            if (!DateTimeRoundingHelper.IsRounded(from)
                || !DateTimeRoundingHelper.IsRounded(to))
            {
                throw new BadRequestException("timetable.wrong_minutes_or_seconds");
            }
        }
    }
}
