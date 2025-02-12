﻿using System;
using System.Runtime.InteropServices;
using SWLOR.Game.Server.NWN.Enum;

namespace SWLOR.Game.Server.NWN
{
    public partial class Effect
    {
        public IntPtr Handle;
        public Effect(IntPtr handle) => Handle = handle;
        ~Effect() { Internal.NativeFunctions.FreeGameDefinedStructure((int)EngineStructure.Effect, Handle); }

        public static implicit operator IntPtr(Effect effect) => effect.Handle;
        public static implicit operator Effect(IntPtr intPtr) => new Effect(intPtr);
    }

    public partial class Event
    {
        public IntPtr Handle;
        public Event(IntPtr handle) => Handle = handle;
        ~Event() { Internal.NativeFunctions.FreeGameDefinedStructure((int)EngineStructure.Event, Handle); }

        public static implicit operator IntPtr(Event effect) => effect.Handle;
        public static implicit operator Event(IntPtr intPtr) => new Event(intPtr);
    }

    public partial class Location
    {
        public IntPtr Handle;
        public Location(IntPtr handle) => Handle = handle;
        ~Location() { Internal.NativeFunctions.FreeGameDefinedStructure((int)EngineStructure.Location, Handle); }

        public static implicit operator IntPtr(Location effect) => effect.Handle;
        public static implicit operator Location(IntPtr intPtr) => new Location(intPtr);
    }

    public partial class Talent
    {
        public IntPtr Handle;
        public Talent(IntPtr handle) => Handle = handle;
        ~Talent() { Internal.NativeFunctions.FreeGameDefinedStructure((int)EngineStructure.Talent, Handle); }

        public static implicit operator IntPtr(Talent effect) => effect.Handle;
        public static implicit operator Talent(IntPtr intPtr) => new Talent(intPtr);
    }

    public partial class ItemProperty
    {
        public IntPtr Handle;
        public ItemProperty(IntPtr handle) => Handle = handle;
        ~ItemProperty() { Internal.NativeFunctions.FreeGameDefinedStructure((int)EngineStructure.ItemProperty, Handle); }

        public static implicit operator IntPtr(ItemProperty effect) => effect.Handle;
        public static implicit operator ItemProperty(IntPtr intPtr) => new ItemProperty(intPtr);
    }

    public partial class SQLQuery
    {
        public IntPtr Handle;
        public SQLQuery(IntPtr handle) => Handle = handle;
        ~SQLQuery() { Internal.NativeFunctions.FreeGameDefinedStructure((int)EngineStructure.SQLQuery, Handle); }

        public static implicit operator IntPtr(SQLQuery effect) => effect.Handle;
        public static implicit operator SQLQuery(IntPtr intPtr) => new SQLQuery(intPtr);
    }

    public delegate void ActionDelegate();
}