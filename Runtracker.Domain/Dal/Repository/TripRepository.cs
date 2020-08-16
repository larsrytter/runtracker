using Npgsql;
using Runtracker.Domain.Dal.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Dapper;
using DapperExtensions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Runtracker.Domain.Dal.Repository
{
    public class TripRepository
    {
        private readonly IConfiguration _configuration;

        public TripRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        
        // TODO: Move to factory-class
        private IDbConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("PostgresConnection");
            IDbConnection db = new NpgsqlConnection(connectionString);
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
            DapperAsyncExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
            return db;
        }

        public async Task<Trip> Create(long userId, long activityTypeId)
        {
            Guid tripGuid = Guid.NewGuid();
            Trip trip = new Trip()
            {   
                TripGuid = tripGuid,
                ActivityTypeId = activityTypeId,
                TimeStart = DateTime.Now,
                TimeEnd = null,
                UserId = userId
            };
            
            using (IDbConnection db = GetConnection())
            {
                try
                {
                    long insertedTripId = await db.InsertAsync<Trip>(trip);
                    trip = await GetByIdOrThrowException(insertedTripId);
                }
                catch(Exception ex)
                {
                    throw;
                }
                
            }

            return trip;
        }

        public async Task<Trip> CloseTrip(Trip trip)
        {
            trip.TimeEnd = DateTime.Now;
            using (IDbConnection db = GetConnection())
            {
                bool isOk = await db.UpdateAsync<Trip>(trip);
                if(!isOk)
                {
                    throw new Exception("Error updating trip TimeEnd.");
                }
            }
            return trip;
        }

        public async Task<Trip> GetByIdOrThrowException(long tripId)
        {
            Trip trip;
            try
            {
                using (IDbConnection db = GetConnection())
                {
                    string sql = @"SELECT 
                                        id AS Id, 
                                        trip_guid AS TripGuid,
                                        user_id AS UserId,
                                        activity_type_id AS ActivityTypeId, 
                                        time_start AS TimeStart, 
                                        time_end AS TimeEnd 
                                    FROM 
                                        trip 
                                    WHERE 
                                        id = @id";

                    trip = await db.QueryFirstAsync<Trip>(sql, new { id = tripId });
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return trip;
        }

        public async Task<Trip> GetByGuidAndUserIdOrThrowException(Guid tripGuid, long userId)
        {
            Trip trip = null;
            using (IDbConnection db = GetConnection())
            {
                try
                {
                    string sql = @"SELECT 
                                    id AS Id, 
                                    trip_guid AS TripGuid,
                                    user_id AS UserId,
                                    activity_type_id AS ActivityTypeId, 
                                    time_start AS TimeStart, 
                                    time_end AS TimeEnd 
                                FROM 
                                    trip 
                                WHERE 
                                    trip_guid = @guid
                                    AND
                                    user_id = @userId";

                    trip = await db.QueryFirstAsync<Trip>(sql, new { guid = tripGuid, userId = userId });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return trip;
        }

        public async Task<List<Trip>> GetOpenTripsByUserId(long userId)
        {
            List<Trip> trips = new List<Trip>();

            using (IDbConnection db = GetConnection())
            {
                try
                {
                    string sql = @"SELECT 
                                        id AS Id, 
                                        trip_guid AS TripGuid,
                                        user_id as UserId,
                                        activity_type_id AS ActivityTypeId, 
                                        time_start AS TimeStart, 
                                        time_end AS TimeEnd 
                                    FROM 
                                        trip 
                                    WHERE 
                                        user_id = @userId
                                        AND
                                        time_end IS NULL";

                    IEnumerable<Trip> queryResult = await db.QueryAsync<Trip>(sql, new { userId = userId });
                    trips = queryResult.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return trips;
        }
    }
}
