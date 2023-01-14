﻿using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Wirebug;

internal class WirebugContextHandler : WirebugViewModel, IContextHandler
{
    public readonly MHRWirebug Context;

    public WirebugContextHandler(MHRWirebug context)
    {
        Context = context;
        
        UpdateData();
        HookEvents();
    }

    public void HookEvents()
    {
        Context.OnCooldownUpdate += OnCooldownUpdate;
        Context.OnTimerUpdate += OnTimerUpdate;
        Context.OnAvailable += OnAvailable;
        Context.OnBlockedStateChange += OnBlockedStateChange;
        Context.OnWirebugConditionChange += OnWirebugConditionChange;
    }

    public void UnhookEvents()
    {
        Context.OnCooldownUpdate -= OnCooldownUpdate;
        Context.OnTimerUpdate -= OnTimerUpdate;
        Context.OnAvailable -= OnAvailable;
        Context.OnBlockedStateChange -= OnBlockedStateChange;
        Context.OnWirebugConditionChange -= OnWirebugConditionChange;
    }

    private void OnBlockedStateChange(object sender, MHRWirebug e) => IsBlocked = e.IsBlocked;

    private void OnWirebugConditionChange(object sender, MHRWirebug e) => WirebugCondition = e.WirebugCondition;

    private void OnTimerUpdate(object sender, MHRWirebug e)
    {
        MaxTimer = e.MaxTimer;
        Timer = e.Timer;
        IsTemporary = e.Timer > 0;
    }

    private void OnAvailable(object sender, MHRWirebug e) => IsAvailable = e.IsAvailable;

    private void OnCooldownUpdate(object sender, MHRWirebug e)
    {
        MaxCooldown = e.MaxCooldown;
        Cooldown = e.Cooldown;
        OnCooldown = e.Cooldown > 0;
    }

    private void UpdateData()
    {
        IsBlocked = Context.IsBlocked;
        WirebugCondition = Context.WirebugCondition;
        MaxCooldown = Context.MaxCooldown == 0 ? 400 : Context.MaxCooldown;
        Cooldown = Context.Cooldown;
        OnCooldown = Context.Cooldown > 0;
        IsAvailable = Context.IsAvailable;

        MaxTimer = Context.MaxTimer;
        Timer = Context.Timer;
        IsTemporary = Context.Timer > 0;
    }
}
