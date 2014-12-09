using ScriptCs.JsonScheme;

namespace ScriptCs.Http {
  public class JsonSchemeScriptPack : IScriptPack {
    IScriptPackContext IScriptPack.GetContext() {
      return new JsonScheme();
    }

    void IScriptPack.Initialize(IScriptPackSession session) {
      session.ImportNamespace("System.Collections.Generic");
      session.ImportNamespace("Newtonsoft.Json");
    }
    void IScriptPack.Terminate() { }
  }
}