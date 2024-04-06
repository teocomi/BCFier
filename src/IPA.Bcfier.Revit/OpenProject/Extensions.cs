// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

namespace IPA.Bcfier.Revit.OpenProject
{
    public static class Extensions
    {
        /// <summary>
        /// Checks a decimal number against infinity.
        /// </summary>
        /// <param name="number">A decimal value</param>
        /// <returns>true, if a decimal value is finite, false otherwise</returns>
        public static bool IsFinite(this decimal number) => number > decimal.MinValue && number < decimal.MaxValue;

        /// <summary>
        /// Converts a double value to a decimal value without throwing a <see
        /// cref="OverflowException"/>. If the double is bigger then the maximal value of decimals,
        /// the decimal maximal value is returned.
        /// </summary>
        /// <param name="double">The double value</param>
        /// <returns>The converted decimal value</returns>
        public static decimal ToDecimal(this double @double)
        {
            if (@double < 0)
                return @double < (double)decimal.MinValue ? decimal.MinValue : (decimal)@double;

            return @double > (double)decimal.MaxValue ? decimal.MaxValue : (decimal)@double;
        }

        /// <summary>
        /// Converts a float value to a decimal value without throwing a <see
        /// cref="OverflowException"/>. If the float is bigger then the maximal value of decimals,
        /// the decimal maximal value is returned.
        /// </summary>
        /// <param name="float">The float value</param>
        /// <returns>The converted decimal value</returns>
        public static decimal ToDecimal(this float @float)
        {
            if (@float < 0)
                return @float < (float)decimal.MinValue ? decimal.MinValue : (decimal)@float;

            return @float > (float)decimal.MaxValue ? decimal.MaxValue : (decimal)@float;
        }
    }
}
