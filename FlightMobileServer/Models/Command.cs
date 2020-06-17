using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class Command
    {
        [Key]
        public long Id { set; get; }

        [JsonPropertyName("aileron")]
        public double Aileron { set; get; }

        [JsonPropertyName("Rudder")]
        public double Rudder { set; get; }

        [JsonPropertyName("elevator")]
        public double Elevator { set; get; }

        [JsonPropertyName("throttle")]
        public double Throttle { set; get; }
    }
}
