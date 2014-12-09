using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;

namespace Imagine.Rest.Model.Dr {
  public class CallDataRecord<T> {

    public List<T> Find(int lobId, string customerAccountId, string[] deviceId, DateTime dateStart, DateTime dateEnd, int limit, int offSet) { return null; }

    internal static void PopulateDataSet(DataSet dataSet, OracleCommand oracleCommand) {
      var adapter = new OracleDataAdapter(oracleCommand);
      adapter.Fill(dataSet);
    }

  }
}