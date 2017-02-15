﻿using System;
using HA4IoT.Actuators.StateMachines;
using HA4IoT.Contracts.Actuators;
using HA4IoT.Contracts.Sensors;

namespace HA4IoT.Actuators.Connectors
{
    public static class StateMachineWithButtonConnector
    {
        public static IStateMachine ConnectMoveNextAndToggleOffWith(this IStateMachine stateMachineActuator, IButton button)
        {
            throw new NotSupportedException();
            //button.PressedShortlyTrigger.Attach(stateMachineActuator.SetNextState);

            if (stateMachineActuator.GetSupportsOffState())
            {
                button.PressedLongTrigger.Attach(() => stateMachineActuator.ChangeState(BinaryStateId.Off));
            }

            return stateMachineActuator;
        }
    }
}
