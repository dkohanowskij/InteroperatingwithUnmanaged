using System.Runtime.InteropServices;

namespace PowerLibrary
{
	[ComVisible(true)]
	[Guid("68BF1E2E-C29F-45D2-8D8E-0928E034E346")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IPowerInformer
	{
		void ShowSystemBatteryState();
		void ShowSystemPowerInformation();
		void ShowLastSleepTime();
		void ShowLastWakeTime();
		void ReserveHiberFile();
		void RemoveHiberFile();
		void Hibernate();
		void Suspend();
	}
}
