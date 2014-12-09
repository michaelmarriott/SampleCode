using System;

namespace Imagine.Rest.Helper {

  /// <summary>
  /// Helper class for generating a random password
  /// </summary>
  public class RandomPasswordGenerator {

    /// <summary>
    /// Creates a random AlphaNumeric password containing as many characters as specified. The password will attempt to have at least one number.
    /// </summary>
    /// <returns>A random password of AlphaNumeric characters</returns>
    public static string CreateRandomPassword(int numberOfCharacters) {
      var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
      var password = new char[numberOfCharacters];
      Random rand = new Random(System.Environment.TickCount);
      for (int i = 0; i < password.Length; i++) {
        password[i] = alpha[rand.Next(alpha.Length)];
      }
      int numbers = rand.Next(password.Length / 2) + 1;
      for (int i = 0; i < numbers; i++) {
        int randomPosition = rand.Next(password.Length);
        password[randomPosition] = "0123456789"[rand.Next(10)];
      }
      return new String(password);
    }
  }
}