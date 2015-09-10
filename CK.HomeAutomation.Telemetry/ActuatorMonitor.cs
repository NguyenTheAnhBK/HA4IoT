﻿using System;
using System.Diagnostics;
using CK.HomeAutomation.Actuators;
using CK.HomeAutomation.Notifications;

namespace CK.HomeAutomation.Telemetry
{
    public abstract class ActuatorMonitor
    {
        protected ActuatorMonitor(INotificationHandler notificationHandler)
        {
            if (notificationHandler == null) throw new ArgumentNullException(nameof(notificationHandler));

            NotificationHandler = notificationHandler;
        }

        protected INotificationHandler NotificationHandler { get; }

        public void ConnectActuators(Home home)
        {
            if (home == null) throw new ArgumentNullException(nameof(home));

            foreach (var actuator in home.Actuators.Values)
            {
                var binaryStateOutput = actuator as IBinaryStateOutputActuator;
                if (binaryStateOutput != null)
                {
                    HandleBinaryStateOutputActuator(binaryStateOutput);
                    continue;
                }

                var stateMachineOutput = actuator as StateMachine;
                if (stateMachineOutput != null)
                {
                    HandleStateMachineOutputActuator(stateMachineOutput);
                    continue;
                }

                var sensor = actuator as BaseSensor;
                if (sensor != null)
                {
                    sensor.ValueChanged += (s, e) =>
                    {
                        OnSensorValueChanged(sensor);
                    };

                    continue;
                }

                var motionDetector = actuator as MotionDetector;
                if (motionDetector != null)
                {
                    motionDetector.MotionDetected += (s, e) =>
                    {
                        OnMotionDetected(motionDetector);
                    };

                    continue;
                }

                var button = actuator as IButton;
                if (button != null)
                {
                    button.PressedShort += (s, e) =>
                    {
                        OnButtonPressed(button, ButtonPressedDuration.Short);
                    };

                    button.PressedLong += (s, e) =>
                    {
                        OnButtonPressed(button, ButtonPressedDuration.Long);
                    };
                }
            }
        }

        protected virtual void OnMotionDetected(MotionDetector motionDetector)
        {
        }

        protected virtual void OnButtonPressed(IButton button, ButtonPressedDuration duration)
        {
        }

        protected virtual void OnBinaryStateActuatorStateChanged(IBinaryStateOutputActuator actuator, TimeSpan previousStateDuration)
        {
        }

        protected virtual void OnStateMachineStateChanged(StateMachine stateMachine, StateMachineStateChangedEventArgs eventArgs, TimeSpan previousStateDuration)
        {
        }

        protected virtual void OnSensorValueChanged(BaseSensor sensor)
        {
        }

        private void HandleBinaryStateOutputActuator(IBinaryStateOutputActuator binaryStateOutputActuator)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            binaryStateOutputActuator.StateChanged += (s, e) =>
            {
                TimeSpan previousStateDuration = stopwatch.Elapsed;
                stopwatch.Restart();

                OnBinaryStateActuatorStateChanged(binaryStateOutputActuator, previousStateDuration);
            };
        }

        private void HandleStateMachineOutputActuator(StateMachine stateMachine)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stateMachine.StateChanged += (s, e) =>
            {
                TimeSpan previousStateDuration = stopwatch.Elapsed;
                stopwatch.Restart();

                OnStateMachineStateChanged(stateMachine, e, previousStateDuration);
            };
        }
    }
}
