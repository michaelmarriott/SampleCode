using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.Rest.Model.Dr;
using Imagine.Rest.Model.Dr.Fakes;

namespace Imagine.Rest.Tests.Mocks {
  public class CallDataRecord {

    public static void PopulateInboundCallDataRecordDataSet() {
      ShimCallDataRecord<InboundCallDataRecord>.PopulateDataSetDataSetOracleCommand = (ds, oc) => {
        ds = new System.Data.DataSet(){};
      };
    }

    public static void PopulateInboundCallDataRecordSummaryDataSet() {
      ShimCallDataRecord<InboundCallDataRecordSummary>.PopulateDataSetDataSetOracleCommand = (ds, oc) => {
        ds = new System.Data.DataSet() { };
      };
    }

    public static void PopulateOutboundCallDataRecordDataSet() {
      ShimCallDataRecord<OutboundCallDataRecord>.PopulateDataSetDataSetOracleCommand = (ds, oc) => {
        ds = new System.Data.DataSet() { };
      };
    }

    public static void PopulateOutboundCallDataRecordSummaryDataSet() {
      ShimCallDataRecord<OutboundCallDataRecordSummary>.PopulateDataSetDataSetOracleCommand = (ds, oc) => {
        ds = new System.Data.DataSet() { };
      };
    }

  }
}
