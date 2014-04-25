using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RadiusNetworks.IBeaconAndroid;

namespace TagBeacons
{
    [Activity(Label = "TagBeacons", MainLauncher = true)]
    public class MainActivity : Activity, IBeaconConsumer
    {
        private readonly string TagName = "TagPoints";
        private bool AppPaused;
        private IBeaconManager beaconManager;
        private MonitorNotifier monitorNotifier;
        private RangeNotifer rangeNotifier;
        private Region monitorRegion, rangeRegion;
    }
}


