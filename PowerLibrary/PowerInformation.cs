using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PowerLibrary
{
	public class PowerInformation
	{
		private const int SYSTEM_POWER_INFORMATION_LEVEL = 12;
		private const int SYSTEM_BATTERY_STATE_LEVEL = 5;
		private const int LAST_SLEEP_TIME_LEVEL = 15;
		private const int LAST_WAKE_TIME_LEVEL = 14;
		private const int SYSTEM_RESERVE_HIBER_FILE_LEVEL = 10;

		const uint STATUS_SUCCESS = 0;

		public struct SystemBatteryState
		{
			public byte AcOnLine;
			public byte BatteryPresent;
			public byte Charging;
			public byte Discharging;
			public byte spare1;
			public byte spare2;
			public byte spare3;
			public byte spare4;
			public UInt32 MaxCapacity;
			public UInt32 RemainingCapacity;
			public Int32 Rate;
			public UInt32 EstimatedTime;
			public UInt32 DefaultAlert1;
			public UInt32 DefaultAlert2;
		}

		public struct SystemPowerInformation
		{
			public uint MaxIdlenessAllowed;
			public uint Idleness;
			public uint TimeRemaining;
			public byte CoolingMode;
		}

		[DllImport("powrprof.dll")]
		private static extern uint CallNtPowerInformation(
			int informationLevel,
			IntPtr lpInputBuffer,
			int nInputBufferSize,
			[Out] IntPtr lpOutputBuffer,
			int nOutputBufferSize
		);

		[DllImport("powrprof.dll")]
		private static extern bool SetSuspendState(
			bool hibernate,
			bool forceCritical,
			bool disableWakeEvents
		);

		public static void ShowSystemBatteryState()
		{
			SystemBatteryState state = new SystemBatteryState();
			var ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(SystemBatteryState)));
				uint retval = CallNtPowerInformation(
					SYSTEM_BATTERY_STATE_LEVEL,
					IntPtr.Zero,
					0,
					ptr,
					Marshal.SizeOf(typeof(SystemBatteryState))
				);

				if (retval == STATUS_SUCCESS)
				{
					state = Marshal.PtrToStructure<SystemBatteryState>(ptr);
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}
			ShowAllPowerInfo(state);
		}

		public static void ShowSystemPowerInformation()
		{
			SystemPowerInformation state = new SystemPowerInformation();
			var ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(SystemPowerInformation)));
				uint retval = CallNtPowerInformation(
					SYSTEM_POWER_INFORMATION_LEVEL,
					IntPtr.Zero,
					0,
					ptr,
					Marshal.SizeOf(typeof(SystemPowerInformation))
				);

				if (retval == STATUS_SUCCESS)
				{
					state = Marshal.PtrToStructure<SystemPowerInformation>(ptr);
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}

			ShowAllPowerInfo(state);
		}

		public static void ShowLastSleepTime()
		{
			long ticks = 0;
			var ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));
				uint retval = CallNtPowerInformation(
					LAST_SLEEP_TIME_LEVEL,
					(IntPtr)null,
					0,
					ptr,
					Marshal.SizeOf(typeof(long))
				);

				if (retval == STATUS_SUCCESS)
				{
					ticks = Marshal.ReadInt64(ptr);
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}

			MessageBox.Show(TimeSpan.FromTicks(ticks).ToString());
		}

		public static void ShowLastWakeTime()
		{
			long ticks = 0;
			var ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));
				uint retval = CallNtPowerInformation(
					LAST_WAKE_TIME_LEVEL,
					(IntPtr)null,
					0,
					ptr,
					Marshal.SizeOf(typeof(long))
				);

				if (retval == STATUS_SUCCESS)
				{
					ticks = Marshal.ReadInt64(ptr);
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}

			MessageBox.Show(TimeSpan.FromTicks(ticks).ToString());
		}

		public static void ReserveHiberFile()
		{
			var ptr = IntPtr.Zero;
			try
			{
				var boolPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
				Marshal.WriteInt32(boolPtr, 0, 1);
				uint retval = CallNtPowerInformation(
					SYSTEM_RESERVE_HIBER_FILE_LEVEL,
					boolPtr,
					Marshal.SizeOf(typeof(int)),
					ptr,
					Marshal.SizeOf(typeof(int))
				);

				if (retval != STATUS_SUCCESS)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}
		}

		public static void RemoveHiberFile()
		{
			var ptr = IntPtr.Zero;
			try
			{
				var boolPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(bool)));
				Marshal.WriteInt32(boolPtr, 0, 0);
				uint retval = CallNtPowerInformation(
					SYSTEM_RESERVE_HIBER_FILE_LEVEL,
					boolPtr,
					Marshal.SizeOf(typeof(int)),
					ptr,
					Marshal.SizeOf(typeof(int))
				);

				if (retval != STATUS_SUCCESS)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}
		}

		public static void Hibernate()
		{
			SetSuspendState(true, false, false);
		}

		public static void Suspend()
		{
			SetSuspendState(false, false, false);
		}

		private static void ShowAllPowerInfo<T>(T data)
		{
			var fields = typeof(T).GetFields();
			var result = new StringBuilder();
			foreach (var info in fields)
			{
				result.AppendLine($"{info.Name} = {info.GetValue(data)}");
			}

			MessageBox.Show(result.ToString());
		}
	}
}
