using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Imagine.Rest.ViewModel.Ucdr;

namespace Imagine.Rest.Scripts {
  public class SchemaGenerate {

    public void SchemaGenerate() {
      var jsonSchemaGenerator = new JsonSchemaGenerator();

      Assembly assembly = Assembly.LoadFrom("../bin/Imagine.Rest.dll");

      var x = from t in assembly.GetTypes()
              where t.IsDefined(typeof(JsonObjectAttribute),false)
              select t;

      typeof(JsonObjectAttribute).

      Newtonsoft.Json.JsonObjectAttribute j = new Newtonsoft.Json.JsonObjectAttribute();

      var types = from type in assembly.GetTypes()
                  where Attribute.IsDefined(type, typeof(JsonObjectAttribute).GetCustomAttribute)
                  select type;

      foreach (Type type in assembly.GetTypes()) {
        if (type.GetCustomAttributes(typeof(JsonObjectAttribute), true).Length > 0) {
          
        }

      }
    }

  }
}