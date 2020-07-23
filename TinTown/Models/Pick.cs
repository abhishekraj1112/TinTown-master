using System.Collections.Generic;

namespace TinTown.Models
{
    public class Pick
    {
        public List<PickLine> pickLines { get; set; }
        public List<SaleHeader> saleHeaders { get; set; }
        public List<SaleLine> saleLines { get; set; }
    }

    public class PickLine
    {
        public string PickNo { get; set; }
        public int PickLineNo { get; set; }
        public string ShipmentNo { get; set; }
        public string SaleNo { get; set; }
        public int SaleLineNo { get; set; }
        public string SourceDocument { get; set; }
        public string OrderNo { get; set; }
        public int Marketplace { get; set; }
        public string MarketplaceName { get; set; }
        public string Bincode { get; set; }
        public string Barcode { get; set; }
        public int QtyOrdered { get; set; }
    }

    public class SaleHeader
    {
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string PSIInvoiceNo { get; set; }
        public string InvoiceDatetime { get; set; }
        public string Tier { get; set; }
        public string Wc { get; set; }
        public string Giftcard { get; set; }
        public string ShipAgentCode2 { get; set; }
        public string DocketNo { get; set; }
        public string DestCode { get; set; }
        public string ShipName { get; set; }
        public string CustomerAddress { get; set; }
        public string ShipSate { get; set; }
        public string MobileNo { get; set; }
        public string PaymentMethod { get; set; }
        public string ShipmentNo { get; set; }
        public string Zcoin { get; set; }
        public string ShipingAmt { get; set; }
        public string CodCharge { get; set; }
        public string SourceNo { get; set; }
        public string MarketplaceOrderId { get; set; }

    }

    public class SaleLine
    {
        public string Type { get; set; }
        public string OrderNo { get; set; }
        public string ItemNo { get; set; }
        public string HSNCode { get; set; }
        public int TotalQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public string SP { get; set; }
        public float GSTPercentage { get; set; }
        public float DstAmt { get; set; }
        public float NetAmt { get; set; }
        public float CgstRate { get; set; }
        public float SgstRate { get; set; }
        public float IgstRat { get; set; }
        public float CgstAmt { get; set; }
        public float SgstAmt { get; set; }
        public float IgstAmt { get; set; }
        public string SourceNo { get; set; }
        public float SourceLineNo { get; set; }
    }

    public class GetPick
    {
        public string EmailId { get; set; }
        public string PickZone { get; set; }
        public int ShiftId { get; set; }
        public int LocationId { get; set; }
        public string TrayNo { get; set; }
        public string WorkType { get; set; }
    }

    public class PickScan
    {
        public string EmailId { get; set; }
        public string PickNo { get; set; }
        public string PickLineNo { get; set; }
        public string TrayNo { get; set; }
        public string UserZone { get; set; }
        public string Barcode { get; set; }
        public string Bincode { get; set; }
        public int LocationId { get; set; }
    }

    public class NAVNF
    {
        public List<PickNF> Nflines { get; set; }
    }

    public class PickNF
    {
        public string PickNo { get; set; }
        public int PickLineNo { get; set; }
        public string ShipmentNo { get; set; }
        public string SaleNo { get; set; }
        public int SaleLineNo { get; set; }
        public string SourceDocument { get; set; }
        public string OrderNo { get; set; }
        public int Marketplace { get; set; }
        public string MarketplaceName { get; set; }
        public string Bincode { get; set; }
        public string Barcode { get; set; }
        public int QtyOrdered { get; set; }
        public int ParentLineNo { get; set; }
    }

    public class ImageResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string img4 { get; set; }
    }

    public class ReturnResponse
    {
        public string condition { get; set; }
        public string message { get; set; }
        public string action { get; set; }
        public string pick_no { get; set; }
        public string pick_line_no { get; set; }
        public string pick_zone { get; set; }
        public string priority { get; set; }
        public string barcode { get; set; }
        public string bincode { get; set; }
        public string qty_ordered { get; set; }
        public string qty_picked { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string style { get; set; }
        public string shift_name { get; set; }
        public string shift_id { get; set; }
        public string tray { get; set; }
        public ImageResponse images { get; set; }
    }

    public class OrderCancel
    {
        public string OrderNo { get; set; }
        public string OrderStatus { get; set; }
    }

    public class Priority
    {
        public string PickNo { get; set; }
        public int PriorityID { get; set; }
        public string EmailID { get; set; }
        public string WorkType { get; set; }
    }

    public class ForcePickAssig
    {
        public string PickNo { get; set; }
        public List<ForceZone> ZoneWithPicker { get; set; }
    }

    public class ForceZone
    {
        public string Zone { get; set; }
        public string Picker { get; set; }
    }

    public class PickSearch
    {
        public string PickNo { get; set; }
        public string WorkType { get; set; }
        public int LocationId { get; set; }
    }

    public class PickSetup
    {
        public string flag { get; set; }
        public int NoOfBin { get; set; }
    }
}
