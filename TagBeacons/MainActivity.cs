using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
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
        private RangeNotifier rangeNotifier;
        private Region monitorRegion, rangeRegion;
        private TextView textUUID, textProximity, textDistance, textMajor, textMinor, textMessage, textNumberSeen, textStatus;
        private DBManager dbm;

        public MainActivity()
        {
            beaconManager = IBeaconManager.GetInstanceForApplication(this);
            monitorNotifier = new MonitorNotifier();
            rangeNotifier = new RangeNotifier();
            monitorRegion = new Region(TagName, null, null, null);
            rangeRegion = new Region(TagName, null, null, null);
            DBManager dbm = new DBManager();
            dbm.SetupDB();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            textUUID = FindViewById<TextView>(Resource.Id.textUUID);
            textProximity = FindViewById<TextView>(Resource.Id.textProximity);
            textDistance = FindViewById<TextView>(Resource.Id.textDistance);
            textMajor = FindViewById<TextView>(Resource.Id.textMajor);
            textMinor = FindViewById<TextView>(Resource.Id.textMinor);
            textMessage = FindViewById<TextView>(Resource.Id.textMessage);
            textNumberSeen = FindViewById<TextView>(Resource.Id.textNumberSeen);
            textStatus = FindViewById<TextView>(Resource.Id.textStatus);

            var btAdapter = BluetoothAdapter.DefaultAdapter;
            if (btAdapter.IsEnabled)
            {
                textStatus.SetTextColor(Android.Graphics.Color.Blue);
                textStatus.Text = "Searching";
            }
            else
            {
                textStatus.SetTextColor(Android.Graphics.Color.Red);
                textStatus.Text = "Bluetooth not enabled";
            }

            beaconManager.Bind(this);

            monitorNotifier.EnterRegionComplete += EnterRegion;
            monitorNotifier.ExitRegionComplete += ExitRegion;
        }

        protected override void OnResume()
        {
            base.OnResume();
            AppPaused = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            AppPaused = true;
        }

        public void OnIBeaconServiceConnect()
        {
            try
            {
                beaconManager.SetMonitorNotifier(monitorNotifier);
                beaconManager.SetRangeNotifier(rangeNotifier);
                beaconManager.StartMonitoringBeaconsInRegion(monitorRegion);
                beaconManager.StartRangingBeaconsInRegion(rangeRegion);
            }
            catch (RemoteException ex)
            {
                Console.WriteLine("Exception in connection - {0}", ex.Message);
            }
        }

        private void EnterRegion(object s, MonitorEventArgs e)
        {
            textStatus.SetTextColor(Android.Graphics.Color.Green);
            textStatus.Text = "Beacon detected";
        }

        private void ExitRegion(object s, MonitorEventArgs e)
        {
            textStatus.SetTextColor(Android.Graphics.Color.Blue);
            textStatus.Text = "Searching";
        }

        private void RangingBeaconsInRegion(object s, RangeEventArgs e)
        {

        }
    }
}


