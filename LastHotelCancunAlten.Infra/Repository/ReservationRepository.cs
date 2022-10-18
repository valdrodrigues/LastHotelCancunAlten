using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using LastHotelCancunAlten.Domain.Entity;
using LastHotelCancunAlten.Domain.IRepository;
using LastHotelCancunAlten.Infra.Configuration;

namespace LastHotelCancunAlten.Infra.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly ReservationConfiguration _settings;

        public ReservationRepository(IOptions<ReservationConfiguration> settings)
        {
            _settings = settings.Value;
            MongoClient client = new MongoClient(_settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(_settings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(_settings.ReservationCollectionName);
        }

        public Reservation GetById(string id)
        {
            return _reservationCollection.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<Reservation> GetByIdCard(string idCard)
        {
            return _reservationCollection.Find(x => x.IdCard == idCard).ToList();
        }

        public void Create(Reservation entity)
        {
            _reservationCollection.InsertOne(entity);
        }

        public Reservation Update(Reservation entity)
        {
            var filter = FilterById(entity.Id);
            _reservationCollection.ReplaceOne(filter, entity);
            return entity;
        }

        public void Delete(string id)
        {
            var filter = FilterById(id);
            _reservationCollection.DeleteOne(filter);
        }

        public bool IsAvailable(DateTime startDate, DateTime endDate, string id = null)
        {
            FilterDefinition<Reservation> filter;

            if (id != null)
            {
                filter = FilterIsAvailableById(startDate, endDate, id);
            }
            else
            {
                filter = FilterIsAvailable(startDate, endDate);
            }

            var hasReservation = _reservationCollection.Find(filter).Any();

            if (hasReservation)
                return false;

            return true;
        }

        private FilterDefinition<Reservation> FilterById(string id)
        {
            return Builders<Reservation>.Filter
                .Eq(s => s.Id, id);
        }

        private FilterDefinition<Reservation> FilterIsAvailable(DateTime startDate, DateTime endDate)
        {
            return Builders<Reservation>.Filter
                .Where(f => 
                        (f.StartDate <= startDate && startDate <= f.EndDate) 
                        || (f.StartDate <= endDate && endDate <= f.EndDate));
        }

        private FilterDefinition<Reservation> FilterIsAvailableById(DateTime startDate, DateTime endDate, string id)
        {
            return Builders<Reservation>.Filter
                .Where(f =>
                        (f.StartDate <= startDate && startDate <= f.EndDate && f.Id != id)
                        || (f.StartDate <= endDate && endDate <= f.EndDate && f.Id != id));
        }
    }
}
