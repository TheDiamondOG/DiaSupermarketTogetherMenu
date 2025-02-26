using HarmonyLib;
using Mirror;
using System.Reflection;

namespace SupermarketTogetherKacker.Patches
{
    [HarmonyPatch(typeof(NetworkBehaviour), "SendCommandInternal")]
    public class RequireAuthorityPatch
    {
        static bool Prefix(ref bool requiresAuthority)
        {
            requiresAuthority = false;
            return true;
        }
    }

}