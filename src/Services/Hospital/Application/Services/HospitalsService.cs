using Application.Dtos;
using AutoMapper;
using Domain;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class HospitalsService : IHospitalsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IPublisher _publisher;

        public HospitalsService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IPublisher publisher)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;

            _publisher = publisher;
        }

        public async Task<HospitalDto> CreateHospitalAsync(CreateHospitalDto dto)
        {
            Hospital newHospital = _mapper.Map<Hospital>(dto);

            _unitOfWork.Hospitals.Add(newHospital);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<HospitalDto>(newHospital);
        }

        public async Task<IEnumerable<HospitalDto>> GetHospitalsListAsync(FilterHospitalsDto dto)
        {
            IEnumerable<Hospital> hospitals = await _unitOfWork.Hospitals.GetListAsync(dto.From, dto.Count);

            return _mapper.Map<IEnumerable<HospitalDto>>(hospitals);
        }

        public async Task<HospitalDto> GetHospitalByIdAsync(long id)
        {
            Hospital? hospital = await _unitOfWork.Hospitals.GetByIdAsync(id);

            if (hospital == null)
            {
                throw new NotFoundException("hospital.not_found");
            }

            return _mapper.Map<HospitalDto>(hospital);
        }

        public async Task<IEnumerable<string>> GetHospitalRoomsListAsync(long id)
        {
            Hospital? hospital = await _unitOfWork.Hospitals.GetByIdAsync(id);

            if (hospital == null)
            {
                throw new NotFoundException("hospital.not_found");
            }

            return hospital.Rooms;
        }

        public async Task UpdateHospitalByIdAsync(long id, UpdateHospitalDto dto)
        {
            Hospital? hospital = await _unitOfWork.Hospitals.GetByIdAsync(id);

            if (hospital == null)
            {
                throw new NotFoundException("hospital.not_found");
            }

            _mapper.Map(dto, hospital);

            await _unitOfWork.SaveChangesAsync();

            await _publisher.Publish(new HospitalUpdated(id, hospital.Rooms));
        }

        public async Task DeleteHospitalByIdAsync(long id)
        {
            Hospital? hospital = await _unitOfWork.Hospitals.GetByIdAsync(id);

            if (hospital == null)
            {
                throw new NotFoundException("hospital.not_found");
            }

            _unitOfWork.Hospitals.Remove(hospital);

            await _unitOfWork.SaveChangesAsync();

            await _publisher.Publish(new HospitalDeleted(id));
        }
    }
}
