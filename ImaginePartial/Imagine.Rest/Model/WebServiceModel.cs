namespace Imagine.Rest.Model {

  /// <summary>
  /// Interface which describes the methods available for WebService Models. Each Model is expected to provide the following functionality
  /// </summary>
  /// <typeparam name="T">Generic type of the model that implements the interface.</typeparam>
  public interface WebServiceModel<T> {

    /// <summary>
    /// Find a model by using the integer identifier provided
    /// </summary>
    /// <param name="id">Integer identifier for the model</param>
    /// <returns>The Model that matches the provided identifier, otherwise it will return null if no model exists</returns>
    T Find(int id);

    T Find(string id);

    bool Create();

    bool Save();

    bool Delete();
  }
}