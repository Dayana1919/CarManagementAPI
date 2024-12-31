namespace CarManagementAPI.Contracts
{
    public class VаlidationConstants
    {
        public static class Garage
        {
            public const int NameMaxLength = 100;
            public const int LocationMaxLength = 200;
            public const int CityMaxLength = 50;
            public const int CapacityMinValue = 1;
        }

        public static class Car
        {
            public const int MakeMaxLength = 50;
            public const int ModelMaxLength = 50;
            public const int LicensePlateMaxLength = 15;
            public const int ProductionYearMin = 1886;
            private static int productionYearMax = DateTime.Now.Year;

            public static int ProductionYearMax => productionYearMax;
        }

        public static class Maintenance
        {
            public const int ServiceTypeMaxLength = 50;
            public const int ServiceTypeMinLength = 3;
        }
    }
}
