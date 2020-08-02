using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BandwidthMonitorService.Domain.Models
{
    public class Sample
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Timestamp { get; set; }
        public string Url { get; set; }
        public double BytesRead { get; set; }
        public double TotalReads { get; set; }
        public TimeSpan Elapsed { get; set; }
        public double RoundTripTime { get; set; }
    }
}
