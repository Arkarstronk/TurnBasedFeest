namespace TurnBasedFeest.BattleEvents.Actions
{
    interface IContinueableAction
    {
        bool HasNextEvent();
        ITurnEvent NextEvent();        
    }
}
