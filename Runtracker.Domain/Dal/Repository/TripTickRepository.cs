using Dapper;
using DapperExtensions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Runtracker.Domain.Dal.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtracker.Domain.Dal.Repository
{
    public class TripTickRepository
    {
        private readonly IConfiguration _configuration;

        public TripTickRepository(IConfiguration configuration)
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

        public async Task<TripTick> AddTripTick(long tripId, int order, decimal latitude, decimal longtitude, DateTime tickDateTime)
        {
            TripTick result;
            using (IDbConnection db = GetConnection())
            {
                try
                {
                    string sql = @"INSERT INTO 
                                        trip_tick (""trip_id"", ""order"", ""tick_time"", ""geom"") 
                                    VALUES 
                                        (@tripId, @order, @tickDateTime, ST_SetSRID(ST_MakePoint(@latitude, @longtitude),4326))
                                    RETURNING id";
                                        //(@tripId, @tickDateTime, ST_SetSRID(ST_MakePoint(@latitude, @long), 4326))";
                    long insertedTripTickId = await db.QueryFirstAsync<long>(sql, new { tripId = tripId, order = order, latitude = latitude, longtitude = longtitude, tickDateTime = tickDateTime });

                    result = await GetByIdOrThrowException(insertedTripTickId);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public async Task<TripTick> GetByIdOrThrowException(long tripTickId)
        {
            TripTick tripTick = null;
            using (IDbConnection db = GetConnection())
            {
                try
                {
                    string sql = @"SELECT
                                        ""id"" AS Id,
                                        ""trip_id"" AS TripId,
                                        ""tick_time"" AS TickTime,
                                        ""order"" AS Order,
                                        ""geom"" AS Geom
                                    FROM 
                                        trip_tick
                                    WHERE 
                                        id=@tripTickId";
                    tripTick = await db.QueryFirstOrDefaultAsync<TripTick>(sql, new { tripTickId = tripTickId });
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            if (tripTick == null)
            {
                throw new Exception($"No TripTick found with id {tripTickId}");
            }
            return tripTick;
        }

        public async Task<List<TripTick>> GetByTripId(long tripId)
        {
            List<TripTick> tripTicks = new List<TripTick>();
            using (IDbConnection db = GetConnection())
            {
                try
                {
                    string sql = @"SELECT 
                                        ""id"" AS Id,
                                        ""trip_id"" AS TripId,
                                        ""tick_time"" AS TickTime,
                                        ""order"" AS Order,
                                        ""geom"" AS Geom
                                    FROM 
                                        trip_tick
                                    WHERE
                                        trip_id = @tripId";
                    IEnumerable<TripTick> queryResult = await db.QueryAsync<TripTick>(sql, new { tripId = tripId });
                    tripTicks = queryResult.ToList();
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            return tripTicks;
        }

    }
}
