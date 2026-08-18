using HarmonyLib;
using Il2CppTLD.Gear;
using UnityEngine.AddressableAssets;

namespace SurvivalBlueprintsIconFix;

public sealed class Main : MelonMod
{
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Survival Blueprints RU Icon Fix loaded.");
    }
}

[HarmonyPatch(typeof(BlueprintManager), nameof(BlueprintManager.LoadAllUserBlueprints))]
internal static class BlueprintManager_LoadAllUserBlueprints_Patch
{
    private const string TargetGearName = "GEAR_SharpeningStone";
    private const string ExistingInventoryIcon = "ico_GearItem__SharpeningStone";

    [HarmonyPostfix]
    private static void Postfix(BlueprintManager __instance)
    {
        if (__instance?.m_AllBlueprints == null)
            return;

        int fixedCount = 0;

        foreach (BlueprintData blueprint in __instance.m_AllBlueprints)
        {
            if (blueprint == null || blueprint.m_CraftedResultGear == null)
                continue;

            string gearName = blueprint.m_CraftedResultGear.gameObject?.name ?? string.Empty;
            if (!string.Equals(gearName, TargetGearName, StringComparison.OrdinalIgnoreCase))
                continue;

            blueprint.m_CraftingIcon = new AssetReferenceTexture2D(ExistingInventoryIcon);
            fixedCount++;
        }

        if (fixedCount > 0)
            MelonLogger.Msg($"Sharpening stone crafting icon fixed on {fixedCount} blueprint(s).");
    }
}
