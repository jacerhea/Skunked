namespace Skunked.Commands
{
    /// <summary>
    /// Client's will execute commands against the game invoker.
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
