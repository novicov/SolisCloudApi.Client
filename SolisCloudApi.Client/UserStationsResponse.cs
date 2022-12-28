namespace SolisCloudApi.Client;

public record UserStationsResponse
{
    public int Page { get; set; }
    public int StationStatusVo { get; set; }
    public long Total { get; set; }
    public List<string> Records { get; set; } = new(); //  todo smt should be here 
    public int All { get; set; }
    public int Normal { get; set; }
    public int Offline { get; set; }
    public int Fault { get; set; }
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? Capacity { get; set; }
    public string? CapacityStr { get; set; }
    public double Capacity1 { get; set; }
    public double FullHour { get; set; }
    public string? PicName { get; set; }
    public long InstallerId { get; set; }
    public string? Installer { get; set; }
    public long DataTimestamp { get; set; }
    public string? InstallerMobile { get; set; }
    public string? Sno { get; set; }
    public int Country { get; set; }
    public string? CountryStr { get; set; }
    public int Region { get; set; }
    public string? RegionStr { get; set; }
    public int City { get; set; }
    public string? CityStr { get; set; }
    public int County { get; set; }
    public string? CountyStr { get; set; }
    public double Dip { get; set; }
    public double Azimuth { get; set; }
    public double TimeZone { get; set; }
    public string? TimeZoneName { get; set; }
    public string? TimeZoneStr { get; set; }
    public long TimeZoneId { get; set; }
    public double Daylight { get; set; }
    public long CreateDate { get; set; }
    public double Price { get; set; }
    public long Module { get; set; }
    public string? Pic1Url { get; set; }
    public double Power { get; set; }
    public string? PowerStr { get; set; }
    public double DayEnergy { get; set; }
    public string? DayEnergyStr { get; set; }
    public double DayIncome { get; set; }
    public string? DayIncomeUnit { get; set; }
    public double MonthEnergy { get; set; }
    public string? MonthEnergyStr { get; set; }
    public double YearEnergy { get; set; }
    public string? YearEnergyStr { get; set; }
    public double AllEnergy { get; set; }
    public string? AllEnergyStr { get; set; }
    public double AllEnergy1 { get; set; }
    public double AllIncome { get; set; }

    public string? AllIncomeUnit { get; set; }

    //0 Full Grid Tied
    //1 Self-consumption
    //2 Off-grid
    public int SynchronizationType { get; set; }

    //1 – Grid tied;
    //2 – Grid-tied+Meter at load side;
    //3 - Grid-tied+Meter at grid side;
    //4 – Hybrid+ Meter at load side;
    //5 - Hybrid+ Meter at grid side;
    public int StationTypeNew { get; set; }
    public double BatteryTotalDischargeEnergy { get; set; }
    public double BatteryTotalChargeEnergy { get; set; }
    public double GridPurchasedTotalEnergy { get; set; }
    public double GridSellTotalEnergy { get; set; }
    public double HomeLoadTotalEnergy { get; set; }
    public double OneSelf { get; set; }
    public double BatteryTodayDischargeEnergy { get; set; }
    public double BatteryTodayChargeEnergy { get; set; }
    public double GridPurchasedTodayEnergy { get; set; }
    public double GridSellTodayEnergy { get; set; }
    public double HomeLoadTodayEnergy { get; set; }
    public string? Money { get; set; }
    public long FisPowerTime { get; set; }
    public long FisGenerateTime { get; set; }
    public string? Remark1 { get; set; }
    public string? Remark2 { get; set; }

    public string? Remark3 { get; set; }

    //1：Online 2：Offline 3：Alarm
    public int State { get; set; }
    public long DataTimestamp1 { get; set; }

    public string? InverterPower { get; set; }

    // For AU only
    public string? NmiCode { get; set; }
}