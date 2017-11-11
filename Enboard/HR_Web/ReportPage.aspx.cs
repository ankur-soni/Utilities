using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Collections;

namespace HR_Web
{
    public partial class ReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                reportLoad();
        }

        protected void reportLoad()
        {
            try
            {
                string urlReportServer = "http://silpundevdb1//ReportServer";
             
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                ReportViewer1.ServerReport.ReportServerUrl = new Uri(urlReportServer);
                ReportViewer1.ServerReport.ReportPath = "/HR Reports/Reports/Background Verification Form";

                ReportViewer1.ServerReport.Refresh();

                ReportParameter[] reportParameterCollection = new ReportParameter[1];       //Array size describes the number of paramaters.
                reportParameterCollection[0] = new ReportParameter();
                reportParameterCollection[0].Name = "UserId";                                 //Give Your Parameter Name
                reportParameterCollection[0].Values.Add("");                         //Pass Parametrs's value here.
                ReportViewer1.ServerReport.SetParameters(reportParameterCollection);
                ReportViewer1.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}