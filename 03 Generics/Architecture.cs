// Вставьте сюда финальное содержимое файла Architecture.cs
using System;
using System.Collections.Generic;

namespace Generics.Robots
{
    public interface IRobotAI<out T>
    {
        T GetCommand();
    }

    public abstract class RobotAI<TCommand> : IRobotAI<TCommand>
    {
        public abstract TCommand GetCommand();
    }

    public class ShooterAI : RobotAI<ShooterCommand>
    {
        int counter = 1;

        public override ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : RobotAI<BuilderCommand>
    {
        int counter = 1;

        public override BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface IDevice<in T>
    {
        string ExecuteCommand(T command);
    }

    public abstract class Device<TCommand> : IDevice<TCommand> 
    {
        public abstract string ExecuteCommand(TCommand command);
    }

    public class Mover : Device<IMoveCommand>
    {
        public override string ExecuteCommand(IMoveCommand _command)
        { 
            var command = _command;
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class ShooterMover : Device<IShooterMoveCommand>
    {
        public override string ExecuteCommand(IShooterMoveCommand _command)
        {
            var command = _command;
            var hide = command.ShouldHide ? "YES" : "NO";
            return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
        }
    }

    public class Robot<TCommand> where TCommand : IMoveCommand
    {
        private readonly IRobotAI<TCommand> ai;
        private readonly IDevice<TCommand> device;

        public Robot(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command);
            }
        }
    }

    public static class Robot
    {
        public static Robot<T> Create<T>(IRobotAI<T> ai, IDevice<T> executor) where T : IMoveCommand
        {
            return new Robot<T>(ai, executor);
        }
    }
}
