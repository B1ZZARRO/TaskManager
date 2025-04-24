using System.Collections.Generic;

namespace TaskManager.Models;

public class DeviceModel
{
    public List<DeviceBodyModel> Body { get; set; }
}

public class DeviceBodyModel
{
    public int DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string DeviceModel { get; set; }
}