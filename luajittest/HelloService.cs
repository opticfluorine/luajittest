namespace luajittest;

/// <summary>
///   Hello World as a service.
/// </summary>
public class HelloService(int value)
{
   /// <summary>
   ///   Does the Hello World thing.
   /// </summary>
   public int HelloWorld(IntPtr luaState)
   {
      Console.Out.WriteLine("Hello World!");
      Console.Out.WriteLine($"Value = {value}");
      return 0;
   } 
}