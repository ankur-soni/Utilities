<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Employer.aspx.vb" Inherits="Employer.Employer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 11%;
        }

        .auto-style3 {
            width: 9%;
        }

        .auto-style4 {
            width: 14%;
        }
    </style>
</head>
<body style="height: 523px">
    <form id="form1" runat="server">
        <asp:Panel style="padding:5px 5px 5px 5px;" ID="EmployerDetails" runat="server" BorderColor="Black" BorderWidth="1px" BorderStyle="Solid">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left; vertical-align: top">
                            <table cellpadding="5" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td class="auto-style60" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Lbl_CompanyName" runat="server" BorderColor="Black" Text="Company Name"></asp:Label>
                                        :</td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_CompanyName" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style5" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label2" runat="server" BorderColor="Black" Text="Role Explanation"></asp:Label>
                                        :</td>
                                    <td class="auto-style10" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_RoleExplaination" runat="server" Height="21px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style60" style="vertical-align: top; text-align: left">Company Address:</td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_CompanyAddress" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style56" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label3" runat="server" BorderColor="Black" Text="Relevant Experince"></asp:Label>
                                        :</td>
                                    <td class="auto-style49" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_RelExp" runat="server" Height="16px"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" Text="E.g. 5.7"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style60" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label5" runat="server" BorderColor="Black" Text="From Date:"></asp:Label>
                                    </td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_FromDate" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style41" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label8" runat="server" BorderColor="Black" Text="Reason For Leaving:"></asp:Label>
                                    </td>
                                    <td class="auto-style42" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_ReasonLeaving" runat="server" Height="16px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style60" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label6" runat="server" BorderColor="Black" Text="To Date"></asp:Label>
                                        :</td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_ToDate" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label11" runat="server" BorderColor="Black" Text="Break(s) in Career:"></asp:Label>
                                    </td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_BreakCareer" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style25" style="vertical-align: top; text-align: left">Designation on Leaving:</td>
                                    <td class="auto-style26" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_DesignLeave" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style26" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label10" runat="server" BorderColor="Black" Text="Designation on Joining:"></asp:Label>
                                    </td>
                                    <td class="auto-style26" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_DesignationJoining" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style60" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label12" runat="server" BorderColor="Black" Text="Last Drawn Salary p.a."></asp:Label>
                                        :</td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_Sal" runat="server" Height="29px"></asp:TextBox>
                                        <asp:Label ID="Label13" runat="server" Text="E.g. 600000"></asp:Label>
                                    </td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label15" runat="server" BorderColor="Black" Text="No. of People Reporting:"></asp:Label>
                                    </td>
                                    <td class="auto-style64" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_NoPeopleReporting" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style27" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label14" runat="server" BorderColor="Black" Text="Type of Business"></asp:Label>
                                    </td>
                                    <td class="auto-style28" style="vertical-align: top; text-align: left">
                                        <asp:DropDownList ID="DDL_TypeBusiness" runat="server" Height="30px" Width="123px">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="auto-style28" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label20" runat="server" BorderColor="Black" Text="No. of&nbsp; Experince in Months:"></asp:Label>
                                    </td>
                                    <td class="auto-style28" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_ExpinMonths" runat="server" Height="29px"></asp:TextBox>
                                        <asp:Label ID="Label16" runat="server" Text="E.g. 36"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style15" style="vertical-align: top; text-align: left">
                                        <asp:Label ID="Label17" runat="server" BorderColor="Black" Text="Company Contact Number"></asp:Label>
                                    </td>
                                    <td class="auto-style16" style="vertical-align: top; text-align: left">
                                        <asp:TextBox ID="Txt_CompContactNo" runat="server" Height="29px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style16" style="vertical-align: top; text-align: left">&nbsp;</td>
                                    <td class="auto-style16" style="vertical-align: top; text-align: left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style15" style="vertical-align: top; text-align: left">&nbsp;</td>
                                    <td class="auto-style16" style="vertical-align: top; text-align: left">&nbsp;</td>
                                    <td class="auto-style16" style="vertical-align: top; text-align: left">&nbsp;</td>
                                    <td class="auto-style16" style="vertical-align: top; text-align: left">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>


            </div>
            <table style="width: 100%">
                <tr>
                    <td class="auto-style2">Experince Certificate:</td>
                    <td style="width: 16.6%">
                        <asp:Button ID="Btn_ExpCertUpload" runat="server" Text="Upload" />
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="Label18" runat="server" Text="Relieving Letter"></asp:Label>
                        :</td>
                    <td style="width: 16.6%">
                        <asp:Button ID="Btn_RelLetterUpload" runat="server" Text="Upload" />
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="Label19" runat="server" Text="Salary Slips(Last 3 months)"></asp:Label>
                        :</td>
                    <td style="width: 16.6%">
                        <asp:Button ID="Btn_SalSlipUpload" runat="server" Text="Upload" Style="margin-left: 0px" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%; margin-top: 20px;">
                <tr>
                    <td style="text-align: right; vertical-align: middle;">
                        <asp:Button ID="Btn_Submit" runat="server" Text="Submit" />
                    </td>
                    <td style="text-align: left; vertical-align: middle;">
                        <asp:Button ID="Btn_Reset" runat="server" Text="Reset" />
                        <br />
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
