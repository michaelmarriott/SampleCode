using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Oracle.DataAccess.Client;
using Imagine.Rest.Data;

namespace Imagine.Rest.Model.Dr {
  public class OutboundCallDataRecord : CallDataRecord<OutboundCallDataRecord> {

    #region Properties
    public double BillRecordId { get; set; }

    public String Account { get; set; }

    public String DeviceId { get; set; }

    public String TrunkId { get; set; }

    public String CallType { get; set; }

    public String TimeCategory { get; set; }

    public double BillCost { get; set; }

    public double ActualCost { get; set; }

    public int Seconds { get; set; }

    public int BilledUnits { get; set; }

    public double Minutes { get; set; }

    public String DialledNumber { get; set; }

    public String CallSource { get; set; }

    public String CallConnected { get; set; }

    public String Description { get; set; }

    public String BillRunName { get; set; }

    public String ContractId { get; set; }

    #endregion
    public List<OutboundCallDataRecord> Find(int lobId, string customerAccountId, string[] deviceId, DateTime dateStart, DateTime dateEnd, int limit, int offSet) {
      var dataSet = GetData("GET_RATED_CDRS_ACCOUNT", lobId, customerAccountId, deviceId, dateStart, dateEnd, limit, offSet);
      return PopulateCdr(dataSet);
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
    /// Populates the CDR.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    private List<OutboundCallDataRecord> PopulateCdr(DataSet data) {
      var cdrCallRecordsView = new List<OutboundCallDataRecord>();
      if (data != null && data.Tables != null && data.Tables.Count > 0) {
        for (int i = 0; i < data.Tables[0].Rows.Count; i++) {
          var cdrCallRecord = new OutboundCallDataRecord();
          cdrCallRecord.Account = data.Tables[0].Rows[i][0].ToString();
          cdrCallRecord.BillCost = Convert.ToDouble(data.Tables[0].Rows[i][1].ToString()); //"Bill Cost"
          cdrCallRecord.CallSource = data.Tables[0].Rows[i][14].ToString(); //Call Source
          cdrCallRecord.DeviceId = data.Tables[0].Rows[i][2].ToString();
          cdrCallRecord.Seconds = int.Parse(data.Tables[0].Rows[i][3].ToString()); //"Seconds"
          cdrCallRecord.Minutes = Convert.ToDouble(data.Tables[0].Rows[i][4].ToString()); //"Minutes"
          cdrCallRecord.CallType = data.Tables[0].Rows[i][5].ToString(); //Call Type"
          cdrCallRecord.TimeCategory = data.Tables[0].Rows[i][6].ToString(); //"Time Category"
          cdrCallRecord.DialledNumber = data.Tables[0].Rows[i][7].ToString();//"Phone Number"
          cdrCallRecord.CallConnected = data.Tables[0].Rows[i][8].ToString();//Start Time
          cdrCallRecord.Description = data.Tables[0].Rows[i][9].ToString();//Description
          cdrCallRecord.BillRunName = data.Tables[0].Rows[i][10].ToString();//BillRunName
          if (data.Tables[0].Rows[i][11].ToString() != "") {
            cdrCallRecord.BillRecordId = double.Parse(data.Tables[0].Rows[i][11].ToString());//BillRecordId
          }
          cdrCallRecord.ContractId = data.Tables[0].Rows[i][12].ToString();//ContractId
          cdrCallRecord.ActualCost = double.Parse(data.Tables[0].Rows[i][13].ToString());//ActualCost
          cdrCallRecordsView.Add(cdrCallRecord);
        }
      }
      return cdrCallRecordsView;
    }
  }
}