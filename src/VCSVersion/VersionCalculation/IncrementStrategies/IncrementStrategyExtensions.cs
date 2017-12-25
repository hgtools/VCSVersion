using System;

namespace VCSVersion.VersionCalculation.IncrementStrategies
{
    public static class IncrementStrategyExtensions
    {
        public static VersionField ToVersionField(this IncrementStrategyType strategyType)
        {
            switch (strategyType)
            {
                case IncrementStrategyType.None:
                    return VersionField.None;
                case IncrementStrategyType.Major:
                    return VersionField.Major;
                case IncrementStrategyType.Minor:
                    return VersionField.Minor;
                case IncrementStrategyType.Patch:
                    return VersionField.Patch;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyType), strategyType, null);
            }
        }
    }
}