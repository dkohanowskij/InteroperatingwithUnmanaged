using System.Runtime.InteropServices;

namespace PowerLibrary
{
	[ComVisible(true)]
	[Guid("4211255B-FC87-4198-A7CD-76A9886BF24B")]
	[ClassInterface(ClassInterfaceType.None)]
	public class PowerInformer : IPowerInformer
	{
		public void ShowSystemBatteryState()
		{
			PowerInformation.ShowSystemBatteryState();
		}

		public void ShowSystemPowerInformation()
		{
			PowerInformation.ShowSystemPowerInformation();
		}

		public void ShowLastSleepTime()
		{
			PowerInformation.ShowLastSleepTime();
		}

		public void ShowLastWakeTime()
		{
			PowerInformation.ShowLastWakeTime();
		}

		public void ReserveHiberFile()
		{
			PowerInformation.ReserveHiberFile();
		}

		public void RemoveHiberFile()
		{
			PowerInformation.RemoveHiberFile();
		}

		public void Hibernate()
		{
			PowerInformation.Hibernate();
		}

		public void Suspend()
		{
			PowerInformation.Suspend();
		}
	}
}