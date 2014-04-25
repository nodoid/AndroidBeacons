using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SQLite;

namespace TagBeacons
{
    public class DBManager
    {
        public DBManager()
        {
            dbLock = new object();
			
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            pDBPath = Path.Combine(documents, "tagbeacons.db");
        }

        private string pDBPath;
        private object dbLock;

        public string DBPath
        {
            get
            {
                return pDBPath;
            }
        }

        #region SetupAndDelete

        public bool SetupDB()
        {
            lock (dbLock)
            {
                try
                {
                    using (SQLiteConnection sqlCon = new SQLiteConnection(DBPath))
                    {
                        sqlCon.CreateTable<Beacon>();
                        sqlCon.Execute(Constants.DBClauseVacuum);
                    }
                    return true;	
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                } 
            }
        }

        public void CleanUpDB()
        {	
            lock (this.dbLock)
            {
                using (SQLiteConnection sqlCon = new SQLiteConnection(this.DBPath))
                {		
                    sqlCon.Execute(Constants.DBClauseSyncOff);
                    sqlCon.BeginTransaction();
                    try
                    {
                        sqlCon.Execute("DELETE FROM Beacon");
                        sqlCon.Commit();
                        sqlCon.Execute(Constants.DBClauseVacuum);
                    }
                    catch (Exception ex)
                    {
                        #if(DEBUG)
                        System.Diagnostics.Debug.WriteLine("Error in CleanUpDB! {0}--{1}", ex.Message, ex.StackTrace);
                        #endif
                        sqlCon.Rollback();
                    }
                }
            }
        }

        #endregion

        #region Setters

        #region InsertOrUpdate

        public void InsertOrUpdateBeacon(Beacon beacon)
        {
            lock (dbLock)
            {
                using (var sqlcon = new SQLiteConnection(DBPath))
                {
                    sqlcon.Execute(Constants.DBClauseSyncOff);
                    sqlcon.BeginTransaction();
                    try
                    {
                        if (sqlcon.Execute("UPDATE Beacon DeviceUUID=?," +
                           "DeviceDistance=?,Proximity=?,FirstSeen=?,LastSeen=?,WelcomeMessage=?,SignalStrength=? WHERE DeviceUUID=?",
                               beacon.DeviceUUID, beacon.DeviceDistance, beacon.Proximity, beacon.FirstSeen, beacon.LastSeen, beacon.WelcomeMessage, 
                               beacon.SignalStrength, beacon.DeviceUUID) == 0)
                            sqlcon.Insert(beacon, typeof(Beacon));
                        sqlcon.Commit(); 
                    }
                    catch (Exception ex)
                    {
                        #if DEBUG
                        Console.WriteLine("Error in DeleteObject - {0}--{1}", ex.Message, ex.StackTrace);
                        #endif
                        sqlcon.Rollback();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Getters

        public List<Beacon> GetSingleBeacon(string id)
        {
            lock (dbLock)
            {
                using (var sqlCon = new SQLiteConnection(DBPath))
                {
                    sqlCon.Execute(Constants.DBClauseSyncOff);
                    sqlCon.BeginTransaction();
                    string sql = string.Format("SELECT * FROM Beacons WHERE id=\"{0}\"", id);
                    var data = sqlCon.Query<Beacon>(sql);
                    return data;
                }
            }
        }

        public List<Beacon> GetListOfBeacons()
        {
            lock (dbLock)
            {
                using (var sqlCon = new SQLiteConnection(DBPath))
                {
                    sqlCon.Execute(Constants.DBClauseSyncOff);
                    sqlCon.BeginTransaction();
                    var data = sqlCon.Query<Beacon>("SELECT * FROM Beacons");
                    return data;
                }
            }
        }

        #endregion
    }
}
