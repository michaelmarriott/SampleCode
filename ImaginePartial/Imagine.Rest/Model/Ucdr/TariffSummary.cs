using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Oracle.DataAccess.Client;
using Imagine.Rest.Data;

namespace Imagine.Rest.Model.Dr {
  public class TariffSummary {
    #region Properties

    public Int64 Id { get; set; }
    public String Zone { get; set; }
    public string FirstPeriod { get; set; }
    public string FirstCharge { get; set; }
    public string TimeCategory { get; set; }
    public string DialCodeGroupName { get; set; }
    public string IsoDialCode { get; set; }
    public string NextPeriod { get; set; }
    public string NextCharge { get; set; }

    #endregion

    internal List<TariffSummary> Find(string zones, string callplanId) {
      var result = new List<TariffSummary>();
      var dataSet = new DataSet();
      using (var connection = new OracleConnection(Properties.Settings.Default.Dr_Reporting)) {
        connection.Open();
        var oracleCommand = new OracleCommand("GET_TARIFF_BY_CALLPLAN", connection);
        oracleCommand.CommandType = CommandType.StoredProcedure;
        oracleCommand.Parameters.Add("v_zones", zones);
        oracleCommand.Parameters.Add("v_callplanId", callplanId);
        oracleCommand.Parameters.Add("myrecords_type", OracleDbType.RefCursor, null, ParameterDirection.Output);
        FillDataSet(dataSet, oracleCommand);
        result = PopulateSummaryCdr(dataSet);
      }
      return result;
    }

    internal void FillDataSet(DataSet dataSet, OracleCommand oracleCommand) {
      var adapter = new OracleDataAdapter(oracleCommand);
      adapter.Fill(dataSet);
    }

    /// <summary>
    /// Depreciated: Populates the Summary CDR.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public List<TariffSummary> PopulateSummaryCdr(DataSet data) {
      var summaryView = new List<TariffSummary>();
      for (int i = 0; i < data.Tables[0].Rows.Count; i++) {
        var tariffSummary = new TariffSummary();
        tariffSummary.Id = Convert.ToInt64(data.Tables[0].Rows[i][0].ToString()); //Id"
        tariffSummary.DialCodeGroupName = data.Tables[0].Rows[i][1].ToString(); //DialCodeGroupName
        tariffSummary.IsoDialCode = data.Tables[0].Rows[i][2].ToString(); //"TIsoDialCode
        tariffSummary.TimeCategory = data.Tables[0].Rows[i][3].ToString(); //"Time Category"
        tariffSummary.FirstPeriod = data.Tables[0].Rows[i][4].ToString(); //FirstPeriod
        tariffSummary.FirstCharge = data.Tables[0].Rows[i][5].ToString(); //Call Type"
        tariffSummary.NextPeriod = data.Tables[0].Rows[i][6].ToString(); //NextPeriod
        tariffSummary.NextCharge = data.Tables[0].Rows[i][7].ToString(); //NextCharge
        tariffSummary.Zone = data.Tables[0].Rows[i][8].ToString(); //ZoneName
        summaryView.Add(tariffSummary);
      }
      return summaryView;
    }
  }
}
