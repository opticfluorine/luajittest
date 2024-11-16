using System.Runtime.InteropServices;
using static luajittest.LuaBinding;

namespace luajittest;

/// <summary>
///     Exception type thrown when Lua encounters an error.
/// </summary>
public class LuaException : Exception
{
    public LuaException(string? message) : base(message)
    {
    }

    public LuaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     Lightweight host class for a Lua instance.
/// </summary>
public class LuaHost : IDisposable
{
    private readonly HelloService _helloService;
    private readonly IntPtr _luaState;
    
    /// <summary>
    ///     Name for the Lua table/package that will be loaded to provide host access.
    /// </summary>
    private const string LuaPackage = "cs_host";

    public LuaHost(HelloService helloService)
    {
        _helloService = helloService;
        _luaState = luaL_newstate();
        
        ConfigureLua();
    }

    /// <summary>
    ///     Lua state handle.
    /// </summary>
    public IntPtr LuaState => _luaState;
    
    public void Dispose()
    {
        lua_close(_luaState);
    }

    /// <summary>
    ///     Executes the given string as Lua code.
    /// </summary>
    /// <param name="str">String to execute.</param>
    /// <exception cref="LuaException">Thrown if Lua encounters an error.</exception>
    public void RunString(string str)
    {
        var result = luaL_dostring(_luaState, str);
        if (result != LUA_OK) throw new LuaException($"Error {result}: {GetErrorMessage()}");
    }

    /// <summary>
    ///     Configures Lua to expose the correct APIs.
    /// </summary>
    private void ConfigureLua()
    {
        // Load the Lua standard library.
        luaL_openlibs(_luaState);
        
        // Create table to hold the custom library.
        lua_newtable(_luaState);
        lua_setglobal(_luaState, LuaPackage);
        
        // Populate the custom library with functions.
        lua_getglobal(_luaState, LuaPackage);
        lua_pushstring(_luaState, "hello_world");
        lua_pushcfunction(_luaState, _helloService.HelloWorld);
        lua_settable(_luaState, -3);
    }

    /// <summary>
    ///     Gets the error message from Lua. Behavior undefined if an error message is not
    ///     at the top of the Lua stack.
    /// </summary>
    /// <returns>Error message.</returns>
    private string GetErrorMessage()
    {
        var message = lua_tostring(_luaState, -1);
        lua_pop(_luaState, 1);
        return message;
    }
}