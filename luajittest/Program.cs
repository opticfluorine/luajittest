// Test application to demonstrate integrating LuaJIT with C#
// Copyright (c) 2024 opticfluorine
// Licensed under the MIT License - see LICENSE file for details

using luajittest;

var host = new LuaHost(new HelloService(42));
try
{
    host.RunString("cs_host.hello_world()");
}
catch (LuaException e)
{
    Console.Error.WriteLine($"Lua error: {e.Message}");
}
