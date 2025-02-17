using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : IGameSystem
{
    private Queue<ICommand> PrepareQueue = new Queue<ICommand>();

    // Stack of command objects to undo
    private Stack<ICommand> UndoStack = new Stack<ICommand>();

    // Second stack of redoable commands
    private Stack<ICommand> RedoStack = new Stack<ICommand>();

    private bool initiated;
    public SystemType TypeEnum
    {
        get
        {
            return SystemType.Command;
        }
    }


    public void AddCommand(ICommand command)
    {
        PrepareQueue.Enqueue(command);
    }

    // Execute a command object directly and save to the undo stack
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        UndoStack.Push(command);

        // Clear out the redo stack if we make a new move
        RedoStack.Clear();
    }

    public void UndoCommand()
    {
        // If we have commands to undo
        if (UndoStack.Count > 0)
        {
            ICommand activeCommand = UndoStack.Pop();
            RedoStack.Push(activeCommand);
            activeCommand.Undo();
        }
    }

    public void RedoCommand()
    {
        // If we have commands to redo
        if (RedoStack.Count > 0)
        {
            ICommand activeCommand = RedoStack.Pop();
            UndoStack.Push(activeCommand);
            activeCommand.Execute();
        }
    }

    public void Setup()
    {
        initiated = true;
    }

    public void Tick(float deltaTime)
    {
        if (!initiated) return;

        while (PrepareQueue.Count > 0)
        {
            ExecuteCommand(PrepareQueue.Dequeue());
        }


    }

    public void Dispose()
    {
        initiated = false;
    }
}
