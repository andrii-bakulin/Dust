namespace Dust.Experimental.Variables
{
    public abstract class NumericVariable : AbstractVariable
    {
        public enum Action
        {
            Set = 0,

            Add = 10,
            Subtract = 11,
            Multiply = 12,
            Divide = 13,
        }
        
        public abstract bool Execute(Action action, string param);
    }
}
