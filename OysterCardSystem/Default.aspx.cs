using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OysterCardSystem.Core;

namespace OysterCardSystem
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                lblCurrentBal.Text = "£0";
                lblErr.Text = "";
                Session["cardbalance"] = 0;

                DataTable cardhistory = new DataTable();
                cardhistory.Columns.Add("sno");
                cardhistory.Columns.Add("initialCardBal");
                cardhistory.Columns.Add("journeytype");
                cardhistory.Columns.Add("startpoint");
                cardhistory.Columns.Add("endpoint");
                cardhistory.Columns.Add("charges");
                cardhistory.Columns.Add("finalcardbal");

                Session["cardhistory"] = cardhistory;

            }
        }

        protected void btnTopupCard_Click(object sender, EventArgs e)
        {
            int n;
            bool isNumeric = int.TryParse(txtAmount.Text.ToString(), out n);

            if (!string.IsNullOrWhiteSpace(txtAmount.Text.ToString()) && isNumeric)
            {
                float _amount = float.Parse(txtAmount.Text.ToString());
                float _currentAmount = float.Parse(Session["cardbalance"].ToString());

                var total = Math.Abs(_currentAmount + _amount).ToString();
                lblCurrentBal.Text = "£" + total;
                Session["cardbalance"] = total;
                txtAmount.Text = "";
            }
        }

        protected void btnJourneyFnished_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddFromPoint.SelectedValue == "0")
                {
                    lblErr.CssClass = "h4 text-danger";
                    lblErr.Text = "Please select journey Start point.";
                    return;
                }

                if (ddEndPoint.SelectedValue == "0")
                {
                    lblErr.CssClass = "h4 text-danger";
                    lblErr.Text = "Please select journey End point.";
                    return;
                }

                Transport _transport;

                if (ddJourneyType.SelectedValue == "1")
                    _transport = Transport.TUBE;
                else
                    _transport = Transport.BUS;

                if (ddFromPoint.SelectedValue != "0" && ddEndPoint.SelectedValue != "0")
                {
                    Zone _startZone = _transport == Transport.TUBE ? new Zone(GetStation(ddFromPoint.SelectedValue)) : null;
                    Zone _endZone = _transport == Transport.TUBE ? new Zone(GetStation(ddEndPoint.SelectedValue)) : null;

                    float _currentBal = float.Parse(Session["cardbalance"].ToString());
                    SmartCard card = new SmartCard(_currentBal);
                    Journey newJourney = new Journey(new JourneyFare());

                    newJourney.SetStartPoint(_transport, _startZone, card);
                    newJourney.SetEndPoint(_endZone);

                    float _remainingBalance = Math.Abs(card.GetBalance());
                    Session["cardbalance"] = _remainingBalance;
                    lblCurrentBal.Text = "£" + _remainingBalance.ToString();

                    AddJourneyLog(_transport.ToString(), ddFromPoint.SelectedItem.Text, ddEndPoint.SelectedItem.Text, _currentBal.ToString(), Math.Round(_currentBal - _remainingBalance, 2).ToString(), _remainingBalance.ToString());

                    lblErr.CssClass = "h4 text-success";
                    lblErr.Text = "Journey finished!";
                }

            }
            catch (FareException ex)
            {
                lblErr.CssClass = "h4 text-danger";
                lblErr.Text = ex.Message;

                DataTable cardhistory = (DataTable)Session["cardhistory"];
                rptCardLogs.DataSource = cardhistory;
                rptCardLogs.DataBind();
            }

        }

        private string GetStation(string stationName)
        {
            switch (stationName)
            {
                case "holburn":
                    return Station.HOLBORN;
                case "earlscourt":
                    return Station.EARLS_COURT;
                case "hammersmith":
                    return Station.HAMMERSMITH;
                case "wimbledon":
                    return Station.WIMBLEDON;
                default:
                    return null;
            }
        }

        private void AddJourneyLog(string journeyType, string startPoint, string endPoint, string initialCardBal, string charges, string finalCardBalance)
        {
            DataTable cardhistory = (DataTable)Session["cardhistory"];
            if (cardhistory != null)
            {
                int count = cardhistory.Rows != null ? cardhistory.Rows.Count + 1 : 1;

                cardhistory.Rows.Add(count, initialCardBal, journeyType, startPoint, endPoint, charges, finalCardBalance);

                rptCardLogs.DataSource = cardhistory;
                rptCardLogs.DataBind();
            }
        }
    }
}