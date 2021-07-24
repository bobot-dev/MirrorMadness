using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MirrorMadness
{
    public class Module : ETGModule
    {
        public static readonly string MOD_NAME = "Mirror Madness";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";

        public override void Start()
        {

            var StartHook = new Hook(
                typeof(BehaviorSpeculator).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(Module).GetMethod("StartHookSB", BindingFlags.Static | BindingFlags.NonPublic));

            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        private static void StartHookSB(Action<BehaviorSpeculator> orig, BehaviorSpeculator self)
        {
            float hp = 15;

            if (self.healthHaver)
            {
                hp = self.healthHaver.GetMaxHealth() / 3;

            }

            if (!self.healthHaver.IsBoss)
            {
                self.AttackBehaviors.Add(new MirrorImageBehavior
                {
                    NumImages = 2,
                    MaxImages = 3,
                    MirrorHealth = hp,
                    SpawnDelay = 0.5f,
                    SplitDelay = 1,
                    SplitDistance = 1.25f,
                    AnimRequiresTransparency = true,
                    Cooldown = 8,

                });
            }
           
            orig(self);
        }

        public static void Log(string text, string color="FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
