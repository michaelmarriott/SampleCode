using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Oracle.DataAccess.Client;
using Imagine.Rest.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Imagine.Rest.Model.Dr;

namespace Imagine.Rest.Model.Dr {
  public class InboundCallDataRecordSummary : CallDataRecord<InboundCallDataRecordSummary> {
   
    #region Properties

    public String Account { get; set; }

    public String ContractId { get; set; }

    public String DeviceId { get; set; }

    public String CallType { get; set; }

    public String TimeCategory { get; set; }

    public String BillRunName { get; set; }

    public double BillCost { get; set; }

    public double ActualCost { get; set; }

    public int Seconds { get; set; }

    public int BilledUnits { get; set; }

    public double Minutes { get; set; }

    public int CallCount { get; set; }

    #endregion

    internal List<InboundCallDataRecordSummary> Find(int lobId, string customerAccountId, string[] deviceId, DateTime dateStart, DateTime dateEnd, int limit, int offSet) {
      var dataSet = GetData("GET_INCOMING_CDRS_SUM_ACCOUNT", lobId, customerAccountId, deviceId, dateStart, dateEnd, limit, offSet);
      return PopulateSummaryCdr(dataSet);
    }

    internal DataSet GetData(string storedProc, int lobId, string customerAccountId, string[] deviceId, DateTime dateStart, DateTime dateEnd, int limit, int offSet) {
      var dataSet = new DataSet();
      using (var connection = new OracleConnection(Properties.Settings.Default.Dr)) {
        connection.Open();
        var oracleCommand = new OracleCommand(storedProc, connection);
        oracleCommand.CommandType = CommandType.StoredProcedure;
        oracleCommand.Parameters.Add("v_lobid", OracleDbType.Int32, lobId, ParameterDirection.Input);
        if (customerAccountId != null)
          oracleCommand.Parameters.Add("v_accountids", OracleDbType.Long, string.Join(",", customerAccountId), ParameterDirection.Input);
        else
          oracleCommand.Parameters.Add("v_accountids", OracleDbType.Long, "", ParameterDirection.Input);
        if (deviceId != null)
          oracleCommand.Parameters.Add("v_deviceids", OracleDbType.Long, string.Join(",", deviceId), ParameterDirection.Input);
        else
          oracleCommand.Parameters.Add("v_deviceids", OracleDbType.Long, "", ParameterDirection.Input);
        oracleCommand.Parameters.Add("v_startdate", OracleDbType.Date, dateStart, ParameterDirection.Input);
        oracleCommand.Parameters.Add("v_enddate", OracleDbType.Date, dateEnd, ParameterDirection.Input);
        oracleCommand.Parameters.Add("myrecords_type", OracleDbType.RefCursor, null, ParameterDirection.Output);
        oracleCommand.Parameters.Add("v_offset", OracleDbType.Int32, offSet, ParameterDirection.Input);
        oracleCommand.Parameters.Add("v_limit", OracleDbType.Int32, limit, ParameterDirection.Input);
        PopulateDataSet(dataSet, oracleCommand);
      }
      return dataSet;
    }

    /// <summary>
    /// Populates the Summary CDR.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public List<InboundCallDataRecordSummary> PopulateSummaryCdr(DataSet data) {
      var cdrCallRecordsSummaryView = new List<InboundCallDataRecordSummary>();
      if (data != null && data.Tables != null && data.Tables.Count > 0) {
        for (int i = 0; i < data.Tables[0].Rows.Count; i++) {
          var cdrCallRecordSummary = new InboundCallDataRecordSummary();
          cdrCallRecordSummary.Account = data.Tables[0].Rows[i][0].ToString(); //Call Type"
          cdrCallRecordSummary.CallCount = int.Parse(data.Tables[0].Rows[i][1].ToString()); //Call Count
          if (data.Tables[0].Rows[i][2] != null && data.Tables[0].Rows[i][2].ToString() != "") {
            cdrCallRecordSummary.BillCost = double.Parse(data.Tables[0].Rows[i][2].ToString()); //"Bill Cost"
          } else {
            cdrCallRecordSummary.BillCost = 0;
          }
          cdrCallRecordSummary.DeviceId = data.Tables[0].Rows[i][3].ToString(); //DeviceId
          if (data.Tables[0].Rows[i][4] != null && data.Tables[0].Rows[i][4].ToString() != "") {
            cdrCallRecordSummary.Seconds = int.Parse(data.Tables[0].Rows[i][4].ToString()); //"Seconds"
          } else {
            cdrCallRecordSummary.Seconds = 0;
          }
          if (data.Tables[0].Rows[i][5] != null && data.Tables[0].Rows[i][5].ToString() != "") {
            cdrCallRecordSummary.Minutes = Convert.ToDouble(data.Tables[0].Rows[i][5].ToString()); //"Minutes"
          } else {
            cdrCallRecordSummary.Minutes = 0;
          }
          cdrCallRecordSummary.CallType = data.Tables[0].Rows[i][6].ToString(); //Call Type"
          cdrCallRecordSummary.TimeCategory = data.Tables[0].Rows[i][7].ToString(); //"Time Category"
          cdrCallRecordSummary.BillRunName = data.Tables[0].Rows[i][8].ToString(); //BillRun
          cdrCallRecordSummary.ContractId = data.Tables[0].Rows[i][9].ToString(); //ContractId
          if (data.Tables[0].Rows[i][10] != null && data.Tables[0].Rows[i][10].ToString() != "") {
            cdrCallRecordSummary.ActualCost = double.Parse(data.Tables[0].Rows[i][10].ToString()); //ActualCost
          } else {
            cdrCallRecordSummary.ActualCost = 0;
          }
          cdrCallRecordsSummaryView.Add(cdrCallRecordSummary);
        }
      }
      return cdrCallRecordsSummaryView;
    }

  }
}