public static class StateMachineFactory {

    public static StateMachine CreateStateMachine(Unit unit, UnitType unitType) {
        StateMachine stateMachine = new StateMachine(unit);
        switch (unitType) {
            case UnitType.Human:
                break;
            case UnitType.Zombie:
                stateMachine.AddState(new StateFollowSmell(unit));
                stateMachine.AddState(new StateMoveToEnemy(unit));
                break;
        }
        return stateMachine;
    }
}
