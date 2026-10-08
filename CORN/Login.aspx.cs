using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using CornPointHelper;
using System.Xml;

public partial class Login : Page
{
    readonly UserController _mController = new UserController();
    readonly DistributorController _mDist = new DistributorController();
    ConfigurationController _cController = new ConfigurationController();
    public static DataTable dtConfig;

    protected void Page_Load(object sender, EventArgs e)
    {
        SqlHelper.GetAppPath();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        Session.Remove("LicenseMessage");
        if (!Page.IsPostBack)
        {
            dtConfig = _cController.SelectAppConfiguration(1, (int)Enums.AppSettingMaster.WebSettings, null, Constants.IntNullValue);
            try
            {
                if (Convert.ToInt32(Session["UserID"]) > 0)
                {
                    Response.Redirect("Forms/Home.aspx");
                }
            }
            catch (Exception)
            {
                txtLogin.Focus();
            }
            int days = 0;
            int max_days = 0;
            DataTable dtLicenseData = _mDist.GetLicenseData();
            if (dtLicenseData.Rows.Count > 0)
            {
                DateTime dtMaxClosingDate = Constants.DateNullValue;
                DateTime dtMaxDate = Constants.DateNullValue;
                if (dtLicenseData.Rows[0]["IsLicenseImplemented"].ToString() == "1")
                {
                    try
                    {
                        dtMaxClosingDate = Convert.ToDateTime(dtLicenseData.Rows[0]["MaxClosingDate"]);
                        dtMaxDate = Convert.ToDateTime(Cryptography.Decrypt(dtLicenseData.Rows[0]["MAX_DIAS"].ToString(), Constants.CryptographyKey));
                    }
                    catch (Exception exp)
                    {
                        lblLicenseMsg.Text = "Your license has been expired, Please Contact to Corn POS Team";
                        btnSignIn.Visible = false;
                        txtLogin.Visible = false;
                        txtPassword.Visible = false;
                        dvLicense.Visible = true;
                    }

                    if (dtMaxClosingDate >= dtMaxDate)
                    {
                        lblLicenseMsg.Text = "Your license has been expired, Please Contact to Corn POS Team";
                        btnSignIn.Visible = false;
                        txtLogin.Visible = false;
                        txtPassword.Visible = false;
                        dvLicense.Visible = true;
                    }
                    else
                    {
                        if ((dtMaxDate - dtMaxClosingDate).TotalDays <= 5)
                        {
                            double _remaingindays = (dtMaxDate - dtMaxClosingDate).TotalDays;
                            lblLicenseMsg.Text = string.Format("Your license will be expired after {0} Day(s), Please Contact to Corn POS Team", _remaingindays);
                            dvLicense.Visible = true;
                        }
                        txtLogin.Focus();
                    }
                }
                else
                {
                    txtLogin.Focus();
                }
            }
            else
            {
                lblLicenseMsg.Text = "Your license has been expired, Please Contact to Corn POS Team";
                btnSignIn.Visible = false;
                txtLogin.Visible = false;
                txtPassword.Visible = false;
                dvLicense.Visible = true;
            }
        }
    }
    private void ValidateUser()
    {

        if (txtLogin.Text == "" && txtPassword.Text == "")
        {
            lblErrorMsg.Text = "Plz enter User name / Password";
            dvError.Visible = true;
            return;

        }
        else
        {
            DataTable dt = null;
            DataRow[] dr = dtConfig.Select("CODE = '" + (int)Enums.AppSetting.IsEncreptedCredentials + "'");
            bool IsEncrypted = Convert.ToBoolean(Convert.ToInt32(dr[0]["VALUE"]));

            if (IsEncrypted)
            {
                dt = _mController.SelectSlashUserEncrypt(txtLogin.Text, Cryptography.Encrypt(txtPassword.Text, Constants.CryptographyKey));
            }
            else
            {
                dt = _mController.SelectSlashUser(txtLogin.Text, txtPassword.Text);
            }
            //dt = _mController.SelectSlashUser(txtLogin.Text, txtPassword.Text);
            Session.Clear();

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["IS_ACTIVE"]) == "True" || Convert.ToString(dt.Rows[0]["IS_ACTIVE"]) == "Active")
                {
                    ArrayList list = new ArrayList();
                    {
                        list.Add(dt.Rows[0]["ROLE_ID"].ToString());
                        list.Add(dt.Rows[0]["USER_DETAIL"].ToString());
                    }
                    Session["EmployeeInfo"] = list;
                    DataTable dtOne = _cController.SelectAppConfiguration(1, (int)Enums.AppSettingMaster.InventorySetting, null, Constants.IntNullValue);

                    dr = dtOne.Select(string.Format("CODE = '{0}'", (int)Enums.AppSetting.ShowClosingStockStatus));
                    bool ClosingStockStatus = Convert.ToBoolean(Convert.ToInt32(dr[0]["VALUE"]));

                    Session.Add("VALIDATE_STOCK", ClosingStockStatus);
                    Session.Add("UserID", Convert.ToInt32(dt.Rows[0]["USER_ID"].ToString()));
                    Session.Add("UserName", dt.Rows[0]["USER_DETAIL"].ToString());
                    Session.Add("DISTRIBUTOR_ID", Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                    Session.Add("CompanyId", Convert.ToInt32(dt.Rows[0]["COMPANY_ID"].ToString()));
                    Session.Add("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());
                    Session.Add("ADDRESS1", dt.Rows[0]["ADDRESS1"].ToString());
                    Session.Add("PHONE", dt.Rows[0]["PHONE"].ToString());
                    Session.Add("RoleID", Convert.ToInt32(dt.Rows[0]["ROLE_ID"]));
                    Session.Add("IS_SystemAdmin", Convert.ToInt32(dt.Rows[0]["IS_SystemAdmin"]));
                    Session.Add("IS_CanReverseDayClose", Convert.ToBoolean(dt.Rows[0]["IS_CanReverseDayClose"]));
                    Session.Add("IS_CanGiveDiscount", Convert.ToBoolean(dt.Rows[0]["IS_CanGiveDiscount"]));
                    Session.Add("Division", Convert.ToInt32(dt.Rows[0]["ISCURRENT"]));
                    Session.Add("UserType", dt.Rows[0]["USER_TYPE_ID"].ToString());
                    Session.Add("TodayMenuID", dt.Rows[0]["TodayMenuID"].ToString());
                    LastClosedDay(Convert.ToInt32(dt.Rows[0]["USER_ID"]), Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                    long userLogId = _mController.InsertUserLoginTime(Convert.ToInt32(dt.Rows[0]["USER_ID"]));
                    Session.Add("User_Log_ID", userLogId);
                    Session.Add("LicenseMessage", lblLicenseMsg.Text);
                    if (dt.Rows[0]["ROLE_NAME"].ToString() == "Tab")
                    {
                        Response.Redirect("Forms/frmOrderTaking.aspx");
                    }
                    else
                    {
                        Response.Redirect("Forms/Home.aspx");
                    }
                }
                else
                {
                    lblErrorMsg.Text = "This User is In Active";
                    dvError.Visible = true;
                }
            }
            else
            {
                lblErrorMsg.Text = "Wrong User Id/Password";
                dvError.Visible = true;
            }
        }
    }
    private void LastClosedDay(int userId, int pDistributor)
    {
        DataTable dt = _mDist.SelectMaxDayClose(userId, pDistributor);
        Session.Add("CurrentWorkDate", dt.Rows.Count > 0 ? DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString()) : DateTime.Now);
    }

    private bool CheckDeplyed()
    {
        try
        {
            DataRow[] dr1 = dtConfig.Select("CODE = '" + (int)Enums.AppSetting.IsDeployed + "'");
            bool deployed = Convert.ToBoolean(Convert.ToInt32(dr1[0]["VALUE"]));
            if (deployed)
            {
                return true;
            }
            if (dtConfig.Rows.Count > 0)
            {
                DataRow[] dr = dtConfig.Select("CODE = '" + (int)Enums.AppSetting.ComputerInfo + "'");
                string CInfo = Cryptography.Encrypt(ComputerInfo.Value(), Constants.CryptographyKey);
                string[] ComInfo = (dr[0]["VALUE"].ToString()).Split(',');
                int length = ComInfo.Length;
                bool ComCheck = false;
                for (int i = 0; i < length; i++)
                {
                    if (ComInfo[i].ToString().Trim() == CInfo)
                    {
                        ComCheck = true;
                        break;
                    }
                }
                return ComCheck;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
            return false;
        }
    }
    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        bool Check = CheckDeplyed();
        if (Check)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["IsDeployment"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["IsDeployment"].ToString() == "1")
                {
                    int DistributorID = Convert.ToInt32(DataControl.chkNull_Zero(System.Configuration.ConfigurationManager.AppSettings["DistributorID"].ToString()));
                    try
                    {
                        ValidateUser();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    ValidateUser();
                }
            }
            else
            {
                ValidateUser();
            }
        }
        else
        {
            dvError.Visible = true;
            lblErrorMsg.Text = "Invalid Key. Contact to Administrator.";
        }
    }

}