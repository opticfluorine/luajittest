// Test application to demonstrate integrating LuaJIT with C#
// Copyright (c) 2024 opticfluorine
// Licensed under the MIT License - see LICENSE file for details

using static luajittest.LuaBinding;

int HelloWorld(IntPtr luaState)
{
    Console.Out.WriteLine("Hello World!");
    return 0;
}

var luaState = luaL_newstate();
luaL_openlibs(luaState);

lua_pushcfunction(luaState, HelloWorld);
lua_setglobal(luaState, "hello_world");

luaL_dostring(luaState, "hello_world()");

lua_close(luaState);

Console.Out.WriteLine("Complete.");
