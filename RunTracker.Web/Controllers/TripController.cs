using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Runtracker.Domain.Dal.Model;
using Runtracker.Domain.Dal.Repository;
using RunTracker.DtoModel;
using RunTracker.Web.DtoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTracker.Controllers
{
    public class TripController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private TripRepository _tripRepository;
        private TripTickRepository _tripTickRepository;

        public TripController(IConfiguration configuration)
        {
            _configuration = configuration;
            _tripRepository = new TripRepository(_configuration);
            _tripTickRepository = new TripTickRepository(_configuration);
        }

        [HttpGet]
        [Route("/trip/opentrips")]
        public async Task<List<TripDto>> GetOpenTrips()
        {
            List<TripDto> results = new List<TripDto>();
            long userId = GetUserId();

            List<Trip> trips = await _tripRepository.GetOpenTripsByUserId(userId);

            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Trip, TripDto>();
            });

            IMapper mapper = config.CreateMapper();

            foreach (Trip trip in trips)
            {
                results.Add(mapper.Map<Trip, TripDto>(trip));
            }

            return results;

        }


        [HttpPost]
        [Route("/trip/create")]
        public async Task<TripDto> CreateTrip([FromBody]CreateTripDto createTripDto)
        {
            TripDto result = null;
            long userId = GetUserId();

            Trip trip = await _tripRepository.Create(userId, createTripDto.ActivityTypeId);

            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Trip, TripDto>();
            });

            IMapper mapper = config.CreateMapper();
            result = mapper.Map<Trip, TripDto>(trip);

            return result;
        }

        [HttpGet]
        [Route("/trip/{tripGuid}/extended")]
        public async Task<TripExtendedDto> GetTripExtendedByGuid(Guid tripGuid)
        {
            TripExtendedDto result = null;
            long userId = GetUserId();
            TripExtended  trip = await _tripRepository.GetTripExtendedByGuidAndUserId(tripGuid, userId);
            if(trip != null)
            {
                MapperConfiguration config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<TripExtended, TripExtendedDto>();
                });
                IMapper mapper = config.CreateMapper();

                result = mapper.Map<TripExtended, TripExtendedDto>(trip);
            }

            return result;
        }

        [HttpGet]
        [Route("/trip/{tripGuid}/close")]
        public async Task<TripDto> CloseTrip(Guid tripGuid)
        {
            TripDto result = null;
            long userId = GetUserId();
            
            var trip = await _tripRepository.GetByGuidAndUserIdOrThrowException(tripGuid, userId);
            if (trip == null)
            {
                throw new Exception("No trip for user exists with the provided trip-guid.");
            }
            if(trip.TimeEnd == null)
            {
                trip = await _tripRepository.CloseTrip(trip);

            }

            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Trip, TripDto>();
            });

            IMapper mapper = config.CreateMapper();
            result = mapper.Map<Trip, TripDto>(trip);

            return result;
        }

        [HttpPost]
        [Route("/trip/{tripGuid}/ticks/add")]
        public async Task<OkResult> AddTripTick(Guid tripGuid, [FromBody]AddTripTickDto model)
        {
            long userId = GetUserId();
            var trip = await _tripRepository.GetByGuidAndUserIdOrThrowException(tripGuid, userId);
            if (trip == null)
            {
                throw new Exception("No trip for user exists with the provided trip-guid.");
            }
            if (trip.TimeEnd != null)
            {
                throw new Exception("Trip is ended. Cannot add new trip-ticks to this trip.");
            }

            DateTime tickTime = DateTime.Now;
            List<TripTick> existingTicks = await _tripTickRepository.GetByTripId(trip.Id);
            int newTickOrder = existingTicks.Any()
                                ? existingTicks.Select(x => x.Order).Max() + 1
                                : 0;

            TripTick newTick = await _tripTickRepository.AddTripTick(trip.Id, newTickOrder, model.Lat, model.Long, model.Altitude, tickTime);
            existingTicks.Add(newTick);

            return Ok();
        }

        /// <summary>
        /// TODO: Sign-in and userId
        /// </summary>
        /// <returns></returns>
        private long GetUserId()
        {
            long userId = 1;
            return userId;
        }
    }
}
