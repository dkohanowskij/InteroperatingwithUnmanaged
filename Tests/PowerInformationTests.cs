using System;
using System.IO;
using NUnit.Framework;
using static PowerLibrary.PowerInformation;

namespace Tests
{
	[TestFixture]
	public class PowerInformationTests
	{
		[Test]
		public void SystemBatteryState_Test()
		{
			ShowSystemBatteryState();
		}

		[Test]
		public void SystemPowerInformation_Test()
		{
			ShowSystemPowerInformation();
		}

		[Test]
		public void LastSleepTime_Test()
		{
			ShowLastSleepTime();
		}

		[Test]
		public void LastWakeTime_Test()
		{
			ShowLastWakeTime();
		}

		//TODO Run as administrator!!!
		[Test]
		public void ReserveHiberFile_Test()
		{
			ReserveHiberFile();

			var hiberFileExists = File.Exists("C:\\hiberfil.sys");
			Assert.True(hiberFileExists);
		}

		//TODO Run as administrator!!!
		[Test]
		public void RemoveHiberFile_Test()
		{
			RemoveHiberFile();

			var hiberFileExists = File.Exists("C:\\hiberfil.sys");
			Assert.False(hiberFileExists);
		}

		[Test]
		public void Hibernate_Test()
		{
			Hibernate();
		}

		[Test]
		public void Suspend_Test()
		{
			Suspend();
		}
	}
}
