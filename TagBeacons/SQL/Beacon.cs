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

        public int Major { get; set; }

        public int Minor { get; set; }

        public DateTime FirstSeen { get; set; }

        public DateTime LastSeen { get; set; }

        public string WelcomeMessage { get; set; }

        public int SignalStrength { get; set; }

        public override string ToString()
        {
            return string.Format("[Beacon: DeviceUUID={0}, DeviceDistance={1}, Proximity={2}, Major={3}, Minor={4}, FirstSeen={5}, LastSeen={6}, WelcomeMessage={7}, SignalStrength={8}]", DeviceUUID, DeviceDistance, Proximity, Major, Minor, FirstSeen, LastSeen, WelcomeMessage, SignalStrength);
        }
    }
}

