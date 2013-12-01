/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

#include <windows.h>
#include <shlwapi.h>
#include <stdio.h>
#include <string.h>

#include "game.hpp"
#include "patch.hpp"

#define UINT32(x) (*(unsigned int *)x)

typedef int (__cdecl *LUA_GETTOP)(void *);
typedef const char * (__cdecl *LUA_TOLSTRING)(void *, int, size_t *);

LUA_GETTOP lua_gettop = NULL;
LUA_TOLSTRING lua_tolstring = NULL;

int __cdecl DebugPrint(void *L)
{
	const char *msg;
	
	msg = lua_tolstring(L, 1, NULL);
	if (_strcmpi(msg, "vint") == 0)
	{
		msg = lua_tolstring(L, 2, NULL);
	}

	printf("%s", msg);
	return 0;
}

bool HookGame(void)
{
	printf("hooking...\n");

	unsigned int debugPrintAddress = (unsigned int)&DebugPrint;

	PatchCode(0x00f56773, &debugPrintAddress, 4);
	lua_gettop = (LUA_GETTOP)0x00FE2070;
	lua_tolstring = (LUA_TOLSTRING)0x00FE2460;

	printf("hooked.\n");

	return true;
}

bool GameAttach(void)
{
	AllocConsole();
	FILE *dummy;
	freopen_s(&dummy, "CONIN$", "rb", stdin);
	freopen_s(&dummy, "CONOUT$", "wb", stdout);
	freopen_s(&dummy, "CONOUT$", "wb", stderr);

	printf("Allocated console.\n");

	HookGame();

	return true;
}

void GameDetach(void)
{
}
