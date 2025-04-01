namespace S2SOMSAPI.Model
{
    public class S2SOrderListReq
    {
        public int CurrentPage {  get; set; }
        public int RecordLimit { get; set; }
        public long UserId { get; set; }
        public string Search  { get; set; }
        public string Filter { get; set; }
    }

    public class S2SOrderHistoryReq
    {
        public string S2SOrderNO { get; set; }
    }

    public class S2SOrderViewReq
    {
        public int WinCashOrderNumber { get; set; }
        public int OMSOrderNo { get; set; }
    }
}
