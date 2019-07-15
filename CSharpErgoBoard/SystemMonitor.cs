using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

public class SystemMonitor
{
    private static Computer computer = new Computer();
    private static bool opened = false;

    public void OpenComputer()
    {
        opened = true;
        computer.Open();
    }
    public bool CloseComputer()
    {
        if (opened)
        {
            opened = false;
            computer.Close();
            return true;
        }
        return false;
    }

    // CPU
    // If you have a 4 core cpu then core#5 would be the package.
    public int GetCpuCores()
    {
        if (!opened)
        {
            return 0;
        }
        int cores = 0;
        computer.CPUEnabled = true;
        for (int i = 0; i < computer.Hardware.Length; i++)
        {
            // Pick CPU hardware
            if (computer.Hardware[i].HardwareType != HardwareType.CPU)
            {
                continue;
            }
            for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
            {
                // Pick random CPU sensor. All cores should have a temp. 
                if (computer.Hardware[i].Sensors[j].SensorType != SensorType.Temperature)
                {
                    continue;
                }
                cores += 1;
            }
        }
        computer.CPUEnabled = false;

        return cores;
    }
    private float GetCPUInfo(int core, SensorType sensor)
    {
        // Making sure we are open
        if (!opened)
        {
            return 0;
        }

        // Making sure we have a legit value
        // Plus 1 to represent package. 
        int totalCore = GetCpuCores();
        //if (core > (totalCore + 1))
        //{
        //    return 0;
        //}

        // Wrong type of sensor
        if ((sensor != SensorType.Temperature) && (sensor != SensorType.Load) && (sensor != SensorType.Clock))
        {
            return 0;
        }

        float value = 100;
        computer.CPUEnabled = true;
        for (int i = 0; core > 0; i++)
        {
            // Find the right hardware
            if (computer.Hardware[i].HardwareType != HardwareType.CPU)
            {
                continue;
            }
            for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
            {
                // Find right sensor. 
                if (computer.Hardware[i].Sensors[j].SensorType != sensor)
                {
                    continue;
                }
                value = (float)(computer.Hardware[i].Sensors[j].Value);

                core -= 1;
                if (core == 0)
                {
                    // Returning here
                    computer.CPUEnabled = false;
                    return (value);
                }
            }
        }
        computer.CPUEnabled = false;

        return 0;
    }
    public float GetCpuClock(int core)
    {
        if (!opened)
        {
            return 0;
        }
        return (GetCPUInfo(core, SensorType.Clock));
    }
    public float GetCpuLoad(int core)
    {
        if (!opened)
        {
            return 0;
        }
        return (GetCPUInfo(core, SensorType.Load));
    }
    public float GetCpuTemp(int core)
    {
        if (!opened)
        {
            return 0;
        }
        return (GetCPUInfo(core, SensorType.Temperature));
    }

    // GPU
    // For clock 1 means core, 2 means memory, 3 means shader
    // For Load 4 means memory and 1 means core.
    private float GetGpuInfo(int core, SensorType sensor)
    {
        // Making sure we are open
        if (!opened)
        {
            return 0;
        }
        // Making sure we don't get a bad number
        if (sensor == SensorType.Clock)
        {
            if (core > 3)
            {
                return 0;
            }
        }
        if (sensor == SensorType.Load)
        {
            if (core > 4)
            {
                return 0;
            }
        }

        float value = 0;
        computer.GPUEnabled = true;
        for (int i = 0; core > 0; i++)
        {
            // Find the right hardware
            if ((computer.Hardware[i].HardwareType != HardwareType.GpuNvidia) && (computer.Hardware[i].HardwareType != HardwareType.GpuAti))
            {
                continue;
            }
            for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
            {
                // Find right sensor. 
                if (computer.Hardware[i].Sensors[j].SensorType != sensor)
                {
                    continue;
                }

                value = (float)(computer.Hardware[i].Sensors[j].Value);
                core -= 1;
                if (core == 0)
                {
                    // Returning here
                    computer.GPUEnabled = false;
                    return (value);
                }
            }
        }
        computer.GPUEnabled = false;

        return 0;
    }
    public float GetGpuTemp()
    {
        if (!opened)
        {
            return 0;
        }
        return (GetGpuInfo(1, SensorType.Temperature));
    }
    public float GetGpuClock(int core)
    {
        if (!opened)
        {
            return 0;
        }
        return (GetGpuInfo(core, SensorType.Clock));
    }
    public float GetGpuLoad(int core)
    {
        if (!opened)
        {
            return 0;
        }
        return (GetGpuInfo(core, SensorType.Load));
    }
}
