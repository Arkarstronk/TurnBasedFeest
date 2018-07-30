namespace TurnBasedFeest.BattleEvents.Actions
{
    interface ContinueableAction
    {
        bool HasNextEvent();
        ITurnEvent NextEvent();        
    }
}
