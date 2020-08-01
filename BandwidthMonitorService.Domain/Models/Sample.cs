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
        public long BytesRead { get; set; }
        public long TotalReads { get; set; }
        public TimeSpan Elapsed { get; set; }
        public long RoundTripTime { get; set; }
    }
}
