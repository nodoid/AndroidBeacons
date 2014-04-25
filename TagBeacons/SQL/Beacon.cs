using System;
using SQLite;

namespace TagBeacons
{
    public class Beacon
    {
        [PrimaryKey]
        public string DeviceUUID { get; set; }

        public double DeviceDistance { get; set; }

        public int Proximity { get; set; }

        public DateTime FirstSeen { get; set; }

        public DateTime LastSeen { get; set; }

        public string WelcomeMessage { get; set; }

        public int SignalStrength { get; set; }

        public override string ToString()
        {
            return string.Format("[Beacon: DeviceUUID={0}, DeviceDistance={1}, Proximity={2}, FirstSeen={3}, LastSeen={4}, WelcomeMessage={5}, SignalStrength={6}]", DeviceUUID, DeviceDistance, Proximity, FirstSeen, LastSeen, WelcomeMessage, SignalStrength);
        }
    }
}

