using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Domain.Settings;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BandwidthMonitorService.DomainServices
{
    public class SamplesService : ISamplesService
    {
        private readonly IMongoCollection<Sample> _samples;

        public SamplesService(ISamplesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _samples = database.GetCollection<Sample>(settings.CollectionName);
        }

        public List<Sample> Get() =>
            _samples.Find(book => true).ToList();

        public Sample Get(string id) =>
            _samples.Find(book => book.Id == id).FirstOrDefault();

        public Sample Create(Sample sample)
        {
            _samples.InsertOne(sample);
            return sample;
        }

        public void Update(string id, Sample sample) =>
            _samples.ReplaceOne(book => book.Id == id, sample);

        public void Remove(Sample sample) =>
            _samples.DeleteOne(book => book.Id == sample.Id);

        public void Remove(string id) =>
            _samples.DeleteOne(book => book.Id == id);
    }
}
