using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class GRN
    {
        public string ExternalInvoiceNo { get; set; } = "";
        public string ExternalInvoiceDate { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }
        public string CreatedBy { get; set; }
        public string GateEntryNo { get; set; } = "";

        public float BilofEntryAmount { get; set; }
        public string BilofEntryNo { get; set; } = "";
        public string BilofEntryDate { get; set; } = "";

        public string ProcessType { get; set; }
        public string GRNHeaderNo { get; set; }
        public string GRNLineNo { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }

        public int DocumentLineNo { get; set; }
        public string VendorLotNo { get; set; }
        public string ExpireDate { get; set; }
        public string Barcode { get; set; }
        public string SalesInvoiceNo { get; set; }

    }

    public class SearchDocument
    {
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int LocationId { get; set; }
        public string Filter { get; set; }
    }

    public class DocumentInfoResponse
    {
        public JArray DocumentData { get; set; }
        public JArray GRNData { get; set; }
    }

    public class BarcodeInput
    {
        public long Barcode { get; set; }
        public int Quantity { get; set; }
    }


    public class DeleteBarcode
    {
        public string GRNHeaderNo { get; set; }
        public string GRNLineNo { get; set; }
        public int DocumentLineNo { get; set; }
        public List<DeleteBarcodeLine> lines { get; set; }
    }

    public class DeleteBarcodeLine
    {
        public string barcode { get; set; }
        public int accepted_qty { get; set; }
    }
}
