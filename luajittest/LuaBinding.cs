// Test application to demonstrate integrating LuaJIT with C#
// Copyright (c) 2024 opticfluorine
// Licensed under the MIT License - see LICENSE file for details

using System.Runtime.InteropServices;

namespace luajittest;

/// <summary>
///     Partial bindings to the LuaJIT library.
/// </summary>
public static partial class LuaBinding
{
    public const string LibName = "luajit-5.1";

    public const int LUA_GLOBALSINDEX = -10002;

    public const int LUA_OK = 0;
    public const int LUA_YIELD = 1;
    public const int LUA_ERRRUN = 2;
    public const int LUA_ERRSYNTAX = 3;
    public const int LUA_ERRMEM = 4;
    public const int LUA_ERRERR = 5;

    public delegate int lua_CFunction(IntPtr luaState);
    
    [LibraryImport(LibName)]
    public static partial IntPtr luaL_newstate();

    [LibraryImport(LibName)]
    public static partial void luaL_openlibs(IntPtr luaState);

    [LibraryImport(LibName)]
    public static partial void lua_close(IntPtr luaState);

    [LibraryImport(LibName)]
    public static partial void lua_pushcclosure(IntPtr luaState, lua_CFunction f, int n);

    public static void lua_pushcfunction(IntPtr luaState, lua_CFunction f) => lua_pushcclosure(luaState, f, 0);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial void lua_getfield(IntPtr luaState, int index, string k);
    
    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial void lua_setfield(IntPtr luaState, int index, string k);

    public static void lua_getglobal(IntPtr luaState, string name) => lua_getfield(luaState, LUA_GLOBALSINDEX, name);
    
    public static void lua_setglobal(IntPtr luaState, string name) => lua_setfield(luaState, LUA_GLOBALSINDEX, name);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial int luaL_loadstring(IntPtr luaState, string s);

    [LibraryImport(LibName)]
    public static partial int lua_pcall(IntPtr luaState, int nargs, int nresults, int errfunc);

    public static int luaL_dostring(IntPtr luaState, string str)
    {
        var result = luaL_loadstring(luaState, str);
        if (result != LUA_OK) return result;
        return lua_pcall(luaState, 0, 0, 0);
    }

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial string lua_tolstring(IntPtr luaState, int index, IntPtr len);

    public static string lua_tostring(IntPtr luaState, int index) => lua_tolstring(luaState, index, IntPtr.Zero);

    [LibraryImport(LibName)]
    public static partial void lua_pop(IntPtr luaState, int n);

    [LibraryImport(LibName)]
    public static partial void lua_createtable(IntPtr luaState, int narr, int nrec);

    public static void lua_newtable(IntPtr luaState) => lua_createtable(luaState, 0, 0);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial void lua_pushstring(IntPtr luaState, string s);

    [LibraryImport(LibName)]
    public static partial void lua_settable(IntPtr luaState, int idx);
}