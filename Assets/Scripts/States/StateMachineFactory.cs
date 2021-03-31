public static class StateMachineFactory {

    public static StateMachine CreateStateMachine(Unit unit, UnitType unitType) {
        StateMachine stateMachine = new StateMachine(unit);
        switch (unitType) {
            case UnitType.Human:
            case UnitType.Human1:
            case UnitType.Human2:
                stateMachine.AddState(new StateFollowTarget(unit, unit.Animator));
                stateMachine.AddState(new StateAttackZombies(unit));
                break;
            case UnitType.Zombie:
                stateMachine.AddState(new StateFollowSmell(unit, unit.Animator));
                stateMachine.AddState(new StateMoveToEnemy(unit, unit.Animator));
                stateMachine.AddState(new StateMeleeAttack(unit, unit.Animator));
                break;
        }
        return stateMachine;
    }
}
