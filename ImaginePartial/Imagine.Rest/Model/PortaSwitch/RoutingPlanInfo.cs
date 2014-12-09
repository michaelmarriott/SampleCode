using log4net;
using System.Linq;
using Imagine.Rest.Helper.Authentication;

namespace Imagine.Rest.Model.PortaSwitch {
  /// <summary> Routing Plan Info Lookup</summary>
  public class RoutingPlanInfo {
    private static readonly ILog log = LogManager.GetLogger(typeof(RoutingPlanInfo));
    public int Id { get; set; }
    public string Name { get; set; }

    public RoutingPlanInfo GetRoutesByName(string name) {
      var config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      int enviroment = int.Parse(config.Environment);
      RoutingPlanInfo result = null;
      if (!string.IsNullOrEmpty(name)) {
        using (Imagine.Rest.Data.Entities context = new Imagine.Rest.Data.Entities()) {
          var selectResult = from rp in context.ROUTINGPLANS
                             where rp.NAME == name && rp.I_ENV == enviroment
                             select rp;
          if (selectResult.Count() == 1) {
            result = new RoutingPlanInfo() { Id = selectResult.First().I_ROUTING_PLAN, Name = selectResult.First().NAME };
          }
        }
      }
      return result;
    }

    public RoutingPlanInfo GetRoutesById(int id) {
      var config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      int enviroment = int.Parse(config.Environment);
      RoutingPlanInfo result = null;
      if (id > 0) {
        using (Imagine.Rest.Data.Entities context = new Imagine.Rest.Data.Entities()) {
          var selectResult = from rp in context.ROUTINGPLANS
                             where rp.I_ROUTING_PLAN == id && rp.I_ENV == enviroment
                             select rp;
          if (selectResult.Count() == 1) {
            result = new RoutingPlanInfo() { Id = selectResult.First().I_ROUTING_PLAN, Name = selectResult.First().NAME };
          }
        }
      }
      return result;
    }
  }
}