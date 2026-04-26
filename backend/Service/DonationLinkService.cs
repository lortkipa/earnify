using AutoMapper;
using Dal;
using Data.Entities;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IDonationLinkService
    {
        Task<IEnumerable<DonationLinkDTO>> GetByCreatorIdAsync(int id);
        Task<DonationLinkDTO> CreateAsync(CreateDonationLinkDTO model);
    }

    public class DonationLinkService : IDonationLinkService
    {
        private readonly IDonationLinkRepository _donationLinkRepository;
        private readonly IMapper _mapper;

        public DonationLinkService(IDonationLinkRepository donationLinkRepository, IMapper mapper)
        {
            _donationLinkRepository = donationLinkRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DonationLinkDTO>> GetByCreatorIdAsync(int creatorId)
        {
            var entities = await _donationLinkRepository.GetByCreatorId(creatorId);
            return _mapper.Map<IEnumerable<DonationLinkDTO>>(entities);
        }

        public async Task<DonationLinkDTO> CreateAsync(CreateDonationLinkDTO model)
        {
            var entity = _mapper.Map<DonationLink>(model);
            entity = await _donationLinkRepository.AddAsync(entity);
            return _mapper.Map<DonationLinkDTO>(entity);
        }
    }
}
