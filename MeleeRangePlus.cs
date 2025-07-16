using UnityEngine;
using GadgetCore.API;
using System.Collections.Generic;
using GadgetCore;
using GadgetCore.Util;
using System.Reflection;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

namespace MeleeRangePlus
{
    [Gadget("MeleeRangePlus", RequiredOnClients: false)]
    public class MeleeRangePlus : Gadget
    {
        public const string MOD_VERSION = "1.0"; // Set this to the version of your mod.
        public const string CONFIG_VERSION = "1.0"; // Increment this whenever you change your mod's config file.

        public static GadgetLogger logger;

        public static ItemInfo GearModItem;
        public static int CurrentGearModCount = 0;
        public static Transform HeldAppearanceParent;

        private static Vector2 attackCube1BaseSize = Vector2.zero;
        private static Vector2 attackCube2BaseSize;
        private static Vector2 attackCube3BaseSize;

        public static void Log(string text)
        {
            logger.Log(text);
        }

        protected override void LoadConfig()
        {
            logger = base.Logger;

            Config.Load();

            string fileVersion = Config.ReadString("ConfigVersion", CONFIG_VERSION, comments: "The Config Version (not to be confused with mod version)");

            if (fileVersion != CONFIG_VERSION)
            {
                Config.Reset();
                Config.WriteString("ConfigVersion", CONFIG_VERSION, comments: "The Config Version (not to be confused with mod version)");
            }

            Config.Save();
        }

        protected override void Initialize()
        {
            Logger.Log(Info.Mod.Name + " v" + Info.Mod.Version);

            // materials
            GearModItem = new ItemInfo(
                Type: ItemType.MOD,
                Name: "MeleeRange+",
                Desc: "GEAR MOD \nAttach to weapons \nand armor in Mech City.",
                Tex: GadgetCoreAPI.LoadTexture2D("GearMod.png"),
                Value: 15);
            ItemRegistry.Singleton.Register(GearModItem, "MeleeRangePlus"); // Registry names must be alphanumeric
        }

        public static void GetReferencesAndApplyGearMods()
        {
            var player = InstanceTracker.PlayerScript;
            if (attackCube1BaseSize == Vector2.zero)
            {
                attackCube1BaseSize = player.attackCube.transform.localScale;
                attackCube2BaseSize = player.attackCube2.transform.localScale;
                attackCube3BaseSize = player.attackCube3.transform.localScale;
            }
            if (HeldAppearanceParent == null)
            {
                var plane4 = player.transform.Find("e/newplayer/Plane_004");
                // add an empty object to be a parent of the sword, so we can scale that, since the animation prevents us scaling the weapon directly.
                HeldAppearanceParent = new GameObject("MeleeRangePlus Animation Parent").transform;
                HeldAppearanceParent.SetParent(plane4);
                HeldAppearanceParent.localPosition = Vector3.zero;
                HeldAppearanceParent.localRotation = Quaternion.identity;
                var swordAppearance = plane4.Find("Plane_005");
                swordAppearance.SetParent(HeldAppearanceParent);
                var chainGun = plane4.Find("chaingun");
                chainGun.SetParent(HeldAppearanceParent);
                chainGun.localScale *= 0.5f; // no idea why
                var magma = plane4.Find("magma");
                magma.SetParent(HeldAppearanceParent);
                magma.localScale *= 0.5f; // no idea why
            }

            // each gear mod adds a percentage to each melee hitbox's base size
            float scaleMultiplier = 1f + CurrentGearModCount * 0.2f;
            player.attackCube.transform.localScale = attackCube1BaseSize * scaleMultiplier;
            player.attackCube2.transform.localScale = attackCube2BaseSize * scaleMultiplier;
            player.attackCube3.transform.localScale = attackCube3BaseSize * scaleMultiplier;

        }
    }
}