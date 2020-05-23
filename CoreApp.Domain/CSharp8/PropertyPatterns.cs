namespace CoreApp.Domain.CSharp8
{
    public static class PropertyPatterns
    {
        // The property pattern enables you to match on properties of the object examined.
        // Consider an eCommerce site that must compute sales tax based on the buyer's address. 
        // That computation isn't a core responsibility of an Address class. It will change over time, 
        // likely more often than address format changes. The amount of sales tax depends on the 
        // State property of the address.The following method uses the property pattern 
        // to compute the sales tax from the address and the price:
        public static decimal ComputeSalesTax(Address location, decimal salePrice) 
            => location switch
            {
                { State: "WA" } => salePrice * 0.06M,
                { State: "MN" } => salePrice * 0.075M,
                { State: "MI" } => salePrice * 0.05M,
                // other cases removed for brevity...
                _ => 0M
            };
    }

    public class Address
    {
        public string State { get; set; }
    }
}
