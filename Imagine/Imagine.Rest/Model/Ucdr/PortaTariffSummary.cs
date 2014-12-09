using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Oracle.DataAccess.Client;
using Imagine.Rest.Data;

namespace Imagine.Rest.Model.Dr {
  public class PortaTariffSummary {
    #region Properties

    public String DialCodeGroupName { get; set; }
    public String IsoDialCode { get; set; }
    public String TimeCategory { get; set; }
    public double FirstPeriod { get; set; }
    public double FirstCharge { get; set; }
    public double NextPeriod { get; set; }
    public double NextCharge { get; set; }


    #endregion

    internal List<PortaTariffSummary> Find(string callplanId, string fromEmail, string toEmail, string subject) {
      IEnumerable<PortaTariffSummary> result = new List<PortaTariffSummary>();

      using (var connection = new OracleConnection(Properties.Settings.Default.Dr_Reporting)) {
        connection.Open();
        var p = new OracleDynamicParameters();
        p.Add("v_DialCodeGroupName", DialCodeGroupName);
        p.Add("v_IsoDialCode", IsoDialCode);
        p.Add("v_TimeCategory", TimeCategory);
        p.Add("v_subject", FirstPeriod);
        p.Add("v_FirstPeriod", FirstCharge);
        p.Add("v_NextPeriod", NextPeriod);
        p.Add("v_NextCharge", NextCharge);

        using (var multi = connection.QueryMultiple("GET_TARIFF_BY_CALLPLAN", param: p, commandType: CommandType.StoredProcedure)) {
          dynamic data = multi.Read();
          result = PopulateSummaryCdr(data);
        }
        return result.ToList();
      }

      return result.ToList();
    }

    /// <summary>
    /// Depreciated: Populates the Summary CDR.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public List<TariffSummary> PopulateSummaryCdr(DataSet data) {
      var tarrifSummarys = new List<TariffSummary>();
      for (int i = 0; i < data.Tables[0].Rows.Count; i++) {
        var tarrifSummary = new TariffSummary();
        tarrifSummary.DialCodeGroupName = data.Tables[0].Rows[i][0].ToString();
        tarrifSummary.IsoDialCode = data.Tables[0].Rows[i][1].ToString();
        tarrifSummary.TimeCategory = data.Tables[0].Rows[i][2].ToString();
        tarrifSummary.FirstPeriod = data.Tables[0].Rows[i][3].ToString();
        tarrifSummary.FirstCharge = data.Tables[0].Rows[i][4].ToString();
        tarrifSummary.NextPeriod = data.Tables[0].Rows[i][5].ToString();
        tarrifSummary.NextCharge = data.Tables[0].Rows[i][6].ToString();
        tarrifSummarys.Add(tarrifSummary);
      }
      return tarrifSummarys;
    }
  }
}