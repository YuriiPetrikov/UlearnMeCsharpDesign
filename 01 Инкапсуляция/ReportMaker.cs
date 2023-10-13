using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public class Common
    {
        public static int IsEarlierSerious(DateTime date1, DateTime date2)
        {
            return (date2 > date1) ? 1 : 0;
        }
    }

    public class FailureDevice
    {
        public FailureType FailureTypes { get; set; }
        public int DeviceId { get; }
        public DateTime Date { get; }
        public FailureDevice(FailureType failureTypes, int deviceId, DateTime date)
        {
            FailureTypes = failureTypes;
            DeviceId = deviceId;
            Date = date;
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    public class Device
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }

        public Device(int DeviceId, string Name)
        {
            this.DeviceId = DeviceId;
            this.Name = Name;
        }
    }

    public class ReportMaker
    {
        static int IsFailureSerious(int failureType)
        {
            return ((FailureType)failureType == FailureType.UnexpectedShutdown 
                || (FailureType)failureType == FailureType.HardwareFailures) ? 1 : 0;
        }

        public static List<string> FindDevicesFailedBeforeDate(
           DateTime date,
           List<Device> deviceId
           ,List<FailureDevice> failureDevices
           )
        {
            var problematicDevices = new HashSet<int>();

            foreach(var vr in failureDevices)
            {
                if (ReportMaker.IsFailureSerious((int)vr.FailureTypes) == 1 
                    && Common.IsEarlierSerious(vr.Date, date) == 1)
                    problematicDevices.Add(vr.DeviceId);
            }

            
            var result = new List<string>();

            foreach (var vr in deviceId)
                if (problematicDevices.Contains(vr.DeviceId))
                            result.Add(vr.Name);

            return result;
        }
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            List<FailureDevice> devices1 = new List<FailureDevice>();

            var date = new DateTime(year, month, day);

            for (int i = 0; i < failureTypes.Length; i++)
            {
                var date1 = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                devices1.Add(new FailureDevice((FailureType)failureTypes[i], deviceId[i], date1));
            }

            List<Device> device_ = new List<Device>();
            foreach (var device in devices)
                device_.Add(new Device((int)device["DeviceId"], device["Name"] as string));

            return FindDevicesFailedBeforeDate(date, device_, devices1);
        }
    }
}
