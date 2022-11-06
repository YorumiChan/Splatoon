﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
#nullable enable
namespace Splatoon.SplatoonScripting
{
    public abstract class SplatoonScript
    {
        /// <summary>
        /// Controller provides easy access to various helper functions that may be helpful for your script.
        /// </summary>
        public Controller Controller { get; } = new();

        /// <summary>
        /// Metadata of a script that contains name and, optionally, author, description, version and script's origin website. This data will be displayed in Splatoon's interface.
        /// </summary>
        public abstract Metadata Metadata { get; }

        /// <summary>
        /// Valid territories where script will be executed. Specify an empty array if you want it to work in all territories. 
        /// </summary>
        public abstract HashSet<uint> ValidTerritories { get; }

        /// <summary>
        /// Indicates whether script is currently enabled and should be executed or not.
        /// </summary>
        public bool IsEnabled => ValidTerritories.Contains(Svc.ClientState.TerritoryType);

        /// <summary>
        /// Executed once after script is compiled and loaded into memory. Setup your layouts, elements and other static data that is not supposed to change within a game session. You should not setup any hooks or direct Dalamud events here, as method to cleanup is not provided (by design). Such things are to be done in OnEnable method.
        /// </summary>
        public virtual void OnSetup() { }

        /// <summary>
        /// Executed when player enters whitelisted territory. Will not trigger when player moves from one whitelisted territory to another.
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// Executed when player leaves whitelisted territory. Will not trigger when player moves from one non-whitelisted territory to another.
        /// </summary>
        public virtual void OnDisable() { }

        /// <summary>
        /// Will be called on combat start. This method will only be called if a script is enabled.
        /// </summary>
        public virtual void OnCombatStart() { }

        /// <summary>
        /// Will be called on combat end. This method will only be called if a script is enabled.
        /// </summary>
        public virtual void OnCombatEnd() { }

        /// <summary>
        /// Will be called on phase change. This method will only be called if a script is enabled. This method will be called if user manually changes phase as well.
        /// </summary>
        /// <param name="newPhase">New phase</param>
        public virtual void OnPhaseChange(int newPhase) { }

        /// <summary>
        /// Will be called on receiving map effect. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="Position">Positional data of map effect. It is not related to actual map coordinates.</param>
        /// <param name="Param1">First parameter of map effect.</param>
        /// <param name="Param2">Second parameter of map effect.</param>
        public virtual void OnMapEffect(uint Position, ushort Param1, ushort Param2) { }

        /// <summary>
        /// Will be called when a tether created between two game objects. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="source">Source object ID of pair.</param>
        /// <param name="target">Target object ID of pair.</param>
        public virtual void OnTetherCreate(uint source, uint target) { }

        /// <summary>
        /// Will be called when a previously created tether between two game objects removed. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="source">Source object ID of pair.</param>
        public virtual void OnTetherRemoval(uint source) { }

        /// <summary>
        /// Will be called when a VFX spawns on a certain game object. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="target">Object ID that is targeted by VFX.</param>
        /// <param name="vfxPath">VFX game path</param>
        public virtual void OnVFXSpawn(uint target, string vfxPath) { }

        /// <summary>
        /// Will be called whenever plugin processes a message. These are the same messages which layout trigger system receives. This method will only be called if a script is enabled.
        /// </summary>
        /// <param name="Message"></param>
        public virtual void OnMessage(string Message) { }

        /// <summary>
        /// Will be called every framework update. You can execute general logic of your script here. 
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// If you override this method, settings section will be added to your script. You can call ImGui methods in this function to draw configuration UI. Keep it simple.
        /// </summary>
        public virtual void OnSettingsDraw() { }

        public bool DoSettingsDraw => this.GetType().GetMethod(nameof(OnSettingsDraw))?.DeclaringType != typeof(SplatoonScript);
    }
}
