using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Data;
using Microsoft.Data.SqlClient;

namespace S2SOMSAPI.Repository
{
    public class S2SOrderViewRepo : IS2SOrderView
    {
        private readonly IConfiguration _configuration;
        private readonly string Connstrr;
        SqlParameter[] param;

        public S2SOrderViewRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            Connstrr = _configuration.GetConnectionString("GWC_ConnectionString")!;
        }

        public async Task<S2SOrderViewResp> OrderView(S2SOrderViewReq req)
        //public Task<List<S2SOrderViewResp>> OrderView(S2SOrderViewReq req)
        {
            var response = new S2SOrderViewResp();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            var ProductList = new List<Skulist>();
            try
            {
                ds = fetchOrder(req);
                //if (ds != null & ds?.Tables[0].Rows.Count > 0)
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string WincashOrderNumber = ds.Tables[0].Rows[0]["WincashOrderNumber"]?.ToString() ?? "";
                    string S2SOrderNo = ds.Tables[0].Rows[0]["S2SOrderNo"]?.ToString() ?? "";
                    string Status = ds.Tables[0].Rows[0]["Status"]?.ToString() ?? "";
                    string Sourcestore = ds.Tables[0].Rows[0]["Sourcestore"]?.ToString() ?? "";
                    string DestinationStore = ds.Tables[0].Rows[0]["DestinationStore"]?.ToString() ?? "";
                    string Performedby = ds.Tables[0].Rows[0]["Performedby"]?.ToString() ?? "";
                    string Receivedby = ds.Tables[0].Rows[0]["Receivedby"]?.ToString() ?? ""; 
                    DateTime CreationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]);


                    foreach (DataRow row in ds1.Tables[0].Rows)
                    {
                        string SKU = row["sku"]?.ToString() ?? "";
                        string Skuname = row["sKuname"]?.ToString() ?? "";
                        string serialnumber = row["serialnumber"]?.ToString() ?? "";
                        Decimal Quantity = Convert.ToDecimal(row["quantity"]);

                        var ListSku = new Skulist
                        {
                            SKU = string.IsNullOrEmpty(SKU) ? "" : SKU,
                            Skuname = string.IsNullOrEmpty(Skuname) ? "" : Skuname,
                            serialnumber = string.IsNullOrEmpty(serialnumber) ? "" : serialnumber,
                            Quantity = Quantity
                        };

                        ProductList.Add(ListSku);
                    }
                    response.Skulist = ProductList;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return response;
        }

        public DataSet fetchOrder(S2SOrderViewReq req)
        {
            var ds = new DataSet();
            param = new SqlParameter[]
            {
                new SqlParameter("WinCashOrderNumber",req.WinCashOrderNumber),
                new SqlParameter("OMSOrderNo", req.OMSOrderNo)
            };
            return ReturnDataset("SP_OrderView", param);
        }

        public DataSet ReturnDataset(string procname, params SqlParameter[] param)
        {
            var ds = new DataSet();
            using SqlConnection conn = new SqlConnection(Connstrr);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(procname, conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                    {
                        da.SelectCommand.Parameters.Add(p);
                    }
                }
                //conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return ds;
        }

    }
}