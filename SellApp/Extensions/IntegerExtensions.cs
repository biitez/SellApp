using System;

namespace SellApp.Extensions
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// SellApp manages payments in decimals, so the prices received from 
        /// the server is a standard way of doing things (without decimals), 
        /// using this method will add the decimals to the price.        
        /// </summary>
        /// <example>
        /// If you have a product with the price of $180.00, 
        /// the server will respond with an integer of value 18000, 
        /// this extension will convert it to 180.00
        /// </example>
        /// <param name="Value">Price without decimals</param>
        /// <returns>
        /// Example: 10000 to 100,00 on <see cref="decimal"/> 
        /// </returns>
        public static decimal PriceToDecimal(this int Value)
        {
            var valueToString = Value.ToString();

            if (!decimal.TryParse(string.Format("{0:0.##}", valueToString.Insert(valueToString.Length - 2, ",")), out var priceDoubled))
            {
                throw new InvalidOperationException("Fail parsing to double");
            }

            return priceDoubled;
        }
    }
}
