using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using System.Security.Claims;
using Domain.Models;
using Domain.Repositories;

namespace Application.Services
{
    public class HistoriesService : IHistoriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IRequester _requester;

        private readonly ClaimsPrincipal _claimsPrincipal;

        private readonly IHistoriesSearchRepository _historiesSearchRepository;

        private long CurrentAccountId => long.Parse(
            _claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        private IList<Role> CurrentRoles => _claimsPrincipal.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => Enum.Parse<Role>(c.Value))
            .ToList();

        public HistoriesService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IRequester requester, 
            ClaimsPrincipal claimsPrincipal,
            IHistoriesSearchRepository historiesSearchRepository)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;

            _requester = requester;

            _claimsPrincipal = claimsPrincipal;

            _historiesSearchRepository = historiesSearchRepository;
        }

        public async Task<HistoryDto> CreateHistoryAsync(CreateHistoryDto dto)
        {
            AccountExistanceResult patientExistanceResult = await _requester
                .GetResponse<CheckAccountExistance, AccountExistanceResult>(new CheckAccountExistance(dto.PacientId));

            if (!patientExistanceResult.Roles.Contains(Role.User))
            {
                throw new NotFoundException("history.patient_not_found");
            }

            AccountExistanceResult doctorExistanceResult = await _requester
                .GetResponse<CheckAccountExistance, AccountExistanceResult>(new CheckAccountExistance(dto.DoctorId));

            if (!doctorExistanceResult.Roles.Contains(Role.Doctor))
            {
                throw new NotFoundException("history.doctor_not_found");
            }

            HospitalExistanceResult hospitalExistanceResult = await _requester
                .GetResponse<CheckHospitalExistance, HospitalExistanceResult>(new CheckHospitalExistance(dto.HospitalId));

            if (!hospitalExistanceResult.Rooms.Contains(dto.Room))
            {
                throw new NotFoundException("history.hospital_or_room_not_found");
            }

            History history = _mapper.Map<History>(dto);

            _unitOfWork.Histories.Add(history);

            await _unitOfWork.SaveChangesAsync();

            await _historiesSearchRepository.CreateAsync(history);

            return _mapper.Map<HistoryDto>(history);
        }

        public async Task<IEnumerable<HistoryDto>> GetHistoriesListByAccountIdAsync(long accountId)
        {
            if (CurrentRoles.Contains(Role.User)
                && accountId != CurrentAccountId)
            {
                return [];
            }

            IList<History> histories = await _historiesSearchRepository.GetListByAccountIdAsync(accountId);

            return _mapper.Map<IEnumerable<HistoryDto>>(histories);
        }

        public async Task<HistoryDto> GetHistoryByIdAsync(long id)
        {
            History? history = await _historiesSearchRepository.GetByIdAsync(id);

            if (history == null
                || CurrentRoles.Contains(Role.User)
                && history.PacientId != CurrentAccountId)
            {
                throw new NotFoundException("history.not_found");
            }

            return _mapper.Map<HistoryDto>(history);
        }

        public async Task UpdateHistoryByIdAsync(long id, UpdateHistoryDto dto)
        {
            AccountExistanceResult patientExistanceResult = await _requester
                .GetResponse<CheckAccountExistance, AccountExistanceResult>(new CheckAccountExistance(dto.PacientId));

            if (!patientExistanceResult.Roles.Contains(Role.User))
            {
                throw new NotFoundException("history.patient_not_found");
            }

            AccountExistanceResult doctorExistanceResult = await _requester
                .GetResponse<CheckAccountExistance, AccountExistanceResult>(new CheckAccountExistance(dto.DoctorId));

            if (!doctorExistanceResult.Roles.Contains(Role.Doctor))
            {
                throw new NotFoundException("history.doctor_not_found");
            }

            HospitalExistanceResult hospitalExistanceResult = await _requester
                .GetResponse<CheckHospitalExistance, HospitalExistanceResult>(new CheckHospitalExistance(dto.HospitalId));

            if (!hospitalExistanceResult.Rooms.Contains(dto.Room))
            {
                throw new NotFoundException("history.hospital_or_room_not_found");
            }

            History? history = await _unitOfWork.Histories.GetByIdAsync(id);

            if (history == null)
            {
                throw new NotFoundException("history.not_found");
            }

            _mapper.Map(dto, history);

            await _unitOfWork.SaveChangesAsync();

            await _historiesSearchRepository.UpdateAsync(id, history);
        }
    }
}
