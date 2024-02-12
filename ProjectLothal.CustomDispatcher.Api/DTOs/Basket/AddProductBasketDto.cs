namespace ProjectLothal.CustomDispatcher.Api.DTOs.Basket
{
    public class AddProductBasketDto
    {
        public int Sku { get; set; }
        public int Qty { get; set; }
        public string? Barcode { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
    }
}
