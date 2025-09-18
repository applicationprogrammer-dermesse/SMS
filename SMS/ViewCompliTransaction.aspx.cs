using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;

namespace SMS
{
    public partial class ViewCompliTransaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                genDetailedToPrint(Session["iNo"].ToString());
            }
        }

        private void genDetailedToPrint(string theID)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"SELECT LTRIM(RTRIM(D.BrName)) AS [Branch]
                                          ,B.CompliName
                                          ,A.[Complimentaryno]
                                          ,A.[Complimentarydate]
                                          ,C.[vPluCode]
	                                      ,C.vDESCRIPTION
                                          ,A.[CompliCustomerName]
                                          ,A.[SRP]
	                                      ,A.[vQty]
                                          ,A.[CompliAmount]
                                      FROM [PostedComplimentary] A
                                      LEFT JOIN CompliList B
                                      ON A.CompliID=B.CompliID
                                      LEFT join vItemMaster C
                                      ON A.vFGCode = C.vFGCode
                                      LEFT JOIN MyBranchList D
                                      ON A.BrCode = D.BrCode
                                      where A.CompliID=@CompliID
                                      UNION ALL
                                      SELECT LTRIM(RTRIM(D.BrName)) AS [Branch]
                                          ,B.CompliName
                                          ,A.[Complimentaryno]
                                          ,A.[Complimentarydate]
                                          ,C.[vPluCode]
	                                      ,C.vDESCRIPTION
                                          ,A.[CompliCustomerName]
                                          ,A.[SRP]
	                                      ,A.[vQty]
                                          ,A.[CompliAmount]
                                      FROM [UnpostedComplimentary] A
                                      LEFT JOIN CompliList B
                                      ON A.CompliID=B.CompliID
                                      LEFT join vItemMaster C
                                      ON A.vFGCode = C.vFGCode
                                      LEFT JOIN MyBranchList D
                                      ON A.BrCode = D.BrCode
                                      where A.CompliID=@CompliID
                                      ORDER BY A.[Complimentarydate]";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@CompliID", theID.ToString());
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);

                    gvPrint.DataSource = dT;
                    gvPrint.DataBind();

                    if (gvPrint.Rows.Count > 0)
                    {

                        gvPrint.FooterRow.Cells[9].Text = "Total Amount";
                        gvPrint.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                        decimal total10 = dT.AsEnumerable().Sum(row => row.Field<decimal>("CompliAmount"));
                        gvPrint.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                        gvPrint.FooterRow.Cells[10].Text = total10.ToString("N2");
                    }
                }
            }
        }

    }
}